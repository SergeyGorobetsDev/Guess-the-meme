using System.Collections.Generic;
using UnityEngine;

namespace Assets.Project.Code.Gameplay.Actors.Spawner
{
    public interface IActorsSpawner
    {
        void SpawnCharacter(List<Vector3> positions);
        void SpawnActors(int amount, List<Vector3> positions);
        void RespawnActiveActors(List<Vector3> positions);
        void RespawnCharacter(List<Vector3> positions);
        void Clear();
        void DespawnActors();
        void DespawnPlayer();
    }
}