using Assets.Project.Code.Common.Player;
using Assets.Project.Code.Gameplay.Actors;
using Assets.Project.Code.Gameplay.Level;
using Assets.Project.Code.Gameplay.Meme;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Code.Gameplay.StaticData
{
    public interface IStaticDataService
    {
        UniTask LoadAllAssets();
        LevelData LevelData { get; }
        AudioData AudioData { get; }
        MemeData MemeData { get; }
        Character Character { get; }
        Actor Actor { get; }

        Dictionary<int, Texture2D> MemeImages { get; }
    }
}