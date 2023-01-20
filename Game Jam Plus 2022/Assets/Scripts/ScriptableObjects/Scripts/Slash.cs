using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    [CreateAssetMenu(fileName = "NewSlash", menuName = "New Item/New Slash")]
    public class Slash : ScriptableObject
    {
        public float lifeTime;
        public LayerMask activeLayerMasks;
    }
}