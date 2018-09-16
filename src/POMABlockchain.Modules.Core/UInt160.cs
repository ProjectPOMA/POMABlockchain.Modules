﻿using System;
using System.Globalization;
using System.Linq;

namespace POMABlockchain.Modules.Core
{
    /// Taken from POMABlockchain-project https://github.com/POMABlockchain-project/POMABlockchain/blob/master/POMABlockchain/UInt160.cs
    /// <summary>
    /// ECC elliptic curve parameters
    /// </summary>
    public class UInt160 : UIntBase, IComparable<UInt160>, IEquatable<UInt160>
    {
        public static readonly UInt160 Zero = new UInt160();

        public UInt160()
            : this(null)
        {
        }

        public UInt160(byte[] value)
            : base(20, value)
        {
        }

        public int CompareTo(UInt160 other)
        {
            var x = ToArray();
            var y = other.ToArray();
            for (var i = x.Length - 1; i >= 0; i--)
            {
                if (x[i] > y[i])
                    return 1;
                if (x[i] < y[i])
                    return -1;
            }
            return 0;
        }

        bool IEquatable<UInt160>.Equals(UInt160 other)
        {
            return Equals(other);
        }

        public new static UInt160 Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException();
            if (value.StartsWith("0x"))
                value = value.Substring(2);
            if (value.Length != 40)
                throw new FormatException();
            return new UInt160(value.HexToBytes().Reverse().ToArray());
        }

        public static bool TryParse(string s, out UInt160 result)
        {
            if (s == null)
            {
                result = null;
                return false;
            }
            if (s.StartsWith("0x"))
                s = s.Substring(2);
            if (s.Length != 40)
            {
                result = null;
                return false;
            }
            var data = new byte[20];
            for (var i = 0; i < 20; i++)
                if (!byte.TryParse(s.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier, null, out data[i]))
                {
                    result = null;
                    return false;
                }
            result = new UInt160(data.Reverse().ToArray());
            return true;
        }

        public static bool operator >(UInt160 left, UInt160 right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(UInt160 left, UInt160 right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(UInt160 left, UInt160 right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(UInt160 left, UInt160 right)
        {
            return left.CompareTo(right) <= 0;
        }
    }
}