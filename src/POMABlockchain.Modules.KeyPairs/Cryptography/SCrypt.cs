﻿using System;
using System.Security.Cryptography;

namespace POMABlockchain.Modules.KeyPairs.Cryptography
{
    /// <summary>
    /// SCrypt class from https://github.com/POMABlockchain-project/POMABlockchain/blob/master/POMABlockchain/Cryptography/SCrypt.cs
    /// </summary>
    public static class SCrypt
    {
        private static unsafe void BulkCopy(void* dst, void* src, int len)
        {
            var d = (byte*) dst;
            var s = (byte*) src;

            while (len >= 8)
            {
                *(ulong*) d = *(ulong*) s;
                d += 8;
                s += 8;
                len -= 8;
            }
            if (len >= 4)
            {
                *(uint*) d = *(uint*) s;
                d += 4;

                s += 4;
                len -= 4;
            }
            if (len >= 2)
            {
                *(ushort*) d = *(ushort*) s;
                d += 2;
                s += 2;
                len -= 2;
            }
            if (len >= 1)
                *d = *s;
        }

        private static unsafe void BulkXor(void* dst, void* src, int len)
        {
            var d = (byte*) dst;
            var s = (byte*) src;

            while (len >= 8)
            {
                *(ulong*) d ^= *(ulong*) s;
                d += 8;
                s += 8;
                len -= 8;
            }
            if (len >= 4)
            {
                *(uint*) d ^= *(uint*) s;
                d += 4;
                s += 4;
                len -= 4;
            }
            if (len >= 2)
            {
                *(ushort*) d ^= *(ushort*) s;
                d += 2;
                s += 2;
                len -= 2;
            }
            if (len >= 1)
                *d ^= *s;
        }

        private static unsafe void Encode32(byte* p, uint x)
        {
            p[0] = (byte) (x & 0xff);
            p[1] = (byte) ((x >> 8) & 0xff);
            p[2] = (byte) ((x >> 16) & 0xff);
            p[3] = (byte) ((x >> 24) & 0xff);
        }

        private static unsafe uint Decode32(byte* p)
        {
            return
                p[0] +
                ((uint) p[1] << 8) +
                ((uint) p[2] << 16) +
                ((uint) p[3] << 24);
        }

        private static unsafe void Salsa208(uint* b)
        {
            var x0 = b[0];
            var x1 = b[1];
            var x2 = b[2];
            var x3 = b[3];
            var x4 = b[4];
            var x5 = b[5];
            var x6 = b[6];
            var x7 = b[7];
            var x8 = b[8];
            var x9 = b[9];
            var x10 = b[10];
            var x11 = b[11];
            var x12 = b[12];
            var x13 = b[13];
            var x14 = b[14];
            var x15 = b[15];

            for (var i = 0; i < 8; i += 2)
            {
                //((x0 + x12) << 7) | ((x0 + x12) >> (32 - 7));
                /* Operate on columns. */
                x4 ^= R(x0 + x12, 7);
                x8 ^= R(x4 + x0, 9);
                x12 ^= R(x8 + x4, 13);
                x0 ^= R(x12 + x8, 18);

                x9 ^= R(x5 + x1, 7);
                x13 ^= R(x9 + x5, 9);
                x1 ^= R(x13 + x9, 13);
                x5 ^= R(x1 + x13, 18);

                x14 ^= R(x10 + x6, 7);
                x2 ^= R(x14 + x10, 9);
                x6 ^= R(x2 + x14, 13);
                x10 ^= R(x6 + x2, 18);

                x3 ^= R(x15 + x11, 7);
                x7 ^= R(x3 + x15, 9);
                x11 ^= R(x7 + x3, 13);
                x15 ^= R(x11 + x7, 18);

                /* Operate on rows. */
                x1 ^= R(x0 + x3, 7);
                x2 ^= R(x1 + x0, 9);
                x3 ^= R(x2 + x1, 13);
                x0 ^= R(x3 + x2, 18);

                x6 ^= R(x5 + x4, 7);
                x7 ^= R(x6 + x5, 9);
                x4 ^= R(x7 + x6, 13);
                x5 ^= R(x4 + x7, 18);

                x11 ^= R(x10 + x9, 7);
                x8 ^= R(x11 + x10, 9);
                x9 ^= R(x8 + x11, 13);
                x10 ^= R(x9 + x8, 18);

                x12 ^= R(x15 + x14, 7);
                x13 ^= R(x12 + x15, 9);
                x14 ^= R(x13 + x12, 13);
                x15 ^= R(x14 + x13, 18);
            }

            b[0] += x0;
            b[1] += x1;
            b[2] += x2;
            b[3] += x3;
            b[4] += x4;
            b[5] += x5;
            b[6] += x6;
            b[7] += x7;
            b[8] += x8;
            b[9] += x9;
            b[10] += x10;
            b[11] += x11;
            b[12] += x12;
            b[13] += x13;
            b[14] += x14;
            b[15] += x15;
        }

        private static uint R(uint a, int b)
        {
            return (a << b) | (a >> (32 - b));
        }

        private static unsafe void BlockMix(uint* bin, uint* bout, uint* x, int r)
        {
            /* 1: X <-- B_{2r - 1} */
            BulkCopy(x, &bin[(2 * r - 1) * 16], 64);

            /* 2: for i = 0 to 2r - 1 do */
            for (var i = 0; i < 2 * r; i += 2)
            {
                /* 3: X <-- H(X \xor B_i) */
                BulkXor(x, &bin[i * 16], 64);
                Salsa208(x);

                /* 4: Y_i <-- X */
                /* 6: B' <-- (Y_0, Y_2 ... Y_{2r-2}, Y_1, Y_3 ... Y_{2r-1}) */
                BulkCopy(&bout[i * 8], x, 64);

                /* 3: X <-- H(X \xor B_i) */
                BulkXor(x, &bin[i * 16 + 16], 64);
                Salsa208(x);

                /* 4: Y_i <-- X */
                /* 6: B' <-- (Y_0, Y_2 ... Y_{2r-2}, Y_1, Y_3 ... Y_{2r-1}) */
                BulkCopy(&bout[i * 8 + r * 16], x, 64);
            }
        }

        private static unsafe long Integerify(uint* b, int r)
        {
            var x = (uint*) ((byte*) b + (2 * r - 1) * 64);

            return ((long) x[1] << 32) + x[0];
        }

        private static unsafe void SMix(byte* b, int r, int n, uint* v, uint* xy)
        {
            var x = xy;
            var y = &xy[32 * r];
            var z = &xy[64 * r];

            /* 1: X <-- B */
            for (var k = 0; k < 32 * r; k++)
                x[k] = Decode32(&b[4 * k]);

            /* 2: for i = 0 to N - 1 do */
            for (var i = 0L; i < n; i += 2)
            {
                /* 3: V_i <-- X */
                BulkCopy(&v[i * (32 * r)], x, 128 * r);

                /* 4: X <-- H(X) */
                BlockMix(x, y, z, r);

                /* 3: V_i <-- X */
                BulkCopy(&v[(i + 1) * (32 * r)], y, 128 * r);

                /* 4: X <-- H(X) */
                BlockMix(y, x, z, r);
            }

            /* 6: for i = 0 to N - 1 do */
            for (var i = 0; i < n; i += 2)
            {
                /* 7: j <-- Integerify(X) mod N */
                var j = Integerify(x, r) & (n - 1);

                /* 8: X <-- H(X \xor V_j) */
                BulkXor(x, &v[j * (32 * r)], 128 * r);
                BlockMix(x, y, z, r);

                /* 7: j <-- Integerify(X) mod N */
                j = Integerify(y, r) & (n - 1);

                /* 8: X <-- H(X \xor V_j) */
                BulkXor(y, &v[j * (32 * r)], 128 * r);
                BlockMix(y, x, z, r);
            }

            /* 10: B' <-- X */
            for (var k = 0; k < 32 * r; k++)
                Encode32(&b[4 * k], x[k]);
        }

#if NET47
        public static byte[] DeriveKey(byte[] password, byte[] salt, int N, int r, int p, int derivedKeyLength)
        {
            return Replicon.Cryptography.SCrypt.SCrypt.DeriveKey(password, salt, (ulong)N, (uint)r, (uint)p, (uint)derivedKeyLength);
        }
#else
        public static unsafe byte[] DeriveKey(byte[] password, byte[] salt, int n, int r, int p, int derivedKeyLength)
        {
            var ba = new byte[128 * r * p + 63];
            var xYa = new byte[256 * r + 63];
            var va = new byte[128 * r * n + 63];
            var buf = new byte[derivedKeyLength];

            var mac = new HMACSHA256(password);

            /* 1: (B_0 ... B_{p-1}) <-- PBKDF2(P, S, 1, p * MFLen) */
            PBKDF2_SHA256(mac, password, salt, salt.Length, 1, ba, p * 128 * r);

            fixed (byte* b = ba)
            fixed (void* v = va)
            fixed (void* xy = xYa)
            {
                /* 2: for i = 0 to p - 1 do */
                for (var i = 0; i < p; i++)
                    /* 3: B_i <-- MF(B_i, N) */
                    SMix(&b[i * 128 * r], r, n, (uint*) v, (uint*) xy);
            }

            /* 5: DK <-- PBKDF2(P, B, 1, dkLen) */
            PBKDF2_SHA256(mac, password, ba, p * 128 * r, 1, buf, buf.Length);

            return buf;
        }
#endif

        private static void PBKDF2_SHA256(HMACSHA256 mac, byte[] password, byte[] salt, int saltLength,
            long iterationCount, byte[] derivedKey, int derivedKeyLength)
        {
            if (derivedKeyLength > (Math.Pow(2, 32) - 1) * 32)
                throw new ArgumentException("Requested key length too long");

            var u = new byte[32];
            var T = new byte[32];
            var saltBuffer = new byte[saltLength + 4];

            var blockCount = (int) Math.Ceiling((double) derivedKeyLength / 32);
            var r = derivedKeyLength - (blockCount - 1) * 32;

            Buffer.BlockCopy(salt, 0, saltBuffer, 0, saltLength);

            using (var incrementalHasher = IncrementalHash.CreateHMAC(HashAlgorithmName.SHA256, mac.Key))
            {
                for (var i = 1; i <= blockCount; i++)
                {
                    saltBuffer[saltLength + 0] = (byte) (i >> 24);
                    saltBuffer[saltLength + 1] = (byte) (i >> 16);
                    saltBuffer[saltLength + 2] = (byte) (i >> 8);
                    saltBuffer[saltLength + 3] = (byte) i;

                    mac.Initialize();
                    incrementalHasher.AppendData(saltBuffer, 0, saltBuffer.Length);
                    Buffer.BlockCopy(incrementalHasher.GetHashAndReset(), 0, u, 0, u.Length);
                    Buffer.BlockCopy(u, 0, T, 0, 32);

                    for (long j = 1; j < iterationCount; j++)
                    {
                        incrementalHasher.AppendData(u, 0, u.Length);
                        Buffer.BlockCopy(incrementalHasher.GetHashAndReset(), 0, u, 0, u.Length);
                        for (var k = 0; k < 32; k++)
                            T[k] ^= u[k];
                    }

                    Buffer.BlockCopy(T, 0, derivedKey, (i - 1) * 32, i == blockCount ? r : 32);
                }
            }
        }
    }
}