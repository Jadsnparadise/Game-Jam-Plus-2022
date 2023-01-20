using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    public class SlashController : MonoBehaviour
    {
        Slash slashType;
        [SerializeField] CollisionSystem.Collision col;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void SetSlash(Slash _newSlash)
        {
            slashType = _newSlash;
            col.SetLayerMask(slashType.activeLayerMasks);
            Destroy(gameObject, slashType.lifeTime);
        }

        public void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}