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


        /// <summary>
        /// File to package
        /// </summary>
        /// <param name="filePath">Full path to file</param>
        /// <param name="inSubDirectory">if in sub directory - true</param>
        /// <param name="basePath">if in sub directory - path parent directory to xml file, else string empty</param>
        public FilePackage(string filePath, bool inSubDirectory, string basePath)
        {
            FullName = filePath;
            InSubDirectory = inSubDirectory;
            BasePath = basePath;
            DirectoryPath = Directory.GetParent(filePath).FullName;
            RelativePath = inSubDirectory ? GetRelativePath(basePath, DirectoryPath) : string.Empty;
            Name = Path.GetFileName(filePath);
            RelativeName = inSubDirectory ? GetRelativePath(basePath, filePath) : Name;

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
