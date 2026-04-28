using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace _1x6Helper.Models.Api
{
    public class PlayerStats
    {
        [JsonPropertyName("data")]
        public PlayerData? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<object>? Errors { get; set; }

        [JsonPropertyName("hasErrors")]
        public bool HasErrors { get; set; }

        public class PlayerData
        {
            public long PlayerId { get; set; }
            public int Rating { get; set; }
            public int DuoRating { get; set; }
            public double AvgGpm { get; set; }
            public double AvgPlace { get; set; }
            public int MatchCount { get; set; }
            public Dictionary<string, double>? Places { get; set; }
            public int FirstPlaces { get; set; }
            public string? FavoriteHero { get; set; }
            public SocialInfo? Social { get; set; }
        }
        public class SocialInfo
        {
            public string? Discord { get; set; }
            public string? DiscordUserName { get; set; }
            public string? Youtube { get; set; }
            public bool? IsYoutubeLive { get; set; }
            public string? Twitch { get; set; }
            public bool? IsTwitchLive { get; set; }
        }
    }
}
