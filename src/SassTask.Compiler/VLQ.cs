using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sass
{
    public static class VLQ
    {
        static IDictionary<char, int> charToInteger;
        static IDictionary<int, char> integerToChar;


        static VLQ()
        {
            charToInteger = new Dictionary<char, int>();
            integerToChar = new Dictionary<int, char>();

            int i = 0;
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/="
            .ToList()
            .ForEach((ch) =>
            {
                charToInteger[ch] = i;
                integerToChar[i] = ch;
                i++;
            });
        }

        public static int[] Decode(string str)
        {
            List<int> result = new List<int>();
            int shift = 0;
            int value = 0;

            for (int i = 0; i < str.Length; i += 1)
            {
                if (!charToInteger.TryGetValue(str[i], out var integer))
                {
                    throw new Exception($"Invalid character ({str[i]})");
                }

                bool hasContinuationBit = Convert.ToBoolean(integer & 32);

                integer &= 31;
                value += integer << shift;

                if (hasContinuationBit)
                {
                    shift += 5;
                }
                else
                {
                    bool shouldNegate = Convert.ToBoolean(value & 1);
                    value >>= 1;

                    if (shouldNegate)
                    {
                        result.Add((value == 0 ? -0x80000000 : -value));
                    }
                    else
                    {
                        result.Add(value);
                    }

                    // reset
                    value = shift = 0;
                }
            }

            return result.ToArray();
        }

        public static string Encode(int value)
        {
            return encodeInteger(value);
        }

        public static string Encode(int[] value)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < value.Length; i += 1)
            {
                result.Append(encodeInteger(value[i]));
            }

            return result.ToString();
        }

        private static string encodeInteger(int num)
        {
            StringBuilder result = new StringBuilder();

            if (num < 0)
            {
                num = (int)(-num << 1) | 1;
            }
            else
            {
                num <<= 1;
            }

            do
            {
                int clamped = num & 31;
                num = num >> 5;

                if (num > 0)
                {
                    clamped |= 32;
                }

                result.Append(integerToChar[(int)clamped]);
            } while (num > 0);

            return result.ToString();
        }
    }
}
