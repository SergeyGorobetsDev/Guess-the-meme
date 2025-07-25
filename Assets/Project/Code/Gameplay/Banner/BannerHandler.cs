using UnityEngine;

namespace Assets.Project.Code.Gameplay.Banner
{
    public class BannerHandler : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer meshRenderer;

        public void ChangeImage(Texture2D texture)
        {
            if (meshRenderer == null)
            {
                Debug.LogError("Material is not assigned in BannerHandler.");
                return;
            }

            if (texture == null)
            {
                Debug.LogError("Texture is null. Cannot change image.");
                return;
            }

            meshRenderer.material.mainTexture = texture;
        }
    }
}