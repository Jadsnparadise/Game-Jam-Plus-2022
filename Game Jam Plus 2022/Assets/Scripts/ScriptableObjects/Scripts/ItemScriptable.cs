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
        public Sprite itemSpriteInHand;
        public RuntimeAnimatorController animInHand;
        [TextArea(2, 3)] public string description;
        [SerializeField, Min(0)] float timeToAtack = 1;
        float currentAtackTimer;
        [SerializeField] Slash slashType;
        [SerializeField] GameObject slashGameObject;
        public bool stack;

        public virtual void ItemStart()
        {
            currentAtackTimer = timeToAtack;
        }
        public virtual void ItemUpdate()
        {
            currentAtackTimer += Time.deltaTime;
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