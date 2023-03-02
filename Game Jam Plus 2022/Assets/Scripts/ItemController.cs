using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Itens
{

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

        public bool hasDropAnim = true;

        [Header("Animation Idle")]
        public float m_idleSpeed = 0.2f;
        public LeanTweenType m_idleEase;

        [Header("Animation Drop")]
        public float m_dropSpeed = 0.2f;
        public LeanTweenType m_dropEase;

        void Start()
        {
        
            SetItem(item);
        }

        private void OnEnable() {


            Vector2 mouseInput = Input.mousePosition;
            float mouseX = mouseInput.x / Screen.width;
            float mouseY = mouseInput.y / Screen.height;

            int inverterX;
            if (mouseX > 0.5f) inverterX = 1;
            else inverterX = -1;

            int inverterY;
            if (mouseY > 0.5f) inverterY = 1;
            else inverterY = -1;

            LeanTween.move(gameObject, new Vector2(transform.position.x + Random.Range(0.2f, 0.4f) * inverterX, transform.position.y + Random.Range(0.1f, 0.2f) * inverterY), m_idleSpeed).setEase(m_idleEase).setOnComplete((() => {
                LeanTween.move(gameObject, new Vector2(transform.position.x, transform.position.y + 0.2f), m_idleSpeed).setEase(m_idleEase).setLoopPingPong();
            }));


        }

        private void OnDisable() {
            gameObject.LeanCancel();
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
            Debug.Log($"Passou em {item.item.itemName}");
            
            

            System.Ui.TextMeshProController.Instance.SetManager(true, gameObject, item.item.itemName, new Vector3(0,0.7f,0));
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
            transform.localScale = _newItem.item.scaleDrop;
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

            public void OnDrawGizmos()
        {
            col.DrawCollider(transform);
        }
    }
}