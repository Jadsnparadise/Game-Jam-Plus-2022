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
        [SerializeField] bool hasAttackAnim;
        [TextArea(2, 3)] public string description;
        [SerializeField, Min(0)] float timeToAtack = 1;
        [SerializeField, Min(0)] protected float timeToUse = 1;
        float currentAtackTimer;
        protected float currentUseTimer;
        [SerializeField] Slash slashType;
        [SerializeField] GameObject slashGameObject;
        public bool stack;
        public List<Player.Inventory.Resources> craft = new();
        //public float playerDistance = 1.6f;
        public Vector3 itemOffset = new(1.6f, 0, 0);
        
        public virtual void ItemStart()
        {
            currentAtackTimer = timeToAtack;
            currentUseTimer = timeToUse;
        }
        public virtual void ItemUpdate()
        {
            currentAtackTimer += Time.deltaTime;
            currentUseTimer += Time.deltaTime;
        }
        public virtual void Atacking(Game.Player.Player _player, Player.Inventory.AimController _aim ,Vector3 _handPos, Quaternion _handRot)
        {
            if (currentAtackTimer < timeToAtack)
            {
                return;
            }
            if (hasAttackAnim)
            {
                _aim.AttackAnim();
            }
            
            SlashController s = Instantiate(slashGameObject, _handPos, _handRot).GetComponent<SlashController>();
            s.SetSlash(slashType, _handRot.eulerAngles.z);
            currentAtackTimer = 0;
        }

        public virtual void Using(Game.Player.Player _player, Player.Inventory.AimController _aim, Vector3 _handPos, Quaternion _handRot)
        {
            if (currentUseTimer < timeToUse)
            {
                return;
            }
            currentUseTimer = 0;
        }
    }
}