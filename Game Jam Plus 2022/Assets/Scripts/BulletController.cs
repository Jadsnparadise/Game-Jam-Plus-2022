using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    public class BulletController : MonoBehaviour
    {

        Bullets bulletType;
        [SerializeField] CollisionSystem.Collision col;

        Vector3 startPos;
        Vector3 targetPos;
        float progress;

        void Start()
        {
            //startPos = transform.position.WithAxis(Axis.Z, -1);
            //Debug.LogError("Teste");
        }

        // Update is called once per frame
        void Update()
        {
            /* OLD SYS
            transform.Translate(Vector3.right * Time.deltaTime * bulletType.speed);
            if (col.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    if (o.CompareTag("Enemy"))
                    {
                        Enemy.AI.EnemyAI enemy = o.gameObject.GetComponent<Enemy.AI.EnemyAI>();
                        enemy.Damage(bulletType.itemDamage);
                    }
                    Destroy(gameObject);
                }
            }
            
            progress += Time.deltaTime * bulletType.speed;
            transform.position = Vector3.Lerp(startPos, targetPos, progress);
            */
            transform.Translate(Vector3.right * Time.deltaTime * bulletType.speed);
            if (col.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    if (o.CompareTag("Enemy"))
                    {
                        Enemy.AI.EnemyAI enemy = o.gameObject.GetComponent<Enemy.AI.EnemyAI>();
                        enemy.Damage(bulletType.damage);
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

        public void SetTargetPos(Vector3 _newTargetPos)
        {
            targetPos = _newTargetPos.WithAxis(Axis.Z, -1);
        }

        public void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }

    public static class VectorExtension
    {
        public static Vector3 WithAxis(this Vector3 _vector, Axis axis, float value)
        {
            return new Vector3(axis == Axis.X ? value : _vector.x, axis == Axis.Y ? value : _vector.y, axis == Axis.Z ? value : _vector.z);
        }
    }

    public enum Axis 
    {
        X, Y, Z
    }
}