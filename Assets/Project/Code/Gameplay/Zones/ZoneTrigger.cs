using Assets.Project.Code.Gameplay.Interactable;
using UnityEngine;

namespace Assets.Project.Code.Gameplay.Zones
{
    public class ZoneTrigger : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer textureMeshRenderer;
        [SerializeField]
        private ZoneCorrectness zoneCorrectness;

        public ZoneCorrectness ZoneCorrectness =>
            zoneCorrectness;

        public void SetZoneCorrectness(ZoneCorrectness zoneCorrectness, Texture2D texture)
        {
            this.zoneCorrectness = zoneCorrectness;
            textureMeshRenderer.material.mainTexture = texture;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactor))
                interactor.Interact(zoneCorrectness, true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactor))
                interactor.Interact(ZoneCorrectness.Incorrect, false);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Color gizmoColor = zoneCorrectness switch
            {
                ZoneCorrectness.Correct => Color.green,
                ZoneCorrectness.Incorrect => Color.red,
                _ => Color.yellow
            };

            Gizmos.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, new Vector3(1f, 0.1f, 1f));
        }
#endif
    }
}