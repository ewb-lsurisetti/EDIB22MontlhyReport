namespace EdiMonthlyReportGenerator.Services.Interfaces
{
    public interface IFileSystemService
    {
        bool DirectoryExists(string path);
        void CreateDirectory(string path);
        string[] GetFilesFromDirectory(string path, string pattern);
        string CombinePath(params string[] paths);
        bool FileExists(string path);
        long GetFileLength(string path);
    }
}
