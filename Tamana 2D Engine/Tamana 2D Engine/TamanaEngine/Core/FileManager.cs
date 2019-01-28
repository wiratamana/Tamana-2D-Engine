using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TamanaEngine.Core
{
    public static class FileManager
    {
        private static readonly string rootDirectory =
            @"C:\Users\wirat\Documents\Tamana 2D Engine\Tamana-2D-Engine\Tamana 2D Engine\Tamana 2D Engine";

        public static void CopyShadersToGameDirectory()
        {
            var directories = Directory.GetDirectories(rootDirectory);
            foreach (var dir in directories)
                Console.WriteLine(dir);
        }
    }
}
