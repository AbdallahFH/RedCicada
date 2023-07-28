using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
class DataFile
{
    float f;
}
[Serializable]
class ImageFile : DataFile
{
    public byte[] Data;
    public int Width;
    public int Height;
    public int nrChannels;
}
class SceneFile : DataFile
{
    public byte[] Data;
}
class AudioFile : DataFile
{
    public byte[] Data;
}

namespace RedCicada
{
    class FileManager
    {
        public static void SaveFile<T>(string path,T data) where T: DataFile{
            using (var stream = new FileStream(path,FileMode.Create,FileAccess.Write)){
                var formater = new BinaryFormatter();
                #pragma warning disable SYSLIB0011
                formater.Serialize(stream, data);
                #pragma warning restore SYSLIB0011

            }
        }
        public static T LoadFile<T>(string path) where T : DataFile{
            if(File.Exists(path)){
                using(var stream = new FileStream(path,FileMode.Open,FileAccess.Read)){
                    var formater = new BinaryFormatter();
                    #pragma warning disable SYSLIB0011
                    T data = (T)formater.Deserialize(stream);
                    #pragma warning restore SYSLIB0011
                    return data;
                }
            }else{
                return null;
            }
        }
    }
}