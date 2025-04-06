using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyMovies.Core.Models
{
    public class ApiResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }


        [JsonPropertyName("status_message")]
        public string StatusMessage { get; set; }


        [JsonPropertyName("data")]
        public ApiData ApiData { get; set; }
    }
}
