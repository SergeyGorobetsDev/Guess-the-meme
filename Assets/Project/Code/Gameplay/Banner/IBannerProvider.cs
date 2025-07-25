namespace Assets.Project.Code.Gameplay.Banner
{
    public interface IBannerProvider
    {
        void Initialize(BannerHandler bannerHandler);
        BannerHandler GetBannerHandler();
    }
}