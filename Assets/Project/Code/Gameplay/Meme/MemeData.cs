using UnityEngine;

namespace Assets.Project.Code.Gameplay.Meme
{
    [CreateAssetMenu(fileName = "MemeData", menuName = "Game / Meme / MemeData", order = 1)]
    public class MemeData : ScriptableObject
    {
        [field: SerializeField]
        public Texture2D[] MemesImages { get; set; }
    }
}