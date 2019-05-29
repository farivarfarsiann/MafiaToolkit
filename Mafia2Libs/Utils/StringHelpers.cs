﻿using System;
using System.IO;

namespace Utils.StringHelpers
{
    public class StringHelpers
    {
        public static Random RandomGenerator = new Random();

        public static string ReadString8(BinaryReader reader)
        {
            byte size = reader.ReadByte();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString16(BinaryReader reader)
        {
            short size = reader.ReadInt16();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString32(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            return new string(reader.ReadChars(size));
        }
        public static string ReadString64(BinaryReader reader)
        {
            long size = reader.ReadInt64();
            return new string(reader.ReadChars((int)size));
        }
        public static string ReadString(BinaryReader reader)
        {
            string newString = "";

            while (reader.PeekChar() != '\0')
            {
                newString += reader.ReadChar();
            }
            reader.ReadByte();
            return newString;
        }
        public static void WriteStringBuffer(BinaryWriter writer, int size, string text, char trim = ' ')
        {
            bool addTrim = (trim == ' ' ? false : true);
            int padding = size - text.Length;
            writer.Write(text.ToCharArray());

            if (addTrim)
            {
                writer.Write('\0');
                padding -= 1;
            }

            writer.Write(new byte[padding]);
        }
        public static void WriteString(BinaryWriter writer, string text)
        {
            writer.Write(text.ToCharArray());
            writer.Write('\0');
        }
        public static void WriteString8(BinaryWriter writer, string text)
        {
            writer.Write((byte)text.Length);
            writer.Write(text.ToCharArray());
        }
        public static void WriteString16(BinaryWriter writer, string text)
        {
            writer.Write((ushort)text.Length);
            writer.Write(text.ToCharArray());
        }
        public static void WriteString32(BinaryWriter writer, string text)
        {
            writer.Write(text.Length);
            writer.Write(text.ToCharArray());
        }
    }

}
