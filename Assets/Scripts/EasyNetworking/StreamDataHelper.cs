using System;
using System.IO;
using UnityEngine;

namespace  EasyNetworking
{
    public static class StreamDataHelper
    {
        public static object ReadNextParameter(BinaryReader reader, Type parameterType)
        {
            switch (parameterType)
            {
                case Type _ when parameterType == typeof(int):
                    return reader.ReadInt32();
                case Type _ when parameterType == typeof(uint):
                    return reader.ReadUInt32();
                case Type _ when parameterType == typeof(float):
                    return reader.ReadSingle();
                case Type _ when parameterType == typeof(bool):
                    return reader.ReadBoolean();
                case Type _ when parameterType == typeof(short):
                    return reader.ReadInt16();
                case Type _ when parameterType == typeof(ushort):
                    return reader.ReadUInt16();
                case Type _ when parameterType == typeof(long):
                    return reader.ReadInt64();
                case Type _ when parameterType == typeof(ulong):
                    return reader.ReadUInt64();
                case Type _ when parameterType == typeof(string):
                    return reader.ReadString();
                case Type _ when parameterType == typeof(char):
                    return reader.ReadChar();
                case Type _ when parameterType == typeof(byte):
                    return reader.ReadByte();
                case Type _ when parameterType == typeof(double):
                    return reader.ReadDouble();
                case Type _ when parameterType == typeof(decimal):
                    return reader.ReadDecimal();
                
                case Type _ when parameterType == typeof(Vector3):
                    return ReadVector3(reader);
                case Type _ when parameterType == typeof(Vector2):
                    return ReadVector2(reader);
                case Type _ when parameterType == typeof(Quaternion):
                    return ReadQuaternion(reader);
            }
            
            Debug.LogError($"StreamHandler | Получено сообщение с параметром неизвестного типа");
            return null;
        }
        
        #region Read special data type from stream
        public static Vector2 ReadVector2(BinaryReader reader)
        {
            return new Vector2()
            {
                x = reader.ReadSingle(),
                y = reader.ReadSingle()
            };
        }
        
        public static Vector3 ReadVector3(BinaryReader reader)
        {
            return new Vector3()
            {
                x = reader.ReadSingle(),
                y = reader.ReadSingle(),
                z = reader.ReadSingle()
            };
        }
        
        public static Quaternion ReadQuaternion(BinaryReader reader)
        {
            return new Quaternion()
            {
                x = reader.ReadSingle(),
                y = reader.ReadSingle(),
                z = reader.ReadSingle(),
                w = reader.ReadSingle()
            };
        }
        
        #endregion
        
        public static void Write(BinaryWriter writer, object variable)
        {
            switch (variable)
            {
                case int var: writer.Write(var); return;
                case uint var: writer.Write(var); return;
                case float var: writer.Write(var); return;
                case bool var: writer.Write(var); return;
                case short var: writer.Write(var); return;
                case ushort var: writer.Write(var); return;
                case long var: writer.Write(var); return;
                case ulong var: writer.Write(var); return;
                case string var: writer.Write(var); return;
                case char var: writer.Write(var); return;
                case byte var: writer.Write(var); return;
                case double var: writer.Write(var); return;
                case decimal var: writer.Write(var); return;
                
                case Vector3 var: Write(writer, var); return;
                case Vector2 var: Write(writer, var); return;
                case Quaternion var: Write(writer, var); return;
            }
            Debug.LogError($"StreamHandler | Отправка переменной типа {variable.GetType()} не поддерживается");
        }
        
        #region Write special data type to stream
        public static void Write(BinaryWriter writer, Vector2 vector2)
        {
            writer.Write(vector2.x);
            writer.Write(vector2.y);
        }
        
        public static void Write(BinaryWriter writer, Vector3 vector3)
        {
            writer.Write(vector3.x);
            writer.Write(vector3.y);
            writer.Write(vector3.z);
        }
        
        public static void Write(BinaryWriter writer, Quaternion quaternion)
        {
            writer.Write(quaternion.x);
            writer.Write(quaternion.y);
            writer.Write(quaternion.z);
            writer.Write(quaternion.w);
        }
        #endregion

        
    }
}

