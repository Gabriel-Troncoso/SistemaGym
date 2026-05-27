using System.Net;
using System.Text.Json.Serialization;

namespace SistemaGym.Core.CustomEntities
{
    public class ResponseData
    {
        public PagedList<object> Pagination { get; set; } = null!;

        public Message[] Messages { get; set; } = Array.Empty<Message>();

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }
    }
}
