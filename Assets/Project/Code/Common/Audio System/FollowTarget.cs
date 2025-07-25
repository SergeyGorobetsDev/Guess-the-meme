using UnityEngine;

namespace Assets.Project.Code.Runtime.Gameplay.Common.AudioSystem
{
    public class FollowTarget : MonoBehaviour
    {
        public Transform target;

        private void Update()
        {
            if (target)
                transform.position = target.position;
        }
    }
}