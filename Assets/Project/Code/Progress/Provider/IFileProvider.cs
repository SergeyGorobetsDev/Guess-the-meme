using Cysharp.Threading.Tasks;

namespace Assets.Code.Scripts.Runtime.Progress
{
    public interface IFileProvider
    {
        UniTask<string> ReadFileAsync(string filePath);
        UniTask WriteFileAsync(string filePath, string text);
        string[] FilesFromDirectory(string directoryPath);
        void DeleteFile(string filePath);
    }
}