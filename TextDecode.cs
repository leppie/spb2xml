using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace spb2xml
{
    static partial class TextDecode
    {
        static readonly byte[][] S = new byte[0x7F][];
        static readonly char[,] K = new char[256,250];

        static void Transform()
        {
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

        public static string Decode(byte[] encoded)
        {
            var chars = new char[encoded.Length - 1];
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = K[encoded[i], i % 250];
            }

            return new string(chars);
        }

        static TextDecode() 
        {
            InitData();
            Transform();
        }
    }
}
