using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    public class SpawnCheck : MonoBehaviour
    {
        [SerializeField] Vector2 sizeRange = Vector2.one;
        [SerializeField] CollisionSystem.Collision col;
        void Start()
        {
            if (col.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    if (o.gameObject != this.gameObject)
                    {
                        Destroy(gameObject);
                    }
                }
            }
            transform.localScale *= Random.Range(sizeRange.x, sizeRange.y);
            GetComponent<SpriteRenderer>().flipX = Random.Range(0, 100) <= 49;
        }

        // Update is called once per frame
        void Update()
        {
            Destroy(this);
        }

        private void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}