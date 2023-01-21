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
        }

        // Update is called once per frame
        void Update()
        {
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
    }

    [Serializable]
    public class Inventory
    {
        [SerializeField] List<GameObject> slots = new();
        public int currentSlot { get; private set; }
        [SerializeField] List<Itens.Resources> resources = new();

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

        public Itens.ItemScriptable CurrentItem()
        {
            return resources[currentSlot].item;
        }
    }
}