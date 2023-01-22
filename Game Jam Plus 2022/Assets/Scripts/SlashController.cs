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
                    StatusController.LifeSystem life;
                    try
                    {
                        life = o.gameObject.GetComponent<StatusController.LifeSystem>();
                    }
                    catch
                    {
                        continue;
                    }
                    life.Damage(slashType.damage);

                    /*
                    if (o.CompareTag("Enemy"))
                    {
                        Enemy.AI.EnemyAI enemy = o.gameObject.GetComponent<Enemy.AI.EnemyAI>();
                        enemy.Damage(slashType.damage);
                    }*/
                }
                objects.AddRange(obj);
            }
        }
        public void SetSlash(Slash _newSlash, float _slashRotation)
        {
            slashType = _newSlash;
            col.SetLayerMask(slashType.activeLayerMasks);
            col.SetRotation(_slashRotation);
            Destroy(gameObject, slashType.LifeTime);
        }

        public void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}