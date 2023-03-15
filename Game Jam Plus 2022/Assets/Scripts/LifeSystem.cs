using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using Game.Itens;

namespace Game.StatusController
{
    public class LifeSystem : MonoBehaviour
    {
        [SerializeField] System.Attribute life;
        [SerializeField] List<Drop> drop;
        [SerializeField] GameObject dropGameObject;
        [SerializeField] bool anim = false;
        [SerializeField] List<LifeAnim> lifeAnim;
        SpriteRenderer spriteRenderer;

        void Start()
        {
            spriteRenderer ??= GetComponent<SpriteRenderer>();
            SetSprite();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Damage(int _damage)
        {
            life.DecreaseValue(_damage);
            SetSprite();
            if (life.CurrentValue <= life.MinValue)
            {
                Death();
            }
        }

        void SetSprite()
        {
            if (anim)
            {
                spriteRenderer.sprite = lifeAnim.Find(x => x.InInterval(life.CurrentValue)).Sprite;
            }
        }

        void Death()
        {
            List<Game.Player.Inventory.Resources> currentDrop = new();
            foreach (Drop d in drop)
            {
                currentDrop.AddRange(d.GetDrop());
            }
            foreach (Player.Inventory.Resources d in currentDrop)
            {
                Drop(d);
            }
            Destroy(gameObject);
        }

        void Drop(Player.Inventory.Resources _itemDrop)
        {
            Itens.ItemController i = Instantiate(dropGameObject, transform.position, transform.rotation).GetComponent<Itens.ItemController>();
            i.SetItem(_itemDrop, 15, SpriteType.Drop);
            i.SlpashDrop();
            
        }
    }

    [Serializable]
    public enum DropType
    {
        Null, RandomDrop, AllDrop
    }

    [Serializable]
    public struct Drop
    {
        [SerializeField] Itens.ItemScriptable itemDrop;
        [SerializeField, Range(0, 100)]  float chanceToDrop;
        [SerializeField] DropType dropType;
        [SerializeField, Min(1)] Vector2Int minMaxItensDrop;

        public List<Player.Inventory.Resources> GetDrop()
        {
            List<Player.Inventory.Resources> currentdrop = new();
            if (UnityEngine.Random.Range(0, 100) >= chanceToDrop)
            {
                return currentdrop;
            }
            switch (dropType)
            {
                case DropType.AllDrop:
                    currentdrop.Add(new(itemDrop, minMaxItensDrop.y + 1));
                    break;
                case DropType.RandomDrop:
                    currentdrop.Add(new(itemDrop, UnityEngine.Random.Range(minMaxItensDrop.x, minMaxItensDrop.y + 1)));
                    break;
            }
            return currentdrop;
        }
    }

    [Serializable]
    public struct LifeAnim
    {
        [SerializeField] Vector2 spriteInterval;
        [SerializeField] Sprite sprite;
        public Sprite Sprite { get { return sprite; } private set { Sprite = value; } }

        public bool InInterval(float _value)
        {
            return _value > spriteInterval.x && _value <= spriteInterval.y;
        }
    }
}