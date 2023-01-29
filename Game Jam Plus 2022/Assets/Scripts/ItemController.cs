using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    using UnityEngine.UI;
    public class ItemController : MonoBehaviour
    {

        [SerializeField] ItemScriptable item;
        [SerializeField] TMPro.TextMeshProUGUI text;
        [SerializeField] Color overlayColor;
        SpriteRenderer spriteRenderer;
        public bool canPick { get; private set; }
        void Start()
        {
            SetItem(item);
        }

        void Update()
        {
            
        }

        private void OnMouseEnter()
        {
            /*
            Debug.Log($"Passou em {item.itemName}");
            text.gameObject.SetActive(true);
            text.text = item.itemName;
            */
            spriteRenderer.color = overlayColor;

        }

        private void OnMouseExit()
        {
            spriteRenderer.color = Color.white;
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

        public void SetItem(ItemScriptable _newItem)
        {
            item = _newItem;
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (item.itemSprite != null)
            {
                spriteRenderer.sprite = item.itemSprite;
            }
            if (!TryGetComponent(out BoxCollider2D _))
            {
                BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
                c.isTrigger = true;
            }
        }

        public ItemScriptable CurrentItem()
        {
            return item;
        }
    }
}