﻿using Gibbed.Illusion.FileFormats.Hashing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Utils.VorticeUtils;

namespace ResourceTypes.Animation2
{
    public class AnimTrack
    {
        public ulong BoneID { get; set; }
        public byte Flags { get; set; }
        public bool IsDataPresent { get; set; }
        public byte DataFlags { get; set; }
        public short NumKeyFrames { get; set; }
        public byte ComponentSize { get; set; }
        public byte TimeSize { get; set; }
        public uint PackedReferenceQuat { get; set; }
        public float Scale { get; set; }
        public float Duration { get; set; }
        public byte[] KeyFrameData { get; set; }
        public Quaternion[] KeyFrames { get; set; }
        public float Unk00 { get; set; }
        public UnkDataBlock[] UnkData { get; set; } = new UnkDataBlock[0];
        public AnimTrack()
        {

        }

        public AnimTrack(Stream s)
        {
            Read(s);
        }

        public AnimTrack(BinaryReader br)
        {
            Read(br);
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }

        public void Read(BinaryReader br)
        {
            BoneID = br.ReadUInt64();
            Flags = br.ReadByte();
            IsDataPresent = br.ReadBoolean();
            DataFlags = br.ReadByte();
            NumKeyFrames = br.ReadInt16();
            ComponentSize = br.ReadByte();
            TimeSize = br.ReadByte();
            PackedReferenceQuat = br.ReadUInt32();
            Scale = br.ReadSingle();
            Duration = br.ReadSingle();
            KeyFrameData = br.ReadBytes(GetKeyframeDataSize(NumKeyFrames, ComponentSize, TimeSize));

            if ((Flags & 0x01) == 1)
            {
                short Count = br.ReadInt16();
                Unk00 = br.ReadSingle();
                UnkData = new UnkDataBlock[Count];

                for (int i = 0; i < UnkData.Length; i++)
                {
                    UnkData[i] = new(br);
                }
            }

            Dequantize();

            //DumpTrackData();
        }

        public int GetKeyframeSize(int ComponentSize, int TimeSize)
        {
            // Perform the calculation and apply the bitwise AND operation
            return (3 * ComponentSize + TimeSize + 2) & 0x7F;
        }

        public int GetKeyframeDataSize(int NumRotationFrames, int ComponentSize, int TimeSize)
        {
            // Perform the calculation and apply the bitwise AND operation
            return 4 + 4 * (NumRotationFrames * GetKeyframeSize(ComponentSize, TimeSize) / 32);
        }

        private void Dequantize() //Code by RoadTrain
        {
            var data = new BigInteger(KeyFrameData);
            var quats = new List<Quaternion>();
            var chunkSize = 3 * ComponentSize + TimeSize + 2;

            for (var i = 0; i < NumKeyFrames; i++)
            {
                var dataCurrent = data >> (i * chunkSize);

                var time = (dataCurrent) & ((1 << TimeSize) - 1);
                var component1 = (dataCurrent >> (TimeSize)) & ((1 << ComponentSize) - 1);
                var component2 = (dataCurrent >> (TimeSize + ComponentSize)) & ((1 << ComponentSize) - 1);
                var component3 = (dataCurrent >> (TimeSize + ComponentSize * 2)) & ((1 << ComponentSize) - 1);
                var omittedComponent = (dataCurrent >> (TimeSize + ComponentSize * 2 + 2)) & ((1 << 2) - 1);

                float x;
                float y;
                float z;
                float w;
                switch ((int)omittedComponent)
                {
                    case 0: // x omitted
                        y = Normalize((int)component1, ComponentSize);
                        z = Normalize((int)component2, ComponentSize);
                        w = Normalize((int)component3, ComponentSize);
                        x = (float)Math.Sqrt(1 - y * y - z * z - w * w);
                        break;
                    case 1: // y omitted
                        x = Normalize((int)component1, ComponentSize);
                        z = Normalize((int)component2, ComponentSize);
                        w = Normalize((int)component3, ComponentSize);
                        y = (float)Math.Sqrt(1 - x * x - z * z - w * w);
                        break;
                    case 2: // z omitted
                        x = Normalize((int)component1, ComponentSize);
                        y = Normalize((int)component2, ComponentSize);
                        w = Normalize((int)component3, ComponentSize);
                        z = (float)Math.Sqrt(1 - x * x - y * y - w * w);
                        break;
                    case 3: // w omitted
                        x = Normalize((int)component1, ComponentSize);
                        y = Normalize((int)component2, ComponentSize);
                        z = Normalize((int)component3, ComponentSize);
                        w = (float)Math.Sqrt(1 - x * x - y * y - z * z);
                        break;
                    default:
                        throw new Exception();
                }

                quats.Add(new Quaternion(x, y, z, w));
            }

            KeyFrames = quats.ToArray();
        }

        private static float Normalize(int value, int size)
        {
            var maxValue = (float)((1 << size) - 1);
            return (value - maxValue / 2) / (maxValue);
        }

        private void DumpTrackData()
        {
            string folderPath = "%userprofile%\\Desktop\\AnimTracks";
            string path = Environment.ExpandEnvironmentVariables(folderPath);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_{FNV32.Hash(KeyFrameData, 0, KeyFrameData.Length)}.bin"), KeyFrameData);

            using (MemoryStream ms = new())
            {
                foreach (var val in KeyFrames)
                {
                    val.WriteToFile(ms, false);
                }

                var data = ms.ToArray();

                File.WriteAllBytes(Path.Combine(path, $"AnimTrack_{BoneID}_{FNV32.Hash(data, 0, data.Length)}_Decompressed.bin"), data);
            }
        }
    }

    public class UnkDataBlock
    {
        public short BoneIndex { get; set; }
        public byte[] Data { get; set; } = new byte[10];

        public UnkDataBlock()
        {

        }

        public UnkDataBlock(Stream s)
        {
            Read(s);
        }

        public UnkDataBlock(BinaryReader br)
        {
            Read(br);
        }

        public void Read(Stream s)
        {
            using (BinaryReader br = new(s))
            {
                Read(br);
            }
        }
        
        public void Read(BinaryReader br)
        {
            BoneIndex = br.ReadInt16();
            Data = br.ReadBytes(10);
        }
    }
}
