using Assets.Code.Scripts.Runtime.InputSystem;
using Assets.Project.Code.Gameplay.CameraSystem;
using Assets.Project.Code.Gameplay.Interactable;
using Assets.Project.Code.Gameplay.Zones;
using UnityEngine;
using Zenject;

namespace Assets.Project.Code.Common.Player
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(CharacterController))]
    public class Character : MonoBehaviour, IInteractable
    {
        [Header("Config")]
        [SerializeField]
        private PlayerData playerData;

        [Header("Components")]
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private CharacterMovement characterMovement;

        private IPlayerInputReader playerInputReader;
        private ICameraProvider cameraProvider;

        public CharacterController CharacterController => characterController;
        public CharacterMovement CharacterMovement => characterMovement;

        [field: SerializeField]
        public ZoneCorrectness ZoneCorrectnessm { get; private set; } = ZoneCorrectness.None;
        [field: SerializeField]
        public bool IsInZone { get; private set; } = false;

        [Inject]
        public void Constructor(IPlayerInputReader playerInputReader, ICameraProvider cameraProvider)
        {
            this.playerInputReader = playerInputReader;
            this.cameraProvider = cameraProvider;
        }

        public void Initialize()
        {
            if (TryGetComponent<CharacterController>(out var characterController))
                this.characterController = characterController;

            if (TryGetComponent<CharacterMovement>(out var characterMovement))
            {
                this.characterMovement = characterMovement;
                this.characterMovement.SetUp(this.characterController, playerInputReader, cameraProvider);
            }
        }

        public void Interact(ZoneCorrectness zoneCorrectnessm, bool isInZone)
        {
            ZoneCorrectnessm = zoneCorrectnessm;
            IsInZone = isInZone;
        }

        public void Reset()
        {
            characterMovement.SetCanMove(false);
            ZoneCorrectnessm = ZoneCorrectness.None;
            IsInZone = false;
        }
    }
}