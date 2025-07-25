namespace Assets.Project.Code.Gameplay.Zones
{
    public interface IZoneProvider
    {
        void Initialize(ZoneTrigger[] zoneTriggers);
        ZoneTrigger[] GetZoneTriggers();
    }

    public sealed class ZoneProvider : IZoneProvider
    {
        private ZoneTrigger[] zoneTriggers;

        public void Initialize(ZoneTrigger[] zoneTriggers)
        {
            this.zoneTriggers = zoneTriggers;
        }

        public ZoneTrigger[] GetZoneTriggers() => zoneTriggers;
    }
}