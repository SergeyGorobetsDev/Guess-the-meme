namespace Assets.Project.Code.Gameplay.Actors.Spawner
{
    public interface ISpawnAreaProvider
    {
        void Initialize(SpawnArea spawnArea);
        SpawnArea GetSpawnArea();
    }

    public class SpawnAreaProvider : ISpawnAreaProvider
    {
        private SpawnArea spawnArea;

        public void Initialize(SpawnArea spawnArea) =>
            this.spawnArea = spawnArea;

        public SpawnArea GetSpawnArea() => spawnArea;
    }
}
