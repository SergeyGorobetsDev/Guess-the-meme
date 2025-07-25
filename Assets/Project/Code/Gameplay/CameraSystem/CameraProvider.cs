using UnityEngine;

namespace Assets.Project.Code.Gameplay.CameraSystem
{
    public sealed class CameraProvider : ICameraProvider
    {
        private Camera camera;
        private CameraHandler cameraHandler;

        public void Initialize(CameraHandler cameraHandler, Camera camera)
        {
            this.cameraHandler = cameraHandler;
            this.camera = camera;
        }

        public Camera GetCamera() => camera;
        public CameraHandler GetCameraHandler() => cameraHandler;
    }
}
