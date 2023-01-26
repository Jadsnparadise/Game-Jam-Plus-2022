using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    public class SlashController : MonoBehaviour
    {
        Slash slashType;
        [SerializeField] CollisionSystem.Collision col;
        [SerializeField] List<Collider2D> objects = new();
        void Start()
        {
            objects = new();
        }

        // Update is called once per frame
        void Update()
        {
            if (col.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    if (objects.Contains(o))
                    {
                        continue;
                    }
                    if (o.gameObject.TryGetComponent(out StatusController.LifeSystem life))
                    {
                        life.Damage(slashType.damage);
                    }
                }
                objects.AddRange(obj);
            }
        }
        public void SetSlash(Slash _newSlash, float _slashRotation)
        {
            slashType = _newSlash;
            col.SetLayerMask(slashType.activeLayerMasks);
            col.SetRotation(_slashRotation);
            GetComponent<SpriteRenderer>().enabled = _newSlash.rendering;
            Destroy(gameObject, slashType.LifeTime);
        }

        public void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}