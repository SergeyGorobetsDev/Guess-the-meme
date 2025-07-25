using UnityEngine;

namespace Assets.Project.Code.Common.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Game / Player / PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField]
        public float MovementSpeed { get; private set; }
    }

}
