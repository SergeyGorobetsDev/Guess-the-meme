using Assets.Code.Scripts.Runtime.InputSystem;
using UnityEngine;

namespace Assets.Project.Code.Gameplay.CameraSystem
{
    public sealed class CameraHandler : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 1.5f, -4f);
        [SerializeField] private float cameraDistance = 5f;
        [SerializeField] private float minVerticalAngle = -30f;
        [SerializeField] private float maxVerticalAngle = 70f;
        [SerializeField] private float rotationSmoothTime = 0.12f;
        [SerializeField] private float mouseSensitivity = 3f;
        [SerializeField] private float controllerSensitivity = 5f;

        [Header("Collision Settings")]
        [SerializeField] private float collisionOffset = 0.2f;
        [SerializeField] private float minCameraDistance = 0.5f;
        [SerializeField] private LayerMask collisionLayers;

        private Vector3 targetPosition;
        private Vector3 rotationSmoothVelocity;
        private Vector3 currentRotation;
        private float actualCameraDistance;
        private float xRotation;
        private float yRotation;
        private bool isUsingController;
        private Vector2 direction;
        private IPlayerInputReader playerInputReader;

        public void SetTarget(Transform target, IPlayerInputReader playerInputReader)
        {
            this.playerTransform = target;
            this.playerInputReader = playerInputReader;
            if (playerInputReader != null)
            {
                playerInputReader.LookPerformed += OnDirectionChanged;
                playerInputReader.LookCanceled += OnDirectionChanged;
            }
        }

        private void OnDisable()
        {
            if (playerInputReader != null)
            {
                playerInputReader.LookPerformed -= OnDirectionChanged;
                playerInputReader.LookCanceled += OnDirectionChanged;
            }
        }

        private void Start()
        {
            actualCameraDistance = cameraDistance;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (playerTransform == null) return;

            HandleRotationInput();
            Quaternion cameraRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            Vector3 desiredPosition = playerTransform.position + cameraRotation * cameraOffset.normalized * actualCameraDistance;
            HandleCameraCollision(ref desiredPosition);
            transform.position = desiredPosition;
            transform.LookAt(playerTransform.position + Vector3.up * cameraOffset.y);
            targetPosition = playerTransform.position;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10f);
        }

        private void OnDirectionChanged(Vector2 direction) =>
          this.direction = direction;

        private void HandleRotationInput()
        {
            float mouseX = this.direction.x;
            float mouseY = this.direction.y;

            float sensitivity = isUsingController ? controllerSensitivity : mouseSensitivity;
            yRotation += mouseX * sensitivity;
            xRotation -= mouseY * sensitivity;

            xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(xRotation, yRotation), ref rotationSmoothVelocity, rotationSmoothTime);
            xRotation = currentRotation.x;
            yRotation = currentRotation.y;
        }

        private void HandleCameraCollision(ref Vector3 desiredPosition)
        {
            RaycastHit hit;
            Vector3 direction = (desiredPosition - playerTransform.position).normalized;
            float targetDistance = cameraDistance;

            if (Physics.SphereCast(
                playerTransform.position + Vector3.up * cameraOffset.y,
                0.2f,
                direction,
                out hit,
                cameraDistance,
                collisionLayers))
            {
                targetDistance = hit.distance - collisionOffset;
            }

            actualCameraDistance = Mathf.Lerp(actualCameraDistance, targetDistance, Time.deltaTime * 10f);
            actualCameraDistance = Mathf.Clamp(actualCameraDistance, minCameraDistance, cameraDistance);
            desiredPosition = playerTransform.position + Quaternion.Euler(xRotation, yRotation, 0f) * cameraOffset.normalized * actualCameraDistance;
        }

        public Quaternion GetCameraRotation() => Quaternion.Euler(0f, yRotation, 0f);
        public Vector3 GetCameraForward() => Quaternion.Euler(0f, yRotation, 0f) * Vector3.forward;
        public Vector3 GetCameraRight() => Quaternion.Euler(0f, yRotation, 0f) * Vector3.right;
    }
}