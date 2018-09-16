﻿using System;
using System.Linq;
using System.Numerics;
using System.Text;

namespace POMABlockchain.Modules.KeyPairs
{
	/// <summary>
	/// Base58 class from https://github.com/POMABlockchain-project/POMABlockchain/blob/master/POMABlockchain/Cryptography/Base58.cs
	/// </summary>
	public static class Base58
	{
		/// <summary>
		/// Base58 encoded alphabet
		/// </summary>
		public const string Alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

		/// <summary>
		/// Decoding
		/// </summary>
		/// <param name="input">The string to be decoded</param>
		/// <returns>Return decoded byte array</returns>
		public static byte[] Decode(string input)
		{
			BigInteger bi = BigInteger.Zero;
			for (int i = input.Length - 1; i >= 0; i--)
			{
				int index = Alphabet.IndexOf(input[i]);
				if (index == -1)
					throw new FormatException();
				bi += index * BigInteger.Pow(58, input.Length - 1 - i);
			}
			byte[] bytes = bi.ToByteArray();
			Array.Reverse(bytes);
			bool stripSignByte = bytes.Length > 1 && bytes[0] == 0 && bytes[1] >= 0x80;
			int leadingZeros = 0;
			for (int i = 0; i < input.Length && input[i] == Alphabet[0]; i++)
			{
				leadingZeros++;
			}
			byte[] tmp = new byte[bytes.Length - (stripSignByte ? 1 : 0) + leadingZeros];
			Array.Copy(bytes, stripSignByte ? 1 : 0, tmp, leadingZeros, tmp.Length - leadingZeros);
			return tmp;
		}

		/// <summary>
		/// Encoding
		/// </summary>
		/// <param name="input">The byte array to encode</param>
		/// <returns>Return encoded string</returns>
		public static string Encode(byte[] input)
		{
			BigInteger value = new BigInteger(new byte[1].Concat(input).Reverse().ToArray());
			StringBuilder sb = new StringBuilder();
			while (value >= 58)
			{
				BigInteger mod = value % 58;
				sb.Insert(0, Alphabet[(int)mod]);
				value /= 58;
			}
			sb.Insert(0, Alphabet[(int)value]);
			foreach (byte b in input)
			{
				if (b == 0)
					sb.Insert(0, Alphabet[0]);
				else
					break;
			}
			return sb.ToString();
		}
	}
}
