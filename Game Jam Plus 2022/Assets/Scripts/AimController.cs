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

            Debug.Log(inventory.currentSlot);

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

        public void AddItem(Itens.ItemScriptable _newItem)
        {
            Resources r = resources.Find(x => x.item == _newItem);
            if (_newItem.stack)
            {
                if (r.item != null)
                {
                    resources.Find(x => x.item == _newItem).quantity++;
                }
                else
                {
                    NewItem(_newItem);
                }
            }
            else
            {
                NewItem(_newItem);
            }
            InvUpdate();
        }

        void NewItem(Itens.ItemScriptable _newItem)
        {
            if (resources[currentSlot].item == null)
            {
                resources[currentSlot] = new(_newItem);
            }
            else
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    if (resources[i].item == null)
                    {
                        resources[i] = new(_newItem);
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
            for (int i = 0; i < slots.Count - 1; i++)
            {
                UnityEngine.UI.RawImage slot = slots[i].GetComponent<UnityEngine.UI.RawImage>();

                if (resources[i].item != null)
                {
                    Sprite spr = resources[i].item.itemSprite;
                    slot.texture = spr != null ? spr.texture : slot.texture;
                }
            }
        }

        public Itens.ItemScriptable CurrentItem()
        {
            return resources[currentSlot].item;
        }
    }
}