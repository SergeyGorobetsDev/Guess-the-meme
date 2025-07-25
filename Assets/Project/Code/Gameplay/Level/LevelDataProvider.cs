namespace Assets.Project.Code.Gameplay.Level
{
    public interface ILevelDataProvider
    {
        void Initialize(LevelData levelData);
        int GetActorAmountToSpawn();
        float GetRoundTime();
        int GetTotalRounds();
    }

    public class LevelDataProvider : ILevelDataProvider
    {
        private LevelData levelData;

        public void Initialize(LevelData levelData) =>
            this.levelData = levelData;

        public int GetActorAmountToSpawn() => levelData.ActorAmountToSpawn;
        public float GetRoundTime() => levelData.RoundTime;
        public int GetTotalRounds() => levelData.TotalRounds;
    }
}
