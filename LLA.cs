using System;
using System.Collections.Generic;
using System.Text;

namespace spb2xml
{
    class LLA
    {
        private double mLat;

        public double Lat
        {
            get { return mLat; }
            set { mLat = value; }
        }

        private double mLon;

        public double Lon
        {
            get { return mLon; }
            set { mLon = value; }
        }

        private double mAlt;

        public double Alt
        {
            get { return mAlt; }
            set { mAlt = value; }
        }

        public LLA(double lat, double lon, double alt)
        {
            Lat = lat;
            Lon = lon;
            Alt = alt;
        }

        public LLA(double lat, double lon) : this(lat, lon, 0)
        {
        }

        public LLA(long lat, long lon, uint alt1, int alt2)
        {
            Lat = lat * 90.0 / (10001750.0 * 65536.0 * 65536.0);
            Lon = lon * 360.0 / (65536.0 * 65536.0 * 65536.0 * 65536.0);
            double alt0 = alt2 + (double)alt1 / (65536.0 * 65536.0);
            // convert to feet
            Alt = alt0 * 3.2808399;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {


            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            LLA objLLa = obj as LLA;
            if (objLLa != null)
            {
                return objLLa.Lat == Lat && objLLa.Lon == Lon && objLLa.Alt == Alt;
            }
            return false;

        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return ((int) Lon) ^ ((int) Lat) ^ ((int) Alt);
        }

        public string ToString(string format)
        {
            if ("N".Equals(format) || "N1".Equals(format))
            {
                return Lat + "," + Lon;
            }
            if ("D2".Equals(format))
            {
                StringBuilder sb = new StringBuilder(40);
                if (Lat < 0)
                {
                    sb.Append('S');
                }
                else
                {
                    sb.Append('N');
                }
                int m1 = (int) Math.Floor(Math.Abs(Lat));
                sb.Append(m1);
                sb.Append("° ");
                double min = (Math.Abs(Lat) - m1) * 60.0;
                int m2 = (int)Math.Floor(min);
                sb.Append(m2);
                sb.Append("' ");
                double sec = (min - m2) * 60;
                sb.Append(sec.ToString("#0.00"));
                sb.Append("\",");

                if (Lon < 0)
                {
                    sb.Append("W");
                }
                else
                {
                    sb.Append("E");
                }
                m1 = (int)Math.Floor(Math.Abs(Lon));
                sb.Append(m1);
                sb.Append("° ");
                min = (Math.Abs(Lon) - m1) * 60.0;
                m2 = (int)Math.Floor(min);
                sb.Append(m2);
                sb.Append("' ");
                sec = (min - m2) * 60;
                sb.Append(sec.ToString("#0.00"));
                sb.Append("\",");

                if (Alt < 0)
                {
                    sb.Append("-");
                }
                else
                {
                    sb.Append("+");
                }
                sb.Append(Math.Abs(Alt).ToString("000000.00"));

                return sb.ToString();
            }
            if ("D1".Equals(format))
            {
                StringBuilder sb = new StringBuilder(40);
                if (Lat < 0)
                {
                    sb.Append('S');
                }
                else
                {
                    sb.Append('N');
                }
                int m1 = (int)Math.Floor(Math.Abs(Lat));
                sb.Append(m1);
                sb.Append("° ");
                double min = (Math.Abs(Lat) - m1) * 60.0;
                sb.Append(min.ToString("#0.00#####"));
                sb.Append(",");

                if (Lon < 0)
                {
                    sb.Append("W");
                }
                else
                {
                    sb.Append("E");
                }
                m1 = (int)Math.Floor(Math.Abs(Lon));
                sb.Append(m1);
                sb.Append("° ");
                min = (Math.Abs(Lon) - m1) * 60.0;
                sb.Append(min.ToString("#0.00#####"));
                sb.Append(",");

                sb.Append(Math.Abs(Alt).ToString("####0.00"));

                return sb.ToString();
            }
            return Lat + "," + Lon + "," + Alt;
        }
    }
}
