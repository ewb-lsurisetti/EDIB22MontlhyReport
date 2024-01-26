using EdiMonthlyReportGenerator.Services.Interfaces;

namespace EdiMonthlyReportGenerator.Services.Implements
{
    public class FileSystemService : IFileSystemService
    {
        public bool DirectoryExists(string path) => Directory.Exists(path);
        public void CreateDirectory(string path) => Directory.CreateDirectory(path);
        public string[] GetFilesFromDirectory(string path, string pattern) => Directory.GetFiles(path, pattern);
        public string CombinePath(params string[] paths) => Path.Combine(paths);
        public bool FileExists(string path) => File.Exists(path);
        public long GetFileLength(string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                return fileStream.Length;
            }
        }
    }
}
