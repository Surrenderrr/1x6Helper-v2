using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
//https://stats.dota1x6.com/api/v2/matches/?dotaMatchId=8660990978

namespace _1x6Helper.Models.Api
{
    public class Dota1x6Match
    {
        // Root properties
        [JsonPropertyName("data")]
        public DataContainer? Data { get; set; }

        [JsonPropertyName("errors")]
        public List<object>? Errors { get; set; }

        [JsonPropertyName("hasErrors")]
        public bool HasErrors { get; set; }

        // Nested classes as inner classes
        public class DataContainer
        {
            [JsonPropertyName("meta")]
            public MatchMeta? Meta { get; set; }

            [JsonPropertyName("players")]
            public List<Player>? Players { get; set; }

            [JsonPropertyName("picks")]
            public List<Pick>? Picks { get; set; }

            [JsonPropertyName("buildingsDestroyed")]
            public List<BuildingDestroyed>? BuildingsDestroyed { get; set; }
        }

        public class MatchMeta
        {
            [JsonPropertyName("map")]
            public string? Map { get; set; }

            [JsonPropertyName("season")]
            public string? Season { get; set; }

            [JsonPropertyName("createdAt")]
            public DateTime CreatedAt { get; set; }

            [JsonPropertyName("isFinished")]
            public bool? IsFinished { get; set; }

            [JsonPropertyName("isDuo")]
            public bool? IsDuo { get; set; }

            [JsonPropertyName("durationAtSeconds")]
            public int? DurationAtSeconds { get; set; }
        }

        public class Player
        {
            [JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonPropertyName("teamId")]
            public long? TeamId { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("talents")]
            public Talents? Talents { get; set; }

            [JsonPropertyName("isFinished")]
            public bool? IsFinished { get; set; }

            [JsonPropertyName("heroName")]
            public string? HeroName { get; set; }

            [JsonPropertyName("ratingStart")]
            public int? RatingStart { get; set; }

            [JsonPropertyName("ratingChange")]
            public int? RatingChange { get; set; }

            [JsonPropertyName("place")]
            public int? Place { get; set; }

            [JsonPropertyName("isDoubleRating")]
            public bool? IsDoubleRating { get; set; }

            [JsonPropertyName("kills")]
            public int? Kills { get; set; }

            [JsonPropertyName("deaths")]
            public int? Deaths { get; set; }

            [JsonPropertyName("assists")]
            public int? Assists { get; set; }

            [JsonPropertyName("endAtSeconds")]
            public int? EndAtSeconds { get; set; }

            [JsonPropertyName("items")]
            public List<string>? Items { get; set; }

            [JsonPropertyName("towerPosition")]
            public int? TowerPosition { get; set; }

            [JsonPropertyName("level")]
            public int? Level { get; set; }

            [JsonPropertyName("networth")]
            public int? Networth { get; set; }

            [JsonPropertyName("lastHits")]
            public int? LastHits { get; set; }

            [JsonPropertyName("towerDamage")]
            public int? TowerDamage { get; set; }

            [JsonPropertyName("aggregatedDealtDamage")]
            public DamageStats? AggregatedDealtDamage { get; set; }

            [JsonPropertyName("aggregatedReceivedDamage")]
            public DamageStats? AggregatedReceivedDamage { get; set; }

            [JsonPropertyName("receivedDamages")]
            public List<ReceivedDamage>? ReceivedDamages { get; set; }
            public string Kda => $"{Kills ?? 0}/{Deaths ?? 0}/{Assists ?? 0}";
            public int DamageDealt => AggregatedDealtDamage?.Total ?? 0;
            public string DisplayRatingChange => (RatingChange >= 0 ? "+" : "") + RatingChange;
            public string RatingColor => (RatingChange >= 0) ? "#4caf50" : "#f44336";
            [JsonIgnore]
            public Bitmap? HeroIcon { get; set; }

            [JsonIgnore]
            public Bitmap? TalentIcon { get; set; }
            [JsonIgnore]
            public Bitmap? PlayerIcon { get; set; }
        }

        public class Talents
        {
            [JsonPropertyName("main")]
            public string? Main { get; set; }

            [JsonPropertyName("secondMain")]
            public string? SecondMain { get; set; }

            [JsonPropertyName("blueCount")]
            public int? BlueCount { get; set; }

            [JsonPropertyName("whiteCount")]
            public int? WhiteCount { get; set; }

            [JsonPropertyName("purpleCount")]
            public int? PurpleCount { get; set; }
        }

        public class DamageStats
        {
            [JsonPropertyName("total")]
            public int? Total { get; set; }

            [JsonPropertyName("physical")]
            public int? Physical { get; set; }

            [JsonPropertyName("magical")]
            public int? Magical { get; set; }

            [JsonPropertyName("pure")]
            public int? Pure { get; set; }
        }

        public class ReceivedDamage
        {
            [JsonPropertyName("heroName")]
            public string? HeroName { get; set; }

            [JsonPropertyName("damage")]
            public DamageStats? Damage { get; set; }
        }

        public class Pick
        {
            [JsonPropertyName("heroName")]
            public string? HeroName { get; set; }

            [JsonPropertyName("number")]
            public int? Number { get; set; }

            [JsonPropertyName("ban")]
            public string? Ban { get; set; }
        }

        public class BuildingDestroyed
        {
            [JsonPropertyName("pusherHeroes")]
            public List<string>? PusherHeroes { get; set; }

            [JsonPropertyName("pushedAtSeconds")]
            public double? PushedAtSeconds { get; set; }

            [JsonPropertyName("pushedHeroes")]
            public List<string>? PushedHeroes { get; set; }

            [JsonPropertyName("type")]
            public string? Type { get; set; }
        }
    }
}
