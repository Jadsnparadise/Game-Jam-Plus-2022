using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    public class ItemController : MonoBehaviour
    {
        [SerializeField] ItemScriptable item;

        void Start()
        {
            
        }

        void Update()
        {

        }
        public void SetItem(ItemScriptable _newItem)
        {
            item = _newItem;
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (item.itemSprite != null)
            {
                spriteRenderer.sprite = item.itemSprite;
            }
            
        }

        public ItemScriptable CurrentItem()
        {
            return item;
        }
    }
}