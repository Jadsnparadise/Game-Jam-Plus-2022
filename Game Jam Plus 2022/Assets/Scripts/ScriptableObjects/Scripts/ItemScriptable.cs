using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    public abstract class ItemScriptable : ScriptableObject
    {
        public string itemName;
        public Sprite itemSprite;
        [TextArea(1, 3)] public string description;
        [SerializeField, Min(1)] int itemDamage;
        public virtual void ItemStart()
        {
            Debug.Log($"{itemName} Start");
        }
        public virtual void ItemUpdate()
        {
            Debug.Log($"{itemName} Update");
        }
        public virtual void Atacking(Vector3 _handPos, Quaternion _handRot)
        {
            Debug.Log($"Atacking with {itemName}");
        }

        public virtual void Using()
        {
            Debug.Log($"Using {itemName}");
        }

        
    }

    public class CraftableItens : ItemScriptable
    {
        public List<ItemScriptable> craftingResources;
    }
}