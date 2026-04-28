using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
//https://stats.dota1x6.com/api/v2/matches/history?playerId=321758292&page=1&count=2
namespace _1x6Helper.Models.Api;

public class History
{   
        [JsonPropertyName("data")]
        public MatchHistoryData? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<object>? Errors { get; set; }

        [JsonPropertyName("hasErrors")]
        public bool HasErrors { get; set; }
    

    public class MatchHistoryData
    {
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("isLastPage")]
        public bool IsLastPage { get; set; }

        [JsonPropertyName("values")]
        public List<MatchInfo>? Values { get; set; }
    }

    public class MatchInfo
    {
        [JsonPropertyName("matchId")]
        public long MatchId { get; set; }

        [JsonPropertyName("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; }

        [JsonPropertyName("isFinished")]
        public bool IsFinished { get; set; }

        [JsonPropertyName("heroName")]
        public string? HeroName { get; set; }

        [JsonPropertyName("talent")]
        public string? Talent { get; set; }

        [JsonPropertyName("place")]
        public int? Place { get; set; }

        [JsonPropertyName("ratingStart")]
        public int RatingStart { get; set; }

        [JsonPropertyName("ratingChange")]
        public int? RatingChange { get; set; }

        [JsonPropertyName("isDoubleRating")]
        public bool? IsDoubleRating { get; set; }

        [JsonPropertyName("mapName")]
        public string? MapName { get; set; }
        [JsonIgnore]
        public int Number { get; set; }
    }

}
