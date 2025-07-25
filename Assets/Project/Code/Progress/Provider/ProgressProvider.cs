using Assets.Project.Code.Progress.ProgressData;

namespace Assets.Code.Scripts.Runtime.Progress
{
    public sealed class ProgressProvider : IProgressProvider
    {
        private UserData userData;
        public void SetUserData(UserData data) =>
            userData = data;

        public UserData GetUserData() => userData;
    }
}