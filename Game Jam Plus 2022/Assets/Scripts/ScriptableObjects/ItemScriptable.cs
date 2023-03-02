using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "New Item/New Generic Item", order = 1)]
    public class ItemScriptable : ScriptableObject
    {
        public string itemName;
        /*
        public Sprite itemSprite;
        public Sprite itemSpriteInHand;
        */
        public List<Sprite> mapSprites;
        public Sprite spriteInInventory;
        public Sprite spriteInHand;
        public Sprite spriteToDrop;
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
        public Vector3 scaleDrop = new(0.3f, 0.3f, 1);
        
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

        public Sprite CurrentSprite(SpriteType _sprite)
        {
            switch (_sprite)
            {
                case SpriteType.World:
                    if (mapSprites != null)
                    {
                        int i = Random.Range(0, mapSprites.Count);
                        return mapSprites[i];
                    }
                    else
                    {
                        return null;
                    }
                    
                case SpriteType.Inventory:
                    return spriteInInventory;
                case SpriteType.Hand:
                    return spriteInHand;
                case SpriteType.Drop:
                    return spriteToDrop;
            }
            return null;
        }

        public bool CompareSprite(Sprite _s)
        {
            return _s == spriteInHand || _s == spriteInInventory || mapSprites.Exists(x => x == _s);
        }
    }
    public enum SpriteType
    {
        World,
        Inventory,
        Hand,
        Drop
    }
}