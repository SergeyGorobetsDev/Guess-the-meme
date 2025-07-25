using UnityEngine;

namespace Assets.Project.Code.Gameplay.Actors
{
    [CreateAssetMenu(fileName = "ActorData", menuName = "Game / Actors / ActorData", order = 1)]
    public class ActorData : ScriptableObject
    {
        [field: SerializeField]
        public float Speed { get; set; }

        [field: SerializeField]
        public float Radius { get; set; }
    }
}
