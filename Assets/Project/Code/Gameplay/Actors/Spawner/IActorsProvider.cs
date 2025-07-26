using Assets.Project.Code.Common.Player;
using System.Collections.Generic;

namespace Assets.Project.Code.Gameplay.Actors.Spawner
{
    public interface IActorsProvider
    {
        IEnumerable<Actor> Actors { get; }
        Character Player { get; }
    }
}