using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "New Item/New Generic Item", order = 1)]
    public class ItemScriptable : ScriptableObject
    {
        public string itemName;
        public Sprite itemSprite;
        [TextArea(2, 3)] public string description;
        [Min(1)] public int itemDamage = 1;
        [SerializeField, Min(0)] float timeToAtack = 1;
        float currentAtackTimer;
        [SerializeField] Slash slashType;
        [SerializeField] GameObject slashGameObject;
        [SerializeField] bool stack;

        public virtual void ItemStart()
        {
            currentAtackTimer = timeToAtack;
            Debug.Log($"{itemName} Start");
        }
        public virtual void ItemUpdate()
        {
            currentAtackTimer += Time.deltaTime;
            Debug.Log($"{itemName} Update");
        }
        public virtual void Atacking(Vector3 _handPos, Quaternion _handRot)
        {
            if (currentAtackTimer < timeToAtack)
            {
                return;
            }
            SlashController s = Instantiate(slashGameObject, _handPos, _handRot).GetComponent<SlashController>();
            s.SetSlash(slashType, _handRot.eulerAngles.z);
            currentAtackTimer = 0;
        }

        public virtual void Using()
        {
            Debug.Log($"Using {itemName}");
        }
    }
}