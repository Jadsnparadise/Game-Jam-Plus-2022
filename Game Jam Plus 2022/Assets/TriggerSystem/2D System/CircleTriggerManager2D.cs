using System.Collections.Generic;
using UnityEngine;

namespace Trigger.System2D.Manager
{
    public class CircleTriggerManager2D : MonoBehaviour
    {
        [SerializeField] Core.UpdateMethod updateMethod = Core.UpdateMethod.FixedUpdate;
        [SerializeField] List<CircleTrigger2D> triggers = new List<CircleTrigger2D>() { new CircleTrigger2D() };

        void Update()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.Update)) return;
            foreach (CircleTrigger2D trigger in triggers)
            {
                trigger.InTrigger(transform.position);
            }
        }

        void FixedUpdate()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.FixedUpdate)) return;
            foreach (CircleTrigger2D trigger in triggers)
            {
                trigger.InTrigger(transform.position);
            }
        }

        void LateUpdate()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.LateUpdate)) return;
            foreach (CircleTrigger2D trigger in triggers)
            {
                trigger.InTrigger(transform.position);
            }
        }

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (CircleTrigger2D trigger in triggers)
            {
                if (trigger.DrawSettings.DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmos) continue;
                trigger.DrawTrigger(transform.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (CircleTrigger2D trigger in triggers)
            {
                if (trigger.DrawSettings.DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmosSelect) continue;
                trigger.DrawTrigger(transform.position);
            }
        }
#endif
        #endregion
    }
}
