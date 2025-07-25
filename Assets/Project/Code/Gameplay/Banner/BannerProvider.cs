namespace Assets.Project.Code.Gameplay.Banner
{
    public class BannerProvider : IBannerProvider
    {
        private BannerHandler bannerHandler;

        public void Initialize(BannerHandler bannerHandler) =>
            this.bannerHandler = bannerHandler;

        public BannerHandler GetBannerHandler() =>
            bannerHandler;
    }
}