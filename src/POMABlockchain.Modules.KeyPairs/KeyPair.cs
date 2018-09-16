﻿using System;
using System.IO;
using POMABlockchain.Modules.Core;
using POMABlockchain.Modules.KeyPairs.Cryptography.ECC;

namespace POMABlockchain.Modules.KeyPairs
{
    /// <summary>
    /// KeyPair class from https://github.com/POMABlockchain-project/POMABlockchain/blob/master/POMABlockchain/Wallets/KeyPair.cs
    /// </summary>
    public class KeyPair : IEquatable<KeyPair>
    {
        public readonly byte[] PrivateKey;
        public readonly ECPoint PublicKey;

        public KeyPair(byte[] privateKey)
        {
            if (privateKey.Length != 32 && privateKey.Length != 96 && privateKey.Length != 104)
                throw new ArgumentException();
            PrivateKey = new byte[32];
            Buffer.BlockCopy(privateKey, privateKey.Length - 32, PrivateKey, 0, 32);
            if (privateKey.Length == 32)
                PublicKey = ECCurve.Secp256r1.G * privateKey;
            else
                PublicKey = ECPoint.FromBytes(privateKey, ECCurve.Secp256r1);
#if NET47
            ProtectedMemory.Protect(PrivateKey, MemoryProtectionScope.SameProcess);
#endif
        }

        public UInt160 PublicKeyHash => PublicKey.EncodePoint(true).ToScriptHash();

        public bool Equals(KeyPair other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (ReferenceEquals(null, other)) return false;
            return PublicKey.Equals(other.PublicKey);
        }

        public IDisposable Decrypt()
        {
#if NET47
            return new ProtectedMemoryContext(PrivateKey, MemoryProtectionScope.SameProcess);
#else
            return new MemoryStream(0);
#endif
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as KeyPair);
        }

        public string Export()
        {
            using (Decrypt())
            {
                var data = new byte[34];
                data[0] = 0x80;
                Buffer.BlockCopy(PrivateKey, 0, data, 1, 32);
                data[33] = 0x01;
                var wif = data.Base58CheckEncode();
                Array.Clear(data, 0, data.Length);
                return wif;
            }
        }

        public override int GetHashCode()
        {
            return PublicKey.GetHashCode();
        }

        public override string ToString()
        {
            return PublicKey.ToString();
        }
    }
}