using Assets.Project.Code.Gameplay.Zones;

namespace Assets.Project.Code.Gameplay.Interactable
{
    public interface IInteractable
    {
        void Interact(ZoneCorrectness zoneCorrectnessm, bool isInZone);
    }
}
