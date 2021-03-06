﻿namespace Bouvet.BouvetBattleRoyale.Domene.OutputModels
{
    using Bouvet.BouvetBattleRoyale.Domene.Entiteter;

    using Newtonsoft.Json;

    public class PostOutputModell
    {
        [JsonProperty(PropertyName = "navn")]
        public string Navn { get; set; }

        [JsonProperty(PropertyName = "gps")]
        public Koordinat Posisjon { get; set; }

        [JsonProperty(PropertyName = "postNummer")]
        public int Nummer { get; set; }

        public PostOutputModell()
        {
            Navn = string.Empty;
            Posisjon = Koordinat.Empty;
            Nummer = 0;
        }
    }
}