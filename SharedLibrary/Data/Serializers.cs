using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeongHun.EmueraFramework.Data
{
    internal static class Serializers
    {
        private delegate void Serializer<T>(Stream stream, T[] data);
        private delegate T[] DeSerializer<T>(Stream stream);

        public static void Serialize<T>(Stream stream, T[] data)
        {
            typeof(Serializers).GetRuntimeMethods()
                .Select(method =>
                {
                    try
                    {
                        return method.CreateDelegate(typeof(Serializer<T>)) as Serializer<T>;
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(serializer => serializer != null)
                .First()(stream, data);
        }

        public static T[] DeSerialize<T>(Stream stream)
        {
            return typeof(Serializers).GetRuntimeMethods()
                .Select(method =>
                {
                    try
                    {
                        return method.CreateDelegate(typeof(DeSerializer<T>)) as DeSerializer<T>;
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(serializer => serializer != null)
                .First()(stream);
        }

        private static long[] Int64DeSerialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int length = reader.ReadInt32();
            long[] array = new long[length];
            byte[] buffer = new byte[length * sizeof(long)];
            stream.Read(buffer, 0, buffer.Length);
            Buffer.BlockCopy(buffer, 0, array, 0, buffer.Length);
            buffer = null;
            return array;
        }

        private static void Int64Serialize(Stream stream, long[] data)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            byte[] buffer = new byte[data.Length * sizeof(long)];
            Buffer.BlockCopy(data, 0, buffer, 0, buffer.Length);
            writer.Write(data.Length);
            stream.Write(buffer, 0, buffer.Length);
            buffer = null;
        }
        private static string[] StringDeSerialize(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            int length = reader.ReadInt32();
            string[] array = new string[length];
            for (int i = 0; i < length; i++)
                array[i] = reader.ReadString();
            return array;
        }

        private static void StringSerialize(Stream stream, string[] data)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            int length = data.Length;
            writer.Write(length);
            for (int i = 0; i < length; i++)
                writer.Write(data[i]);
        }
    }
}
