using System.Collections.Generic;
using UnityEngine;

namespace Trigger.System2D.Manager
{
    public class BoxTriggerManager2D : MonoBehaviour
    {
        [SerializeField] Core.UpdateMethod updateMethod = Core.UpdateMethod.FixedUpdate;
        [SerializeField] List<BoxTrigger2D> triggers = new List<BoxTrigger2D>() { new BoxTrigger2D() };

        void Update()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.Update)) return;
            foreach (BoxTrigger2D trigger in triggers)
            {
                trigger.InTrigger(transform.position);
            }
        }

        void FixedUpdate()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.FixedUpdate)) return;
            foreach (BoxTrigger2D trigger in triggers)
            {
                trigger.InTrigger(transform.position);
            }
        }

        void LateUpdate()
        {
            if (!updateMethod.Equals(Core.UpdateMethod.LateUpdate)) return;
            foreach (BoxTrigger2D trigger in triggers)
            {
                trigger.InTrigger(transform.position);
            }
        }

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (BoxTrigger2D trigger in triggers)
            {
                if (trigger.DrawSettings.DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmos) continue;
                trigger.DrawTrigger(transform.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (BoxTrigger2D trigger in triggers)
            {
                if (trigger.DrawSettings.DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmosSelect) continue;
                trigger.DrawTrigger(transform.position);
            }
        }
#endif
        #endregion
    }
}
