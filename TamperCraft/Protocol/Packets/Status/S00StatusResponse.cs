using Craft.Net;
using Craft.Net.Packets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamperCraft.Protocol.Packets.Status
{
    public class S00StatusResponse : IPacket
    {
        public StatusResponse StatusResponse;

        public S00StatusResponse(StatusResponse statusResponse)
        {
            StatusResponse = statusResponse;
        }

        public int GetId()
        {
            return 0x00;
        }

        public void Handle(NetHandler netHandler)
        {
            throw new NotImplementedException();
        }

        public void Receive(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void Send(PacketBuffer buffer)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            buffer.WriteString(JsonConvert.SerializeObject(StatusResponse, settings));
        }
    }
}
