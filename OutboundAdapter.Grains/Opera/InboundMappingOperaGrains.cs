using Orleans;
using OutboundAdapter.Interfaces.Opera;
using OutboundAdapter.Interfaces.Opera.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OutboundAdapter.Grains.Opera
{
    public class InboundMappingOperaGrains : Grain, IInboundMappingGrains
    {
        Task<RoomStatusUpdateBERequestDto> IInboundMappingGrains.MapRoomStatusUpdateBE(string message)
        {
            RoomStatusUpdateBERequestDto roomStatusUpdateBeRequestDto;
            var doc = XDocument.Parse(message.Trim());
            var reservationNodes = doc.Descendants().Where(o => o.Name.LocalName == "RoomStatusUpdateBERequest");
            var content = reservationNodes.First().ToString();

            var serializer = new XmlSerializer(typeof(RoomStatusUpdateBERequestDto));
            using (var reader = new StringReader(content))
            {
                roomStatusUpdateBeRequestDto = (RoomStatusUpdateBERequestDto)serializer.Deserialize(reader);
            }

            return Task.FromResult(roomStatusUpdateBeRequestDto);
        }
    }
}
