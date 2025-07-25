using UnityEngine;

namespace Assets.Project.Code.Gameplay.Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "Game / Level / LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        [field: SerializeField, Min(1)]
        public int ActorAmountToSpawn { get; private set; } = 1;

        [field: SerializeField, Min(1)]
        public float RoundTime { get; private set; } = 30f;

        [field: SerializeField, Min(1)]
        public int TotalRounds { get; private set; } = 1;
    }
}
