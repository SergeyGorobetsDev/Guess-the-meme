using Assets.Code.Scripts.Runtime.AssetManagement;
using Assets.Project.Code.Common.Player;
using Assets.Project.Code.Gameplay.Actors;
using Assets.Project.Code.Gameplay.Level;
using Assets.Project.Code.Gameplay.Meme;
using Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Gameplay.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        public LevelData LevelData { get; private set; }
        public AudioData AudioData { get; private set; }
        public MemeData MemeData { get; private set; }
        public Character Character { get; private set; }
        public Actor Actor { get; private set; }
        public Dictionary<int, Texture2D> MemeImages { get; private set; }

        private readonly IAssetProvider assetProvider;

        [Inject]
        public StaticDataService(IAssetProvider assetProvider)
        {
            this.assetProvider = assetProvider;
        }

        public async UniTask LoadAllAssets()
        {
            LevelData = await assetProvider.LoadAsync<LevelData>(AssetsAddress.LevelDataAddress);
            AudioData = await assetProvider.LoadAsync<AudioData>(AssetsAddress.AudioDataAddress);
            MemeData = await assetProvider.LoadAsync<MemeData>(AssetsAddress.MemeDataAddress);
            GameObject heroObject = await assetProvider.LoadAsync<GameObject>(AssetsAddress.CharacterAddress);
            Character = heroObject.GetComponent<Character>();
            GameObject actorObject = await assetProvider.LoadAsync<GameObject>(AssetsAddress.ActorAddress);
            Actor = actorObject.GetComponent<Actor>();

            MemeImages = new Dictionary<int, Texture2D>();
            for (int i = 0; i < MemeData.MemesImages.Length; i++)
                MemeImages.Add(i, MemeData.MemesImages[i]);
        }
    }
}