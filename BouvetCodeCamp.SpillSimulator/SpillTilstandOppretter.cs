﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BouvetCodeCamp.Domene.Entiteter;

namespace BouvetCodeCamp.SpillSimulator
{
    public class SpillTilstandOppretter
    {
        public SpillTilstandOppretter()
        {
            OpprettKoordinater();
            OpprettPostKoder();
        }

        private static void OpprettKoordinater()
        {
            SpillKonfig.Koordinater = new List<Koordinat>
            {
                new Koordinat
                {
                    Latitude = "59.67878",
                    Longitude = "10.60392"
                },
                new Koordinat
                {
                    Latitude = "59.67944",
                    Longitude = "10.6042"
                },
                new Koordinat
                {
                    Latitude = "59.68023411",
                    Longitude = "10.6041259971"
                },
                new Koordinat
                {
                    Latitude = "59.6804558114",
                    Longitude = "10.60457",
                }
            };
        }

        private void OpprettPostKoder()
        {
            SpillKonfig.PostKoder = new Dictionary<int, string>
            {
                {24, "dxg19"},
                {21, "exr16"},
                {23, "rxp18"},
                {1, "oxx2"},
                {8, "lxr9"},
                {2, "fxo3"}
            };
        }
    }
}