using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


namespace Game.Itens
{
    [CreateAssetMenu(fileName = "NewGun", menuName = "New Item/New Gun/New Repeat Gun")]
    public class RepeatGun : ItemScriptable
    {
        [Min(0)] public int currentAmmo;
        [SerializeField] GameObject bullet;
        [SerializeField, Min(0)] float fireRate;
        float currentFireRate;
        [SerializeField] Bullets bulletType;
        bool shot = false;
        [SerializeField] Vector2 randomAccuracy;
        [SerializeField] Vector2Int randomBullet;
        [SerializeField] GameObject flash;
        public float knockbackForce;
        [SerializeField] float recoil;
        [SerializeField, Min(0)] float camShakeTime;
        [SerializeField, Range(0, 1)] float camShakeMagnitude;     

        public override void ItemStart()
        {
            currentFireRate = fireRate;
            shot = false;
        }
        public override void ItemUpdate()
        {
            currentFireRate += Time.deltaTime;

            if (shot && !Input.GetButton("Fire1") && currentFireRate >= fireRate)
            {
                shot = false;
            }            
        }
        public override void Atacking(Game.Player.Player _player, Player.Inventory.AimController _aim, Vector3 _handPos, Quaternion _handRot)
        {
            if (!shot)
            {
                Shot(_player, _aim, _handPos, _handRot);
                shot = true;
                PlayAttackSound(attackSound);
            }
        }

        void Shot(Game.Player.Player _player, Player.Inventory.AimController _aim, Vector3 _handPos, Quaternion _handRot)
        {
            if (currentAmmo <= 0)
            {
                return;
            }
            for (int i = 0; i <= Random.Range(randomBullet.x, randomBullet.y); i++)
            {
                float r = Random.Range(randomAccuracy.x, randomAccuracy.y);
                Quaternion rot = Quaternion.Euler(1, 1, r);
                
                BulletController b = Instantiate(bullet, _handPos, _handRot * rot).GetComponent<BulletController>();
                b.SetBullet(bulletType);
            }
            _aim.Knockback(knockbackForce, recoil);
            Destroy(Instantiate(flash, _handPos, _handRot), 0.8f);
            currentAmmo -= 1;
            currentFireRate = 0;
            _player.CamShake(camShakeTime, camShakeMagnitude);
            
        }
    }
}