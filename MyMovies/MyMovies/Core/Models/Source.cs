using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyMovies.Core.Models
{
    public class Source
    {
        private string infoHash;

        [JsonPropertyName("name")]
        public string Name { get; set; }


        [JsonPropertyName("title")]
        public string Title { get; set; }


        [JsonPropertyName("infoHash")]
        public string InfoHash
        {
            get { return infoHash; }
            set { infoHash = $"magnet:?xt=urn:btih:{value}&dn=Url+Encoded+Movie+Name&tr=http://track.one:1234/announce&tr=udp://track.two:80&tr=udp://open.demonii.com:1337/announce&tr=udp://tracker.openbittorrent.com:80&tr=udp://tracker.coppersurfer.tk:6969&tr=udp://glotorrents.pw:6969/announce&tr=udp://tracker.opentrackr.org:1337/announce&tr=udp://torrent.gresille.org:80/announce&tr=udp://p4p.arenabg.com:1337&tr=udp://tracker.leechers-paradise.org:6969"; }
        }


        [JsonPropertyName("behaviorHints")]
        public BehaviorHints Metadata { get; set; }

        public class BehaviorHints
        {
            [JsonPropertyName("bingeGroup")]
            public string Formats { get; set; }
        }
    }
}
