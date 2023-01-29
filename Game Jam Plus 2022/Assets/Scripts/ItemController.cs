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

        void Start()
        {
            
        }

        void Update()
        {
            
        }

        private void OnMouseEnter()
        {
            Debug.Log($"Passou em {item.itemName}");
            text.gameObject.SetActive(true);
            text.text = item.itemName;
        }

        private void OnMouseExit()
        {
            text.gameObject.SetActive(false);
        }

        public void SetItem(ItemScriptable _newItem)
        {
            item = _newItem;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (item.itemSprite != null)
            {
                spriteRenderer.sprite = item.itemSprite;
            }

            BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
            c.isTrigger = true;
        }

        public ItemScriptable CurrentItem()
        {
            return item;
        }
    }
}