﻿using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.Domene.InputModels
{
    public class KodeModel
    {
        public string Kode { get; set; }
        public string LagId { get; set; }
        public Koordinat Koordinat { get; set; }

        public KodeModel()
        {
            Kode = string.Empty;
            LagId = string.Empty;
            Koordinat = Koordinat.Empty;
        }
    }
}
