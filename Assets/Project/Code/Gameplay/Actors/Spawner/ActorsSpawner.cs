using Assets.Code.Scripts.Runtime.InputSystem;
using Assets.Project.Code.Common.Player;
using Assets.Project.Code.Gameplay.CameraSystem;
using Assets.Project.Code.Gameplay.StaticData;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Gameplay.Actors.Spawner
{
    public interface IActorsSpawner
    {
        void SpawnCharacter(List<Vector3> positions);
        void SpawnActors(int amount, List<Vector3> positions);
        void RespawnActiveActors(List<Vector3> positions);
        void RespawnCharacter(List<Vector3> positions);
    }

    public interface IActorsProvider
    {
        IEnumerable<Actor> Actors { get; }
        Character Player { get; }
    }

    public class ActorsSpawner : IActorsSpawner, IActorsProvider
    {
        private const int DefaulActorsAmount = 5;
        private List<Actor> spawnedActors = new(DefaulActorsAmount);
        private Character character;

        public IEnumerable<Actor> Actors => spawnedActors;
        public Character Player => character;

        private readonly IInstantiator instantiator;
        private readonly IStaticDataService staticDataService;
        private readonly ICameraProvider cameraProvider;
        private readonly IPlayerInputReader playerInputReader;

        [Inject]
        public ActorsSpawner(IInstantiator instantiator,
                             IStaticDataService staticDataService,
                             ICameraProvider cameraProvider,
                             IPlayerInputReader playerInputReader)
        {
            this.instantiator = instantiator;
            this.staticDataService = staticDataService;
            this.cameraProvider = cameraProvider;
            this.playerInputReader = playerInputReader;
        }

        public void SpawnCharacter(List<Vector3> positions)
        {
            Character player = instantiator.InstantiatePrefabForComponent<Character>(staticDataService.Character);
            character = player;
            player.gameObject.transform.SetPositionAndRotation(positions[Random.Range(0, positions.Count)], Quaternion.identity);
            player.Initialize();
            cameraProvider.GetCameraHandler().SetTarget(player.transform, playerInputReader);
        }

        public void SpawnActors(int amount, List<Vector3> positions)
        {
            if (positions.Count < amount)
            {
                Debug.LogWarning($"Not enough spawn positions available. Requested: {amount}, Available: {positions.Count}");
                amount = positions.Count;
            }

            for (int i = 0; i < amount; i++)
            {
                Actor actor = instantiator.InstantiatePrefabForComponent<Actor>(staticDataService.Actor);
                actor.gameObject.transform.SetPositionAndRotation(positions[Random.Range(i, positions.Count)], Quaternion.identity);
                spawnedActors.Add(actor);
                actor.Initialize();
            }
        }

        public void RespawnActiveActors(List<Vector3> positions)
        {
            foreach (var actor in spawnedActors)
            {
                if (actor.gameObject.activeSelf)
                {
                    actor.Reset();
                    actor.gameObject.transform.SetPositionAndRotation(positions[Random.Range(0, positions.Count)], Quaternion.identity);
                }

            }
        }

        public void RespawnCharacter(List<Vector3> positions)
        {
            character.Reset();
            character.gameObject.transform.SetPositionAndRotation(positions[Random.Range(0, positions.Count)], Quaternion.identity);
        }

        public void Clear()
        {
            foreach (var actor in spawnedActors)
            {
                if (actor != null)
                {
                    Object.Destroy(actor.gameObject);
                }
            }

            Object.Destroy(character.gameObject);
        }
    }
}