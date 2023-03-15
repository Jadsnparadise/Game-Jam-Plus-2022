using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Game.Itens;

namespace Game.Player.Inventory
{
    
    public class AimController : MonoBehaviour
    {
        //[HideInInspector] public bool canAttack = true;
        [SerializeField] GameObject hand;
        [SerializeField] GameObject itenHand;
        //[SerializeField] List<HandSystem> hands;
        SpriteRenderer handSpriteRenderer;
        SpriteRenderer itemSpriteRenderer;
        [SerializeField] Animator handAnim;
        Player playerScript;

        public Vector2 lookingDir { get; private set; }
        [SerializeField] Itens.ItemScriptable currentItem;
        [SerializeField] Itens.ItemScriptable handItem;

        [SerializeField] Inventory inventory = new();
        [SerializeField] List<KeyCode> inventoryKeys = new();
        Vector3 savePos;

        //[SerializeField, Range(0, 1)] float knockbackForce;

        void Start()
        {
            handSpriteRenderer = hand.GetComponent<SpriteRenderer>();
            itemSpriteRenderer = itenHand.GetComponent<SpriteRenderer>();
            currentItem ??= handItem;
            handSpriteRenderer.sprite = currentItem.CurrentSprite(Itens.SpriteType.Hand);
            currentItem.ItemStart();
            inventory.InvStart();
            handAnim = GetComponentInChildren<Animator>();
            playerScript = GetComponentInParent<Player>();
            savePos = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            HandUpdate();
            InputUpdate();
            Aim();
            AnimUpdate();
        }

        void HandUpdate()
        {

            
            currentItem = inventory.CurrentItem();
            currentItem ??= handItem;
            if (currentItem.animInHand != null)
            {
                handAnim.runtimeAnimatorController = currentItem.animInHand;
            }
            else
            {
                //itemSpriteRenderer.sprite = currentItem.itemSpriteInHand != null ? currentItem.itemSpriteInHand : currentItem.itemSprite;
                Sprite s = currentItem.CurrentSprite(Itens.SpriteType.Hand) != null ? currentItem.CurrentSprite(Itens.SpriteType.Hand) : currentItem.CurrentSprite(Itens.SpriteType.Inventory);
                itemSpriteRenderer.sprite = s;
                handAnim.runtimeAnimatorController = handItem.animInHand;
            }
            if (handAnim.runtimeAnimatorController == handItem.animInHand)
            {
                hand.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            else
            {
                handSpriteRenderer.gameObject.transform.rotation = new();
            }
            itemSpriteRenderer.enabled = currentItem.CompareSprite(itemSpriteRenderer.sprite);
            hand.transform.localPosition = currentItem.itemOffset;
            handSpriteRenderer.sortingOrder = lookingDir.y < 0 ? 1 : -1;
            currentItem.ItemUpdate();
        }

        void InputUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                currentItem.Atacking(playerScript, this, hand.transform.position, transform.rotation);
            }
            if (Input.GetMouseButton(1))
            {
                currentItem.Using(playerScript, this, hand.transform.position, transform.rotation);
            }
            foreach (KeyCode k in inventoryKeys)
            {
                if (Input.GetKeyDown(k))
                {
                    inventory.ChangeSlot(inventoryKeys.FindIndex(x => x == k));
                }
            }
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            {
                inventory.AddSlot();
            }
            else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                inventory.DecreaseSlot();
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q))
            {
                DropAllInHand();
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                DropItem();
            }
        }

        void Aim()
        {
         
            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            lookingDir = (mousePos - screenPoint).normalized;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(lookingDir.y,lookingDir.x) * Mathf.Rad2Deg);
            handSpriteRenderer.flipY = handAnim.runtimeAnimatorController != handItem.animInHand ? mousePos.x < screenPoint.x : false;

        }

        public void Knockback(float _knockback, float _recoil)
        {
            playerScript.Knockback(_knockback, -lookingDir);
            StartCoroutine(KnockbackAnim(lookingDir, _recoil));
        }

        IEnumerator KnockbackAnim(Vector3 _lookingDir, float _recoil)
        {
            Vector3 dir = (-_lookingDir * _recoil) + savePos;
            transform.localPosition = dir;
            yield return new WaitForSeconds(0.07f);
            transform.localPosition = savePos;
        }

        void AnimUpdate()
        {
            if (handAnim.runtimeAnimatorController != null)
            {
                handAnim.SetFloat("MousePosX", lookingDir.x);
                handAnim.SetFloat("MousePosY", lookingDir.y);
                handAnim.SetBool("Moving", playerScript.Moving());
            }
        }

        public void AttackAnim()
        {
            if (handAnim.runtimeAnimatorController != null)
            {
                handAnim.SetTrigger("Attack");
            }
        }

        [ContextMenu("Inv Update")]
        public void InvUpdate()
        {
            inventory.InvUpdate();
        }

        public bool AddItem(Resources _newItem)
        {
            inventory.AddItem(_newItem, out bool added);
            return added;
        }

        public void UseItem()
        {
            if (currentItem == handItem)
            {
                return;
            }
            inventory.DropItem();
        }

        public void DropItem()
        {
            if (currentItem == handItem)
            {
                return;
            }
            Itens.ItemController i = Instantiate(inventory.dropGameObject, transform.position - new Vector3(0, 0, 0.1f), Quaternion.identity).GetComponent<Itens.ItemController>();
            i.SetItem(new(inventory.CurrentResource().item), 15, SpriteType.Drop);
            inventory.DropItem();
        }

        public void DropAllInHand()
        {
            if (currentItem == handItem)
            {
                return;
            }
            Itens.ItemController item = Instantiate(inventory.dropGameObject, transform.position - new Vector3(0, 0, 0.1f), Quaternion.identity).GetComponent<Itens.ItemController>();
            item.SetItem(inventory.CurrentResource(), 15, SpriteType.Drop);
            inventory.DropAllInHand();
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

        public Resources(Itens.ItemScriptable _newItem, int _quantity)
        {
            item = _newItem;
            quantity = _quantity;
        }
    }

    [Serializable]
    public class Inventory
    {
        [SerializeField] List<GameObject> slots = new();
        [SerializeField] GameObject overlay;
        public int currentSlot { get; private set; }
        [SerializeField] List<Resources> resources = new(8);
        public List<Resources> Resources { get { return resources; } set { Resources = value; } }
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
            overlay.transform.position = slots[currentSlot].transform.position;
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
            InvUpdate();
        }
        public void DropAllInHand()
        {
            resources[currentSlot] = new();
            InvUpdate();
        }

        public void AddItem(Resources _newItem, out bool added)
        {
            added = false;
            //Resources r = resources.Find(x => x.item == _newItem);
            bool hasItem = resources.Exists(x => x.item == _newItem.item);
            if (_newItem.item.stack)
            {
                if (hasItem)
                {
                    resources.Find(x => x.item == _newItem.item).quantity += _newItem.quantity;
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

        void NewItem(Resources _newItem, out bool added)
        {
            added = false;
            if (resources[currentSlot].item == null)
            {
                resources[currentSlot] = _newItem;
                added = true;
            }
            else
            {
                for (int i = 0; i < resources.Count; i++)
                {
                    if (resources[i].item == null)
                    {
                        resources[i] = _newItem;
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
                    Sprite spr = resources[i].item.CurrentSprite(Itens.SpriteType.Inventory);
                    slot.texture = spr != null ? spr.texture : slot.texture;
                }
                else
                {
                    slot.color = Color.clear;
                }
            }
        }

        public Resources CurrentResource()
        {
            return resources[currentSlot];
        }
        public Itens.ItemScriptable CurrentItem()
        {
            return resources[currentSlot].item;
        }
    }
}