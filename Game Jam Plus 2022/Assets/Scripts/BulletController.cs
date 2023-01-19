using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    public class BulletController : MonoBehaviour
    {
        Bullets bulletType;
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * bulletType.speed);
        }

        public void SetBullet(Bullets _newBullet)
        {
            bulletType = _newBullet;
            Destroy(gameObject, bulletType.lifeTime);
        }
    }
}