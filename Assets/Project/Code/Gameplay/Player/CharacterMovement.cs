using Assets.Code.Scripts.Runtime.InputSystem;
using Assets.Project.Code.Gameplay.CameraSystem;
using UnityEngine;

namespace Assets.Project.Code.Common.Player
{
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField]
        public float speed = 1f;
        [SerializeField]
        private float rotationSpeed = 10f;

        [SerializeField]
        public float groundCheckDistance = 0.2f;
        [SerializeField]
        public LayerMask groundMask;

        private Vector3 velocity;
        private bool isGrounded;
        [SerializeField]
        private bool canMove = true;

        private CharacterController characterController;
        [SerializeField]
        private Vector2 direction;
        private IPlayerInputReader playerInputReader;
        private ICameraProvider cameraProvider;
        private Camera characterCam;

        public void SetCanMove(bool canMove) => this.canMove = canMove;

        public void SetUp(CharacterController characterController, IPlayerInputReader playerInputReader, ICameraProvider cameraProvider)
        {
            this.characterController = characterController;
            this.playerInputReader = playerInputReader;
            this.cameraProvider = cameraProvider;
            this.characterCam = cameraProvider.GetCamera();

            if (playerInputReader != null)
            {
                playerInputReader.MovePerformed += OnDirectionChanged;
                playerInputReader.MoveCanceled += OnDirectionChanged;
            }
        }

        private void OnEnable()
        {
            if (playerInputReader != null)
            {
                playerInputReader.MovePerformed += OnDirectionChanged;
                playerInputReader.MoveCanceled += OnDirectionChanged;
            }
        }

        private void OnDisable()
        {
            if (playerInputReader != null)
            {
                playerInputReader.MovePerformed -= OnDirectionChanged;
                playerInputReader.MoveCanceled += OnDirectionChanged;
            }
        }

        private void Update()
        {
            if (characterController == null ||
                !canMove ||
                characterCam == null)
                return;

            MoveAndRotate(direction);
            ToGround();
        }

        private void OnDirectionChanged(Vector2 direction) =>
           this.direction = direction;

        private void MoveAndRotate(Vector2 direction)
        {
            Vector3 moveDirection = CalculateCameraRelativeMovement(direction);

            if (moveDirection != Vector3.zero)
            {
                characterController.Move(moveDirection * speed * Time.deltaTime);
                RotateTowardsMovement(moveDirection);
            }
        }

        private void ToGround()
        {
            isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
            if (isGrounded && velocity.y < 0)
                velocity.y = -2f;
            velocity.y += Physics.gravity.y * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }

        private Vector3 CalculateCameraRelativeMovement(Vector2 inputDirection)
        {
            Vector3 cameraForward = characterCam.transform.forward;
            Vector3 cameraRight = characterCam.transform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();
            return cameraForward * inputDirection.y + cameraRight * inputDirection.x;
        }

        private void RotateTowardsMovement(Vector3 moveDirection)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}