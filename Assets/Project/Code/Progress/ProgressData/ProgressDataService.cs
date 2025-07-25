using Assets.Code.Scripts.Runtime.Progress;
using Assets.Project.Code.Shared;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Progress.ProgressData
{
    public sealed class ProgressDataService : ISaveLoadService
    {
        [Header("CONFIGURATION DATA")]
        private string directoryPath;

        private UserData activeData;
        public UserData ActiveData => activeData;

        [Header("Injected Services")]
        private readonly IFileProvider filDataHandler;

        [Inject]
        public ProgressDataService(IFileProvider fileDataHandler)
        {
            this.filDataHandler = fileDataHandler;
            this.directoryPath = CreateDirectoryPath();
        }

        public void Initialize() { }

        public async UniTask LoadAsync()
        {
            string data = await filDataHandler.ReadFileAsync(directoryPath);
            activeData = string.IsNullOrEmpty(data) || data == null ? new UserData() : JsonConvert.DeserializeObject<UserData>(data);
        }

        public async UniTask SaveAsync()
        {
            await filDataHandler.WriteFileAsync(CreateFilePath(), JsonConvert.SerializeObject(activeData));
        }

        public void DeleteSave(int id) =>
            filDataHandler.DeleteFile(CreateFilePath());

        private string CreateDirectoryPath()
        {
#if UNITY_EDITOR
            return Path.Combine(Application.dataPath, AppConfigs.SaveFileFolder);
#elif UNITY_ANDROID
            return Path.Combine(Application.persistentDataPath,AppConfigs.SaveFileFolder);
#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE
            return Path.Combine(Application.persistentDataPath, AppConfigs.SaveFileFolder);
#endif
        }

        private string CreateFilePath() =>
            Path.Combine(directoryPath, AppConfigs.SaveFileFolder + AppConfigs.SaveFile + AppConfigs.SaveFileFormat);
    }
}