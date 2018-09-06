using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace Craft.Net.Auth
{
    public class Yggdrasil
    {

        /// <summary>
        /// Allows to login to a premium Minecraft account using the Yggdrasil authentication scheme.
        /// </summary>
        /// <param name="user">Login</param>
        /// <param name="pass">Password</param>
        /// <param name="accesstoken">Will contain the access token returned by Minecraft.net, if the login is successful</param>
        /// <param name="clienttoken">Will contain the client token generated before sending to Minecraft.net</param>
        /// <param name="uuid">Will contain the player's PlayerID, needed for multiplayer</param>
        /// <returns>Returns the status of the login (Success, Failure, etc.)</returns>

        public static LoginResult GetLogin(string user, string pass, out SessionToken session)
        {
            session = new SessionToken() { ClientID = Guid.NewGuid().ToString().Replace("-", "") };

            try
            {
                string result = "";

                string json_request = "{\"agent\": { \"name\": \"Minecraft\", \"version\": 1 }, \"username\": \"" + JsonEncode(user) + "\", \"password\": \"" + JsonEncode(pass) + "\", \"clientToken\": \"" + JsonEncode(session.ClientID) + "\" }";
                int code = DoHTTPSPost("authserver.mojang.com", "/authenticate", json_request, ref result);
                if (code == 200)
                {
                    if (result.Contains("availableProfiles\":[]}"))
                    {
                        return LoginResult.NotPremium;
                    }
                    else
                    {
                        string[] temp = result.Split(new string[] { "accessToken\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (temp.Length >= 2) { session.ID = temp[1].Split('"')[0]; }
                        temp = result.Split(new string[] { "name\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (temp.Length >= 2) { session.PlayerName = temp[1].Split('"')[0]; }
                        temp = result.Split(new string[] { "availableProfiles\":[{\"id\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (temp.Length >= 2) { session.PlayerID = temp[1].Split('"')[0]; }
                        return LoginResult.Success;
                    }
                }
                else if (code == 403)
                {
                    if (result.Contains("UserMigratedException"))
                    {
                        return LoginResult.AccountMigrated;
                    }
                    else return LoginResult.WrongPassword;
                }
                else if (code == 503)
                {
                    return LoginResult.ServiceUnavailable;
                }
                else
                {
                    Console.WriteLine("Got error code from server: " + code);
                    return LoginResult.OtherError;
                }
            }
            catch (System.Security.Authentication.AuthenticationException)
            {
                return LoginResult.SSLError;
            }
            catch (System.IO.IOException e)
            {
                if (e.Message.Contains("authentication"))
                {
                    return LoginResult.SSLError;
                }
                else return LoginResult.OtherError;
            }
            catch
            {
                return LoginResult.OtherError;
            }
        }

        /// <summary>
        /// Validates whether accessToken must be refreshed
        /// </summary>
        /// <param name="accesstoken">Will contain the cached access token previously returned by Minecraft.net</param>
        /// <param name="clienttoken">Will contain the cached client token created on login</param>
        /// <returns>Returns the status of the token (Valid, Invalid, etc.)</returns>
        ///
        public static LoginResult GetTokenValidation(SessionToken session)
        {
            try
            {
                string result = "";
                string json_request = "{\"accessToken\": \"" + JsonEncode(session.ID) + "\", \"clientToken\": \"" + JsonEncode(session.ClientID) + "\" }";
                int code = DoHTTPSPost("authserver.mojang.com", "/validate", json_request, ref result);
                if (code == 204)
                {
                    return LoginResult.Success;
                }
                else if (code == 403)
                {
                    return LoginResult.LoginRequired;
                }
                else
                {
                    return LoginResult.OtherError;
                }
            }
            catch
            {
                return LoginResult.OtherError;
            }
        }

        /// <summary>
        /// Refreshes invalid token
        /// </summary>
        /// <param name="user">Login</param>
        /// <param name="accesstoken">Will contain the new access token returned by Minecraft.net, if the refresh is successful</param>
        /// <param name="clienttoken">Will contain the client token generated before sending to Minecraft.net</param>
        /// <param name="uuid">Will contain the player's PlayerID, needed for multiplayer</param>
        /// <returns>Returns the status of the new token request (Success, Failure, etc.)</returns>
        ///
        public static LoginResult GetNewToken(SessionToken currentsession, out SessionToken newsession)
        {
            newsession = new SessionToken();
            try
            {
                string result = "";
                string json_request = "{ \"accessToken\": \"" + JsonEncode(currentsession.ID) + "\", \"clientToken\": \"" + JsonEncode(currentsession.ClientID) + "\", \"selectedProfile\": { \"id\": \"" + JsonEncode(currentsession.PlayerID) + "\", \"name\": \"" + JsonEncode(currentsession.PlayerName) + "\" } }";
                int code = DoHTTPSPost("authserver.mojang.com", "/refresh", json_request, ref result);
                if (code == 200)
                {
                    if (result == null)
                    {
                        return LoginResult.NullError;
                    }
                    else
                    {
                        string[] temp = result.Split(new string[] { "accessToken\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (temp.Length >= 2) { newsession.ID = temp[1].Split('"')[0]; }
                        temp = result.Split(new string[] { "clientToken\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (temp.Length >= 2) { newsession.ClientID = temp[1].Split('"')[0]; }
                        temp = result.Split(new string[] { "name\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (temp.Length >= 2) { newsession.PlayerName = temp[1].Split('"')[0]; }
                        temp = result.Split(new string[] { "selectedProfile\":[{\"id\":\"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (temp.Length >= 2) { newsession.PlayerID = temp[1].Split('"')[0]; }
                        return LoginResult.Success;
                    }
                }
                else if (code == 403 && result.Contains("InvalidToken"))
                {
                    return LoginResult.InvalidToken;
                }
                else
                {
                    Console.WriteLine("Got error code from server while refreshing authentication: " + code);
                    return LoginResult.OtherError;
                }
            }
            catch
            {
                return LoginResult.OtherError;
            }
        }

        /// <summary>
        /// Check session using Mojang's Yggdrasil authentication scheme. Allows to join an online-mode server
        /// </summary>
        /// <param name="user">Username</param>
        /// <param name="accesstoken">Session ID</param>
        /// <param name="serverhash">Server ID</param>
        /// <returns>TRUE if session was successfully checked</returns>

        public static bool SessionCheck(string uuid, string accesstoken, string serverhash)
        {
            try
            {
                string result = "";
                string json_request = "{\"accessToken\":\"" + accesstoken + "\",\"selectedProfile\":\"" + uuid + "\",\"serverId\":\"" + serverhash + "\"}";
                int code = DoHTTPSPost("sessionserver.mojang.com", "/session/minecraft/join", json_request, ref result);
                return (result == "");
            }
            catch { return false; }
        }

        /// <summary>
        /// Make a HTTPS GET request to the specified endpoint of the Mojang API
        /// </summary>
        /// <param name="host">Host to connect to</param>
        /// <param name="endpoint">Endpoint for making the request</param>
        /// <param name="cookies">Cookies for making the request</param>
        /// <param name="result">Request result</param>
        /// <returns>HTTP Status code</returns>

        private static int DoHTTPSGet(string host, string endpoint, string cookies, ref string result)
        {
            List<String> http_request = new List<string>
            {
                "GET " + endpoint + " HTTP/1.1",
                "Cookie: " + cookies,
                "Cache-Control: no-cache",
                "Pragma: no-cache",
                "Host: " + host,
                "User-Agent: Java/1.6.0_27",
                "Accept-Charset: ISO-8859-1,UTF-8;q=0.7,*;q=0.7",
                "Connection: close",
                ""
            };
            return DoHTTPSRequest(http_request, host, ref result);
        }

        /// <summary>
        /// Make a HTTPS POST request to the specified endpoint of the Mojang API
        /// </summary>
        /// <param name="host">Host to connect to</param>
        /// <param name="endpoint">Endpoint for making the request</param>
        /// <param name="request">Request payload</param>
        /// <param name="result">Request result</param>
        /// <returns>HTTP Status code</returns>

        private static int DoHTTPSPost(string host, string endpoint, string request, ref string result)
        {
            List<String> http_request = new List<string>
            {
                "POST " + endpoint + " HTTP/1.1",
                "Host: " + host,
                "User-Agent: CAC/1.0",
                "Content-Type: application/json",
                "Content-Length: " + Encoding.ASCII.GetBytes(request).Length,
                "Connection: close",
                "",
                request
            };
            return DoHTTPSRequest(http_request, host, ref result);
        }

        /// <summary>
        /// Manual HTTPS request since we must directly use a TcpClient because of the proxy.
        /// This method connects to the server, enables SSL, do the request and read the response.
        /// </summary>
        /// <param name="headers">Request headers and optional body (POST)</param>
        /// <param name="host">Host to connect to</param>
        /// <param name="result">Request result</param>
        /// <returns>HTTP Status code</returns>

        private static int DoHTTPSRequest(List<string> headers, string host, ref string result)
        {
            string postResult = null;
            int statusCode = 520;
            TcpClient client = new TcpClient(host, 443);
            SslStream stream = new SslStream(client.GetStream());
            stream.AuthenticateAsClient(host);
            stream.Write(Encoding.ASCII.GetBytes(String.Join("\r\n", headers.ToArray())));
            System.IO.StreamReader sr = new System.IO.StreamReader(stream);
            string raw_result = sr.ReadToEnd();
            if (raw_result.StartsWith("HTTP/1.1"))
            {
                postResult = raw_result.Substring(raw_result.IndexOf("\r\n\r\n") + 4);
                statusCode = int.Parse(raw_result.Split(' ')[1]);
            }
            else statusCode = 520; //Web server is returning an unknown error
            result = postResult;
            return statusCode;
        }

        /// <summary>
        /// Encode a string to a json string.
        /// Will convert special chars to \u0000 unicode escape sequences.
        /// </summary>
        /// <param name="text">Source text</param>
        /// <returns>Encoded text</returns>

        private static string JsonEncode(string text)
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in text)
            {
                if ((c >= '0' && c <= '9') ||
                    (c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z'))
                {
                    result.Append(c);
                }
                else
                {
                    result.AppendFormat(@"\u{0:x4}", (int)c);
                }
            }

            return result.ToString();
        }
    }
}