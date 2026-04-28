using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _1x6Helper.Models.Api;


public class Dota1x6ShortMatch
{
    [JsonPropertyName("data")]
    public DataContainer? Data { get; set; }

    [JsonPropertyName("errors")]
    public List<object>? Errors { get; set; }

    [JsonPropertyName("hasErrors")]
    public bool? HasErrors { get; set; }

    public class DataContainer
    {
        [JsonPropertyName("matchId")]
        public long MatchId { get; set; }

        [JsonPropertyName("mapName")]
        public string? MapName { get; set; }

        [JsonPropertyName("createdAtUtc")]
        public DateTime? CreatedAtUtc { get; set; }

        [JsonPropertyName("isFinished")]
        public bool? IsFinished { get; set; }

        [JsonPropertyName("isDuo")]
        public bool? IsDuo { get; set; }

        [JsonPropertyName("endAtSeconds")]
        public int? EndAtSeconds { get; set; }

        [JsonPropertyName("heroes")]
        public List<string>? Heroes { get; set; }
    }
}
