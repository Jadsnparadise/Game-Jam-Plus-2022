using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{
    using UnityEngine.UI;
    using UnityEngine.UIElements;

    public class ItemController : MonoBehaviour
    {

        [SerializeField] Player.Inventory.Resources item;

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
                        AddResource(i.CurrentResource());
                        Destroy(i.gameObject);
                    }
                }
            }
        }

        private void OnMouseEnter()
        {
            /*
            Debug.Log($"Passou em {item.item.itemName}");
            text.gameObject.SetActive(true);
            text.text = item.item.itemName;
            
            
            */
            System.Ui.TextMeshProController.Instance.SetManager(true, gameObject, item.item.itemName);
            spriteRenderer.color = overlayColor;
            mouseOn = true;
        }

        private void OnMouseExit()
        {
            System.Ui.TextMeshProController.Instance.SetManager(false);
            spriteRenderer.color = Color.white;
            mouseOn = false;
            /*
            text.gameObject.SetActive(false);
            
            //text.gameObject.SetActive(false);
            */
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
            spriteRenderer.sprite = _newItem.item.CurrentSprite(SpriteType.World);
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
            spriteRenderer.sprite = _newItem.item.CurrentSprite(SpriteType.World);
            if (!TryGetComponent(out BoxCollider2D _))
            {
                BoxCollider2D c = gameObject.AddComponent<BoxCollider2D>();
                c.isTrigger = true;
            }
            stack = true;
            //Destroy(gameObject, lifeTime);
        }

        public void AddResource(Player.Inventory.Resources _newResource)
        {
            item.quantity += _newResource.quantity;
        }

        public Player.Inventory.Resources CurrentResource()
        {
            return item;
        }

        public ItemScriptable CurrentItem()
        {
            return item.item;
        }

        public void SlpashDrop()
        {
            //StartCoroutine(fallDown());
        }

        /*IEnumerator fallDown()
        {
            float Delay = 1;
            float delay = Delay;
            float finalXPosition = Random.Range(-3f, 3);
            float zRange = Random.Range(1f, 3f);
            Vector3 initialPosition = transform.position;
            int Bounce = 4;
            int bounce = 1;
            float normalizedDelay;

            while (Bounce != bounce)
            {

                normalizedDelay = delay / (Delay / bounce);

                if (normalizedDelay <= 0)
                {

                    if (zRange > initialPosition.z)
                    {

                        zRange = Random.Range(-3f, -1f);

                    }
                    else
                    {

                        zRange = Random.Range(1f, 3f);

                    }

                    initialPosition = transform.position;
                    bounce++;
                    delay = (Delay / bounce);

                }

                normalizedDelay = delay / (Delay / bounce);

                transform.position = new Vector3(Mathf.Lerp(initialPosition.x, initialPosition.x + finalXPosition, 1 - ((delay + bounce)/Delay)), transform.position.y, transform.position.z);
                transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(initialPosition.z, initialPosition.z + zRange, 1 - normalizedDelay));


                yield return new WaitForSeconds(0.02f);
                delay -= 0.02f;
                
            }
        }*/

            public void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}