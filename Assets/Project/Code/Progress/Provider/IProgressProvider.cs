using Assets.Project.Code.Progress.ProgressData;

namespace Assets.Code.Scripts.Runtime.Progress
{
    public interface IProgressProvider
    {
        void SetUserData(UserData data);
        UserData GetUserData();
    }
}