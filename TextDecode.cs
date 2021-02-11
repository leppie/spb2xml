using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace spb2xml
{
    static partial class TextDecode
    {
        static readonly byte[][] S = new byte[256][];
        static readonly char[,] K = new char[256, 250];

        static void Transform()
        {
            for (int i = 0; i < K.GetLength(0); i++)
            {
                for (int j = 0; j < K.GetLength(1); j++)
                {
                    K[i, j] = (char)0xff;
                }
            }

            for (int i = 0; i < S.Length; i++)
            {
                var row = S[i];
                if (row != null)
                {
                    for (int j = 0; j < row.Length; j++)
                    {
                        K[row[j], j] = (char)i;
                    }
                }
            }
        }

        public static byte[] Encode(string str)
        {
            var bytes = new byte[str.Length + 1];
            int i = 0;

            for (; i < str.Length; i++)
            {
                var row = S[str[i]];
                bytes[i] = row?[i % 250] ?? 0xff;
            }

            bytes[i] = S[0][i % 250];

            return bytes;
        }

        public static string Decode(byte[] encoded)
        {
            var chars = new char[encoded.Length - 1];

            int i = 0;
            for (; i < chars.Length; i++)
            {
                chars[i] = K[encoded[i], i % 250];
            }

            var last = K[encoded[i], i % 250];

            if (last != 0)
            {
                throw new Exception("Unexpected");
            }

            return new string(chars);
        }

        [Conditional("DEBUG")]
        private static void CheckUnique()
        {
            // vertical
            for (int j = 0; j < 250; j++)
            {
                var bytes = new List<byte>();

                for (int i = 0; i < S.Length; i++)
                {
                    var row = S[i];
                    if (row != null)
                    {
                        bytes.Add(row[j]);
                    }
                }

                var u = bytes.Distinct().ToArray();
                var e = Enumerable.Range(0, 256).Select(x => (byte)x).Except(bytes).ToArray();

                Debug.Assert(u.Length == bytes.Count);
                Debug.Assert(u.Length == 195);
                Debug.Assert(e.Length == 61);
            }

            // horizontal
            for (int i = 0; i < S.Length; i++)
            {
                var row = S[i];
                if (row != null)
                {
                    var bytes = new List<byte>();

                    for (int j = 0; j < 250; j++)
                    {

                        bytes.Add(row[j]);
                    }

                    var u = bytes.Distinct().ToArray();
                    var e = Enumerable.Range(0, 256).Select(x => (byte)x).Except(bytes).ToArray();

                    Debug.Assert(u.Length == bytes.Count);
                    Debug.Assert(u.Length == 250);
                    Debug.Assert(e.Length == 6);
                    Debug.Assert(e.Contains((byte)i));
                }
            }
        }

        static TextDecode()
        {
            InitData();
            Transform();
            CheckUnique();
        }
    }
}
