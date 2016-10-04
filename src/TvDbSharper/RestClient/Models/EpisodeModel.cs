﻿namespace TvDbSharper.RestClient.Models
{
    public class EpisodeModel
    {
        public int? AbsoluteNumber { get; set; }

        public int? AiredEpisodeNumber { get; set; }

        public int? AiredSeason { get; set; }

        public int? DvdEpisodeNumber { get; set; }

        public int? DvdSeason { get; set; }

        public string EpisodeName { get; set; }

        public int? Id { get; set; }

        public string Overview { get; set; }
    }
}