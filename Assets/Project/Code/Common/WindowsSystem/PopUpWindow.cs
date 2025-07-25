namespace Assets.Project.Code.Runtime.Gameplay.Common.WindowsSystem
{
    public abstract class PopUpWindow : Window
    {
        public virtual void ChangeDisplayOrder(int order) =>
            canvas.sortingOrder = order;
    }
}
