using Craft.Net.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using TamperCraft.Util;

namespace TamperCraft.Login
{
    public class CredentialManager
    {
        private List<MojangCredential> mojangCredentials;
        private const string fileName = "sessionstore.bin";

        public LoginResult LogIn(string username, string password, out SessionToken token)
        {
            token = null;
            if (mojangCredentials == null)
                if (File.Exists(fileName))
                {
                    var bf = new BinaryFormatter();
                    using (var stream = OpenFile(StreamMode.Read))
                    {
                        mojangCredentials = (List<MojangCredential>)bf.Deserialize(stream);
                    }
                }
                else mojangCredentials = new List<MojangCredential>();

            var passwordHash = password.ToBytes().Hash();
            foreach (var credential in mojangCredentials)
                if (credential.Email == username && credential.PasswordHash.EqualsArray(passwordHash))
                    token = credential.LastToken;

            if (token != null)
            {
                if (Yggdrasil.GetTokenValidation(token) == LoginResult.Success) return LoginResult.Success; else Console.WriteLine("Token validation failed");
                if (Yggdrasil.GetNewToken(token, out token) == LoginResult.Success)
                {
                    UpdateToken(username, token);
                    return LoginResult.Success;
                }
                else Console.WriteLine("Token renew failed");
            }

            var login = Yggdrasil.GetLogin(username, password, out token);
            if (login != LoginResult.Success)
            {
                RemoveToken(username);
                token = null;
                return login;
            }
            else UpdateToken(username, token);

            AddToken(username, passwordHash, token);

            return LoginResult.Success;
        }

        private void AddToken(string email, byte[] pwHash, SessionToken tok)
        {
            bool found = false;
            foreach (var cred in mojangCredentials)
                if (cred.Email == email) { 
                    cred.LastToken = tok;
                    found = true;
                }
            if (!found) mojangCredentials.Add(new MojangCredential(email, pwHash, tok));
            Save();
        }

        private void UpdateToken(string email, SessionToken token)
        {
            foreach(var cred in mojangCredentials)
                if (cred.Email == email)
                    cred.LastToken = token;
            Save();
        }

        private void RemoveToken(string email)
        {
            foreach(var cred in mojangCredentials)
                if(cred.Email == email)
                {
                    mojangCredentials.Remove(cred);
                    break;
                }
            Save();
        }

        private void Save()
        {
            var bf = new BinaryFormatter();
            using (var stream = OpenFile(StreamMode.Write))
            {
                bf.Serialize(stream, mojangCredentials);
            }
        }

        private Stream OpenFile(StreamMode mode)
        {
            var fileStream = File.Open(fileName, mode == StreamMode.Read ? FileMode.Open : FileMode.Create, mode == StreamMode.Read ? FileAccess.Read : FileAccess.ReadWrite);
            var gzipStream = new GZipStream(fileStream, mode == StreamMode.Read ? CompressionMode.Decompress : CompressionMode.Compress);
            return gzipStream;
        }

        private enum StreamMode
        {
            Read,
            Write
        }
    }
}
