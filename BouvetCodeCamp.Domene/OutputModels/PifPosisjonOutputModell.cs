﻿using System;

namespace BouvetCodeCamp.Domene.OutputModels
{
    using Newtonsoft.Json;

    public class PifPosisjonOutputModell
    {
        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "lagId")]
        public string LagId { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public DateTime? Tid { get; set; }
    }
}