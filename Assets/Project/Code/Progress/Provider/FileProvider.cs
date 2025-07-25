using Cysharp.Threading.Tasks;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Code.Scripts.Runtime.Progress
{
    public sealed class FileProvider : IFileProvider
    {
        public FileProvider() { }

        public async UniTask<string> ReadFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return string.Empty;

            using FileStream sourceStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
            using StreamReader reader = new(sourceStream);
            StringBuilder sb = new();

            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync();
                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        public async UniTask WriteFileAsync(string filePath, string text)
        {
            using FileStream destinationStream = new(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, bufferSize: 4096, useAsync: true);
            using StreamWriter writer = new(destinationStream);
            await writer.WriteLineAsync(text);
        }

        public void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
                File.Delete(filePath);
            else
            {
#if UNITY_EDITOR
                Debug.Log($"Can't delete file : {filePath}");
#endif
            }
        }

        public string[] FilesFromDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
            return Directory.GetFiles(directoryPath, "*.json");
        }
    }
}