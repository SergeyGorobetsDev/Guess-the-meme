using Cysharp.Threading.Tasks;

namespace Assets.Project.Code.Progress.ProgressData
{
    public interface ISaveLoadService
    {
        UserData ActiveData { get; }
        void Initialize();
        UniTask LoadAsync();
        UniTask SaveAsync();
        void DeleteSave(int id);
    }
}