using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace TamanaEngine.Core
{
    public static class FileManager
    {
        private static string shadersLocation = string.Empty;

        private const string SHADER_FOLDER_NAME = "Shaders";

        public static void CopyShadersToGameDirectory()
        {
            var rootDirectory = Directory.GetParent("../../res").FullName;
            GetShadersLocation(rootDirectory);

            if (string.IsNullOrEmpty(shadersLocation))
            {
                throw new Exception("Can't find 'Shaders' folder inside your project.");
            }

            var shaderFiles = Directory.GetFiles(shadersLocation);

            foreach(var file in shaderFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
                var destination = string.Format("./res/{0}", fileInfo.Name);
                if (File.Exists(destination))
                {
                    if(!CompareFiles(file, destination))
                    {
                        Console.WriteLine("File is not same");
                        File.Copy(file, destination, true);
                    }

                    continue;
                }
                
                File.Copy(file, destination);
            }
        }

        private static void GetShadersLocation(string startLocation)
        {
            var foldersInsideRootDirectory = Directory.GetDirectories(startLocation);
            foreach (var dir in foldersInsideRootDirectory)
            {
                var directoryInfo = new DirectoryInfo(dir);
                if (directoryInfo.Name == SHADER_FOLDER_NAME)
                {
                    shadersLocation = directoryInfo.FullName;
                    break;
                }
                
                GetShadersLocation(dir);
            }
        }

        private static bool CompareFiles(string filePath1, string filePath2)
        {
            var file1 = File.ReadAllBytes(filePath1);
            var file2 = File.ReadAllBytes(filePath2);
            
            if (file1.Length != file2.Length)
                return false;

            for (int i = 0; i < file1.Length; i++)
                if (file1[i] != file2[i])
                    return false;

            return true;
        }

        public static void Write()
        {
            int[] statuses = { 100, 200 };
            var bytes = ToByteArray(new Wira("Wira", 1, 10, statuses));
            File.WriteAllBytes("./res/Wira", bytes);

            var loadedBytes = File.ReadAllBytes("./res/Wira");
            Wira wira = FromByteArray<Wira>(loadedBytes);

            if(wira.name != string.Empty)
            {
                Console.WriteLine("Name : {0} | Level : {1} | Attack : {2} | Statuses {3} and {4}.", wira.name, wira.level, wira.attack, wira.statuses[0], wira.statuses[1]);
            }

            File.Delete("./res/Wira");
        }

        public static byte[] ToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }

        [Serializable]
        public struct Wira
        {
            public string name;
            public int level;
            public int attack;
            public int[] statuses;

        public Wira(string name, int level, int attack, int[] statuses)
            {
                this.name = name;
                this.level = level;
                this.attack = attack;
                this.statuses = statuses;
            }
        }
    }
}
