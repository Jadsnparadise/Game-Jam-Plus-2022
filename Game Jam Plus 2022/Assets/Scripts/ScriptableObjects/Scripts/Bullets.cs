using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    [CreateAssetMenu(fileName = "NewBullet", menuName = "New Item/New Bullet")]
    public class Bullets : ItemScriptable
    {
        public float speed;
        public float lifeTime;
    }
}