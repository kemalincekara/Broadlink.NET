using System;
using System.Linq;
using System.Text;

namespace Broadlink.NET
{
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Convert an <see cref="int"/> value to a <see cref="byte[]"/> with little endian ordering
        /// </summary>
        /// <param name="input"><see cref="int"/> value</param>
        /// <returns><see cref="byte[]"/> with little endian ordering</returns>
        public static byte[] ToLittleEndianBytes(this int input)
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new NotImplementedException("Computer is big endian and conversion is not implemented yet");
            }
            return BitConverter.GetBytes(input);
        }

        /// <summary>
        /// Convert a <see cref="short"/> value to a <see cref="byte[]"/> with little endian ordering
        /// </summary>
        /// <param name="input"><see cref="short"/> value</param>
        /// <returns><see cref="byte[]"/> with little endian ordering</returns>
        public static byte[] ToLittleEndianBytes(this short input)
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new NotImplementedException("Computer is big endian and conversion is not implemented yet");
            }
            return BitConverter.GetBytes(input);
        }

        ///// <summary>
        ///// Convenience function to get the slice of a <see cref="byte[]"/> from <paramref name="input"/>
        ///// </summary>
        ///// <param name="input">byte array to slice</param>
        ///// <param name="startIndex">start index to slice from</param>
        ///// <returns>slice of <paramref name="input"/> starting from <paramref name="startIndex"/></returns>
        //public static byte[] Slice(this byte[] input, int startIndex)
        //{
        //    var restArray = new byte[input.Length - startIndex];
        //    Array.Copy(input, startIndex, restArray, 0, restArray.Length);
        //    return restArray;
        //}

        /// <summary>
        /// Convenience function to get the slice of a <see cref="byte[]"/>
        /// </summary>
        /// <param name="input">byte array to slice</param>
        /// <param name="startIndex">start index to slice from</param>
        /// <param name="endIndex">endIndex to slice to. Use -1 to slice until the end</param>
        /// <returns>slice of <paramref name="input"/></returns>
        public static byte[] Slice(this byte[] input, int startIndex, int endIndex = -1)
        {
            if (endIndex <= 0)
            {
                endIndex = input.Length - 1;
            }

            if (endIndex <= startIndex || endIndex >= input.Length)
            {
                throw new ArgumentException();
            }

            var restArray = new byte[endIndex - startIndex + 1];

            Array.Copy(input, startIndex, restArray, 0, restArray.Length);
            return restArray;
        }

        public static bool BytesContains(this byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                    return true;
            return false;
        }
        public static byte[] HexToBytes(this string hex) => Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        public static string ByteToHex(this byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }
            return new string(c);
        }
        public static string ToBase64(this byte[] data) => Convert.ToBase64String(data, 0, data.Length);
        public static byte[] FromBase64Bytes(this string data) => Convert.FromBase64String(data);
        public static string ToBase64(this string data) => data.GetBytesUTF8().ToBase64();
        public static string FromBase64String(this string data) => data.FromBase64Bytes().GetStringUTF8();
        public static byte[] GetBytesUTF8(this string data) => Encoding.UTF8.GetBytes(data);
        public static string GetStringUTF8(this byte[] data) => Encoding.UTF8.GetString(data);
    }
}
