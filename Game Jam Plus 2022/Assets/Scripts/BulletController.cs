using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    public class BulletController : MonoBehaviour
    {

        Bullets bulletType;
        [SerializeField] CollisionSystem.Collision col;

        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * bulletType.speed);
            if (col.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    if (o.gameObject.TryGetComponent(out StatusController.LifeSystem life))
                    {
                        life.Damage(bulletType.damage);
                    }
                    Destroy(gameObject);
                }
            }
        }
 
        public void SetBullet(Bullets _newBullet)
        {
            bulletType = _newBullet;
            Destroy(gameObject, bulletType.lifeTime);
        }

        public void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}