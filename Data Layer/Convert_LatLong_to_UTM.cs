﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Oware
{
    public class LatLngUTMConverter
    {
        private LatLngUTMConverter() { }
        public class LatLng
        {
            public double Lat { get; set; }
            public double Lng { get; set; }
        }

        public class UTMResult
        {
            public double Easting { get; set; }
            public double UTMEasting { get; set; }
            public double Northing { get; set; }
            public double UTMNorthing { get; set; }
            public int ZoneNumber { get; set; }
            public String ZoneLetter { get; set; }
            public String Zona
            {
                get
                {
                    return ZoneNumber + ZoneLetter;
                }
            }

            public override string ToString()
            {
                return "" + "ZoneNumber: " + ZoneNumber + "\nZoneLetter: " + ZoneLetter + "\nEasting: " + Easting + "\nNorthing: " + Northing;
            }
        }


        private double toRadians(double grad)
        {
            return grad * Math.PI / 180;
        }

        private String getUtmLetterDesignator(double latitude)
        {
            if ((84 >= latitude) && (latitude >= 72))
                return "X";
            else if ((72 > latitude) && (latitude >= 64))
                return "W";
            else if ((64 > latitude) && (latitude >= 56))
                return "V";
            else if ((56 > latitude) && (latitude >= 48))
                return "U";
            else if ((48 > latitude) && (latitude >= 40))
                return "T";
            else if ((40 > latitude) && (latitude >= 32))
                return "S";
            else if ((32 > latitude) && (latitude >= 24))
                return "R";
            else if ((24 > latitude) && (latitude >= 16))
                return "Q";
            else if ((16 > latitude) && (latitude >= 8))
                return "P";
            else if ((8 > latitude) && (latitude >= 0))
                return "N";
            else if ((0 > latitude) && (latitude >= -8))
                return "M";
            else if ((-8 > latitude) && (latitude >= -16))
                return "L";
            else if ((-16 > latitude) && (latitude >= -24))
                return "K";
            else if ((-24 > latitude) && (latitude >= -32))
                return "J";
            else if ((-32 > latitude) && (latitude >= -40))
                return "H";
            else if ((-40 > latitude) && (latitude >= -48))
                return "G";
            else if ((-48 > latitude) && (latitude >= -56))
                return "F";
            else if ((-56 > latitude) && (latitude >= -64))
                return "E";
            else if ((-64 > latitude) && (latitude >= -72))
                return "D";
            else if ((-72 > latitude) && (latitude >= -80))
                return "C";
            else
                return "Z";
        }

        public UTMResult convertLatLngToUtm(double latitude, double longitude)
        {

            int ZoneNumber;

            var LongTemp = longitude;
            var LatRad = toRadians(latitude);
            var LongRad = toRadians(LongTemp);
            //calculate zone number
            if (LongTemp >= 8 && LongTemp <= 13 && latitude > 54.5 && latitude < 58)
            {
                ZoneNumber = 32;
            }
            else if (latitude >= 56.0 && latitude < 64.0 && LongTemp >= 3.0 && LongTemp < 12.0)
            {
                ZoneNumber = 32;
            }
            else
            {
                ZoneNumber = (int)((LongTemp + 180) / 6) + 1;

                if (latitude >= 72.0 && latitude < 84.0)
                {
                    if (LongTemp >= 0.0 && LongTemp < 9.0)
                    {
                        ZoneNumber = 31;
                    }
                    else if (LongTemp >= 9.0 && LongTemp < 21.0)
                    {
                        ZoneNumber = 33;
                    }
                    else if (LongTemp >= 21.0 && LongTemp < 33.0)
                    {
                        ZoneNumber = 35;
                    }
                    else if (LongTemp >= 33.0 && LongTemp < 42.0)
                    {
                        ZoneNumber = 37;
                    }
                }
            }

            var ZoneCM = ZoneNumber * 6 - 183;
            var ZoneCMRad = toRadians(ZoneCM);
            var delta = LongTemp - ZoneCM;
            var Aa = 6378137;
            var Bb = 6356752;
            var Ff = (Aa - Bb) / Aa;
            var rm = Math.Sqrt(Aa * Bb);
            var k0 = 0.9996;
            var e = 0.081819;
            var eSqrt = e * e / (1 - e * e);
            //float x = (float)y / (float)z;
            float n = (float)(Aa - Bb) / (float)(Aa + Bb);
            var rho = Aa * (1 - e * e) / (Math.Sqrt(1 - (e * Math.Sin(LatRad) * e * Math.Sin(LatRad))) * Math.Sqrt(1 - (e * Math.Sin(LatRad) * e * Math.Sin(LatRad))) * Math.Sqrt(1 - (e * Math.Sin(LatRad) * e * Math.Sin(LatRad))));
            var Nu = Aa / Math.Sqrt(1 - e * Math.Sin(LatRad) * e * Math.Sin(LatRad));

            var a0 = Aa * (1 - n + (5 * n * n / 4) * (1 - n) + (1 * (n * n * n * n) / 64) * (1 - n));
            var b0 = (3 * Aa * n / 2) * (1 - n - (7 * n * n / 8) * (1 - n) + 55 * (n * n * n * n) / 64);
            var c0 = (15 * Aa * n * n / 16) * (1 - n + (3 * n * n / 4) * (1 - n));
            var d0 = (35 * Aa * (n * n * n) / 48) * (1 - n + 11 * n * n / 16);
            var e0 = (315 * Aa * (n * n * n * n) / 51) * (1 - n);

            var s = a0 * LatRad - b0 * Math.Sin(2 * LatRad) + c0 * Math.Sin(4 * LatRad) - d0 * Math.Sin(6 * LatRad) + e0 * Math.Sin(8 * LatRad);
            var p = toRadians(delta);
            var k1 = s * k0;
            var k2 = Nu * Math.Sin(LatRad) * Math.Cos(LatRad) * k0 / 2;
            var k3 = ((Nu * Math.Sin(LatRad) * Math.Cos(LatRad) * Math.Cos(LatRad) * Math.Cos(LatRad)) / 24) * (5 - Math.Tan(LatRad) * Math.Tan(LatRad) + 9 * eSqrt * Math.Cos(LatRad) * Math.Cos(LatRad) + 4 * eSqrt * eSqrt * Math.Cos(LatRad) * Math.Cos(LatRad) * Math.Cos(LatRad) * Math.Cos(LatRad)) * k0;
            var k4 = Nu * Math.Cos(LatRad) * k0;
            var k5 = Math.Cos(LatRad) * Math.Cos(LatRad) * Math.Cos(LatRad) * (Nu / 6) * (1 - Math.Tan(LatRad) * Math.Tan(LatRad) + eSqrt * Math.Cos(LatRad) * Math.Cos(LatRad)) * k0;
            var UTMNorthing = (k1 + k2 * p * p + k3 * p * p * p * p);
            var UTMEasting = 500000 + (k4 * p + k5 * p * p * p);

            var UTMZone = getUtmLetterDesignator(latitude);
            return new UTMResult { Easting = UTMEasting, Northing = UTMNorthing, ZoneNumber = ZoneNumber, ZoneLetter = UTMZone };
        }

        private double toDegrees(double rad)
        {
            return rad / Math.PI * 180;
        }

        static void Main(string[] args)
        {
            LatLngUTMConverter val = new LatLngUTMConverter();

            double a = Convert.ToDouble(Console.ReadLine());
            double b = Convert.ToDouble(Console.ReadLine());
            System.Console.WriteLine(val.convertLatLngToUtm(a, b));
            // string c = convertLatLngToUtm(a, b)
            System.Console.ReadKey();
        }
    }
}