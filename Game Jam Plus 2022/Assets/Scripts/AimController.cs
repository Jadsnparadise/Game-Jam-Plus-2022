using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Game.Player.Inventory
{
    
    public class AimController : MonoBehaviour
    {
        [SerializeField] GameObject hand;
        SpriteRenderer handSpriteRenderer;

        public Vector2 lookingDir { get; private set; }
        [SerializeField] Itens.ItemScriptable currentItem;
        [SerializeField] Itens.ItemScriptable handItem;

        [SerializeField] Inventory inventory = new();
        [SerializeField] List<KeyCode> inventoryKeys = new();

        void Start()
        {
            handSpriteRenderer = hand.GetComponent<SpriteRenderer>();
            currentItem ??= handItem;
            handSpriteRenderer.sprite = currentItem.itemSprite;
            currentItem.ItemStart();
            inventory.InvStart();
        }

        // Update is called once per frame
        void Update()
        {
            currentItem = inventory.CurrentItem();
            currentItem ??= handItem;
            handSpriteRenderer.sprite = currentItem.itemSprite;
            currentItem.ItemUpdate();

            //Debug.Log(inventory.currentSlot);

            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                inventory.AddSlot();
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                inventory.DecreaseSlot();
            }

            Aim();

            if (Input.GetButton("Fire1"))
            {
                currentItem.Atacking(hand.transform.position, hand.transform.rotation);
            }

            foreach (KeyCode k in inventoryKeys)
            {
                if (Input.GetKeyDown(k))
                {
                    inventory.ChangeSlot(inventoryKeys.FindIndex(x => x == k));
                }
            }
        }

        void Aim()
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            lookingDir = new(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(lookingDir.y, lookingDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            handSpriteRenderer.flipY = mousePos.x < screenPoint.x;
        }
        [ContextMenu("Inv Update")]
        public void InvUpdate()
        {
            inventory.InvUpdate();
        }

        public bool AddItem(Itens.ItemScriptable _newItem)
        {
            inventory.AddItem(_newItem, out bool added);
            return added;
        }

        public void DropItem()
        {
            if (currentItem == handItem)
            {
                return;
            }
            Itens.ItemController i = Instantiate(inventory.dropGameObject, transform.position, transform.rotation).GetComponent<Itens.ItemController>();
            i.SetItem(inventory.CurrentItem());
            inventory.DropItem();

        }
    }


    [Serializable]
    public class Resources
    {
        public Itens.ItemScriptable item;
        public int quantity;

        public Resources()
        {
            item = null;
            quantity = 0;
        }

        public Resources(Itens.ItemScriptable _newItem)
        {
            item = _newItem;
            quantity = 1;
        }
    }

    [Serializable]
    public class Inventory
    {
        [SerializeField] List<GameObject> slots = new();
        public int currentSlot { get; private set; }
        [SerializeField] List<Resources> resources = new(8);
        public GameObject dropGameObject;

        public void ChangeSlot(int _newValue)
        {
            if (_newValue < 0)
            {
                _newValue = slots.Count - 1;
            }
            else if (_newValue > slots.Count - 1)
            {
                _newValue = 0;
            }

            currentSlot = _newValue;
        }

        public void AddSlot()
        {
            ChangeSlot(currentSlot + 1);
        }

        public void DecreaseSlot()
        {
            ChangeSlot(currentSlot - 1);
        }

        public void DropItem()
        {
            if (resources[currentSlot].quantity > 1)
            {
                resources[currentSlot].quantity--;
            }
            else
            {
                resources[currentSlot] = new();
            }
             //= new Resources();
        }

        public void AddItem(Itens.ItemScriptable _newItem, out bool added)
        {
            added = false;
            //Resources r = resources.Find(x => x.item == _newItem);
            bool hasItem = resources.Exists(x => x.item == _newItem);
            if (_newItem.stack)
            {
                if (hasItem)
                {
                    resources.Find(x => x.item == _newItem).quantity++;
                    added = true;
                }
                else
                {
                    NewItem(_newItem, out added);
                }
            }
            else
            {
                NewItem(_newItem, out added);
            }
            InvUpdate();
        }

        void NewItem(Itens.ItemScriptable _newItem, out bool added)
        {
            added = false;
            if (resources[currentSlot].item == null)
            {
                resources[currentSlot] = new(_newItem);
                added = true;
            }
            else
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    if (resources[i].item == null)
                    {
                        resources[i] = new(_newItem);
                        added = true;
                        break;
                    }
                }
            }
        }

        public void InvStart()
        {
            while (resources.Count < 8)
            {
                resources.Add(new Resources());
            }
            InvUpdate();
        }

        public void InvUpdate()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                UnityEngine.UI.RawImage slot = slots[i].GetComponent<UnityEngine.UI.RawImage>();

                if (resources[i].item != null)
                {
                    slot.color = Color.white;
                    Sprite spr = resources[i].item.itemSprite;
                    slot.texture = spr != null ? spr.texture : slot.texture;
                }
                else
                {
                    slot.color = Color.clear;
                }
            }
        }

        public Itens.ItemScriptable CurrentItem()
        {
            return resources[currentSlot].item;
        }
    }
}