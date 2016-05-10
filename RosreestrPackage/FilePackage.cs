using System.IO;

namespace RosreestrPackage
{
    public class FilePackage
    {
        public readonly string Name;
        public readonly string FullName;
        public readonly string RelativeName;
        public readonly string BasePath;
        public readonly string RelativePath;
        public readonly bool InSubDirectory;
        public readonly string DirectoryPath;
        public bool IsSigned;


        /// <summary>
        /// File to package
        /// </summary>
        /// <param name="filePath">Full path to file</param>
        /// <param name="inSubDirectory">if in sub directory - true</param>
        /// <param name="basePath">if in sub directory - path parent directory to xml file, else string empty</param>
        public FilePackage(string filePath, string basePath)
        {
            FullName = filePath;
            InSubDirectory = basePath.Length > 0;
            BasePath = basePath;
            DirectoryPath = Directory.GetParent(filePath).FullName;
            RelativePath = InSubDirectory ? GetRelativePath(basePath, DirectoryPath) : string.Empty;
            Name = Path.GetFileName(filePath);
            RelativeName = InSubDirectory ? GetRelativePath(basePath, filePath) : Name;
            IsSigned = false;
            //Debug.WriteLine("FullName: " + FullName);
            //Debug.WriteLine("Name: " + Name);
            //Debug.WriteLine("RelativePath: " + RelativePath);
            //Debug.WriteLine("RelativeName: " + RelativeName);
        }


        private string GetRelativePath(string basicPath, string filepath)
        {
            return filepath.StartsWith(basicPath) ? filepath.Substring(basicPath.Length + 1) : filepath;
        }

    }
}
