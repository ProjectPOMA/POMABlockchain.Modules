﻿using System.IO;
using POMABlockchain.Modules.Core;
using POMABlockchain.Modules.KeyPairs;
using POMABlockchain.Modules.NEP6.Helpers;
using Helper = POMABlockchain.Modules.KeyPairs.Helper;

namespace POMABlockchain.Modules.NEP6.Transactions
{
    public class SignedTransaction
    {
        private UInt256 _hash;
        public TransactionAttribute[] Attributes;
        public decimal Gas;

        public Input[] Inputs;
        public Output[] Outputs;

        public Input[] References;
        public byte[] Script;

        public TransactionType Type;
        public byte Version;
        public Witness[] Witnesses;

        public UInt256 Hash
        {
            get
            {
                if (_hash == null)
                {
                    var rawTx = Serialize(false);
                    _hash = new UInt256(Helper.Hash256(rawTx));
                }

                return _hash;
            }
        }

        public byte[] Serialize(bool signed = true)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)Type);
                    writer.Write(Version);

                    // exclusive data
                    switch (Type)
                    {
                        case TransactionType.InvocationTransaction:
                            {
                                writer.WriteVarInt(Script.Length);
                                writer.Write(Script);
                                if (Version >= 1) writer.WriteFixed(Gas);

                                break;
                            }
                        case TransactionType.ClaimTransaction:
                            {
                                writer.WriteVarInt(References.Length);
                                foreach (var entry in References)
                                {
                                    SerializationHelper.SerializeTransactionInput(writer, entry);
                                }

                                break;
                            }
                    }
                    // Don't need any attributes
                    if (Attributes != null)
                    {
                        writer.WriteVarInt(Attributes.Length);
                        foreach (var attr in Attributes)
                        {
                            attr.Serialize(writer);
                        }
                    }
                    else
                    {
                        writer.Write((byte)0);
                    }

                    writer.WriteVarInt(Inputs.Length);
                    foreach (var input in Inputs) SerializationHelper.SerializeTransactionInput(writer, input);

                    writer.WriteVarInt(Outputs.Length);
                    foreach (var output in Outputs) SerializationHelper.SerializeTransactionOutput(writer, output);

                    if (signed && Witnesses != null)
                    {
                        writer.WriteVarInt(Witnesses.Length);
                        foreach (var witness in Witnesses) witness.Serialize(writer);
                    }
                }
                return stream.ToArray();
            }
        }

        public void Sign(KeyPair key)
        {
            var txdata = Serialize(false);

            var privkey = key.PrivateKey;
            var signature = Utils.Sign(txdata, privkey);

            var invocationScript = ("40" + signature.ToHexString()).HexToBytes();
            var verificationScript = Helper.CreateSignatureRedeemScript(key.PublicKey);
            Witnesses = new[]
            {
                new Witness
                {
                    InvocationScript = invocationScript,
                    VerificationScript = verificationScript
                }
            };
        }

        public void Sign(byte[] privateKey)
        {
            var txdata = Serialize(false);

            var signature = Utils.Sign(txdata, privateKey);

            var invocationScript = ("40" + signature.ToHexString()).HexToBytes();
            var verificationScript = Helper.CreateSignatureRedeemScript(new KeyPair(privateKey).PublicKey);
            Witnesses = new[]
            {
                new Witness
                {
                    InvocationScript = invocationScript,
                    VerificationScript = verificationScript
                }
            };
        }

        public struct Input
        {
            public byte[] PrevHash;
            public uint PrevIndex;
        }

        public struct Output
        {
            public byte[] ScriptHash;
            public byte[] AssetId;
            public decimal Value;
        }
    }
}