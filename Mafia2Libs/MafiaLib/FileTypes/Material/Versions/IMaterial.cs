﻿using System.Collections.Generic;
using System.IO;
using Utils.Types;
using System.ComponentModel;
using Utils.Extensions;

namespace ResourceTypes.Materials
{
    public class IMaterial
    {
        public Hash MaterialName { get; set; }

        [Editor(typeof(FlagEnumUIEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public MaterialFlags Flags { get; set; }
        public ulong ShaderID { get; set; }
        public uint ShaderHash { get; set; }
        public List<IMaterialSampler> Samplers { get; set; }
        public List<MaterialParameter> Parameters { get; set; }

        public IMaterial()
        {
            MaterialName = new Hash();
            Samplers = new List<IMaterialSampler>();
            Parameters = new List<MaterialParameter>();

            // TODO: Remove this from base class, make some kind of factory of Shader Types.
            MaterialName.Set("NEW_MATERIAL");
            ShaderHash = 3388704532;
            ShaderID = 4894707398632176459;
            Flags = (MaterialFlags)31461376;
        }

        public virtual void ReadFromFile(BinaryReader reader, VersionsEnumerator version) { }

        public virtual void WriteToFile(BinaryWriter writer, VersionsEnumerator version) { }

        public void SetName(string name)
        {
            MaterialName.Set(name);
        }

        public string GetMaterialName()
        {
            return MaterialName.String;
        }

        public ulong GetMaterialHash()
        {
            return MaterialName.uHash;
        }
        public virtual Hash GetTextureByID(string SamplerName)
        {
            foreach (var sampler in Samplers)
            {
                if (sampler.ID == SamplerName)
                {
                    Hash TextureFile = new Hash();
                    TextureFile.String = sampler.GetFileName();
                    TextureFile.uHash = sampler.GetFileHash();
                    return TextureFile;
                }
            }

            return null;
        }

        public MaterialParameter GetParameterByKey(string ParameterKey)
        {
            foreach (var param in Parameters)
            {
                if (param.ID == ParameterKey)
                {
                    return param;
                }
            }

            return null;
        }
    }

    public class IMaterialSampler
    {
        public string ID { get; set; }
        public byte[] SamplerStates { get; set; }

        public IMaterialSampler()
        {
            // TODO: Remove this from base class, make some kind of factory of Shader Types.
            ID = "S000";
            SamplerStates = new byte[6] { 3, 3, 2, 0, 0, 0 };
        }

        public virtual void ReadFromFile(BinaryReader reader, VersionsEnumerator version) { }

        public virtual void WriteToFile(BinaryWriter writer, VersionsEnumerator version) { }

        public virtual string GetFileName()
        {
            return "Invalid";
        }

        public virtual ulong GetFileHash()
        {
            return ulong.MinValue;
        }
    }
}