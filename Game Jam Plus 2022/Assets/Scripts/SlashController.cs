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
            if (col.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    if (o.CompareTag("Enemy"))
                    {
                        Enemy.AI.EnemyAI enemy = o.gameObject.GetComponent<Enemy.AI.EnemyAI>();
                        enemy.Damage(slashType.damage);
                    }
                }
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