using UnityEngine;

namespace Assets.Project.Code.Gameplay.CameraSystem
{
    public interface ICameraProvider
    {
        void Initialize(CameraHandler cameraHandler, Camera camera);
        Camera GetCamera();
        CameraHandler GetCameraHandler();
    }
}
