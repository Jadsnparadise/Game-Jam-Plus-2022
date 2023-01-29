using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    using UnityEngine.UI;
    public class ItemController : MonoBehaviour
    {

        [SerializeField] Player.Inventory.Resources item;
        [SerializeField] TMPro.TextMeshProUGUI text;
        [SerializeField] Color overlayColor;
        SpriteRenderer spriteRenderer;
        [SerializeField] CollisionSystem.Collision col;
        [SerializeField, Min(0)] float lifeTime;
        [SerializeField] bool stack = true;
        public bool canPick { get; private set; }
        public bool mouseOn { get; private set; }
        void Start()
        {
            SetItem(item);
        }

        void Update()
        {
            if (!stack)
            {
                return;
            }
            if (col.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    ItemController i = o.GetComponent<ItemController>();
                    if (i.CurrentItem().stack && i.CurrentItem() == item.item && i.gameObject != gameObject)
                    {
                        AddResource();
                        Destroy(i.gameObject);
                    }
                }
            }
        }

        private void OnMouseEnter()
        {
            Debug.Log($"Passou em {item.item.itemName}");
            text.gameObject.SetActive(true);
            text.text = item.item.itemName;
            
            spriteRenderer.color = overlayColor;
            mouseOn = true;
        }

        private void OnMouseExit()
        {
            text.gameObject.SetActive(false);
            spriteRenderer.color = Color.white;
            mouseOn = false;
            //text.gameObject.SetActive(false);
        }

        private void OnMouseDown()
        {
            canPick = true;
        }

        private void OnMouseUp()
        {
            canPick = false;
        }

        void SetItem(Player.Inventory.Resources _newItem)
        {
            item = _newItem;
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (item.item.itemSprite != null)
            {
                spriteRenderer.sprite = item.item.itemSprite;
            }
            if (!TryGetComponent(out BoxCollider2D _))
            {
                BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
                c.isTrigger = true;
            }
        }

        public void SetItem(Player.Inventory.Resources _newItem, float _lifeTime)
        {
            item = _newItem;
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (item.item.itemSprite != null)
            {
                spriteRenderer.sprite = item.item.itemSprite;
            }
            if (!TryGetComponent(out BoxCollider2D _))
            {
                BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
                c.isTrigger = true;
            }
            stack = true;
            //Destroy(gameObject, lifeTime);
        }

        public void AddResource()
        {
            item.quantity++;
        }

        public Player.Inventory.Resources CurrentResource()
        {
            return item;
        }

        public ItemScriptable CurrentItem()
        {
            return item.item;
        }

        public void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}