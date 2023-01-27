using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Map
{
    public class SpawnCheck : MonoBehaviour
    {
        [SerializeField] CollisionSystem.Collision col;
        void Start()
        {
            if (col.InCollision(transform)) Destroy(gameObject);
            GetComponent<SpriteRenderer>().flipX = Random.Range(0, 100) <= 49;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}