using ObjLoader.Loader.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3Dengine.Components
{
    /// <summary>
    /// Linkt een obj bestand op de correcte manier met een bijbehorend mtl bestand met materials.
    /// </summary>
    public class ObjMtlFileLoader : IMaterialStreamProvider
    {
        private string objPath;

        public ObjMtlFileLoader(string objPath)
        {
            this.objPath = objPath;
        }

        public Stream Open(string materialFilePath)
        {
            if (!Path.IsPathRooted(materialFilePath))
            {
                FileInfo fi = new FileInfo(objPath);
                materialFilePath = fi.DirectoryName + Path.DirectorySeparatorChar + materialFilePath;
            }

            return File.Open(materialFilePath, FileMode.Open, FileAccess.Read);
        }
    }
}
