using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    [CreateAssetMenu(fileName = "NewSlash", menuName = "New Item/New Slash")]
    public class Slash : ScriptableObject
    {
        float lifeTime = 0.3f;
        public float LifeTime { get { return lifeTime; } private set { LifeTime = value; } }
        public LayerMask activeLayerMasks;
    }
}