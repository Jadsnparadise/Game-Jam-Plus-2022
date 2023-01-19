using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    [CreateAssetMenu(fileName = "NewGun", menuName = "New Item/New Gun/New Auto Gun")]
    public class AutoGun : ItemScriptable
    {
        [Min(0)] public int currentAmmo;
        [SerializeField] GameObject bullet;
        [SerializeField, Min(0)] float fireRate;
        float currentFireRate;
        [SerializeField] Bullets bulletType;

        public override void ItemStart()
        {
            currentFireRate = fireRate;
        }
        public override void ItemUpdate()
        {
            currentFireRate += Time.deltaTime;
        }
        public override void Atacking(Vector3 _handPos, Quaternion _handRot)
        {
            if (currentFireRate >= fireRate)
            {
                Shot(_handPos, _handRot);
            }
        }

        void Shot(Vector3 _handPos, Quaternion _handRot)
        {
            if (currentAmmo <= 0)
            {
                return;
            }
            BulletController b = Instantiate(bullet, _handPos, _handRot).GetComponent<BulletController>();
            b.SetBullet(bulletType);
            currentAmmo -= 1;
            currentFireRate = 0;
        }
    }
}