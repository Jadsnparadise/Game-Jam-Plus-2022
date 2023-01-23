using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Player
{
    using CollisionSystem;
    using Unity.VisualScripting;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Player Attributes")]
        [SerializeField] System.Attribute lifeBar;
        public System.Attribute LifeBar { get { return lifeBar; } private set { LifeBar = value; } }

        [SerializeField] System.Attribute staminaBar;
        public System.Attribute StaminaBar { get { return staminaBar; } private set { StaminaBar = value; } }

        [SerializeField] System.Attribute waterBar;
        public System.Attribute WaterBar { get { return waterBar; } private set { WaterBar = value; } }

        [SerializeField] System.Attribute hungrybar;
        public System.Attribute Hungrybar { get { return hungrybar; } private set { Hungrybar = value; } }

        [SerializeField] System.Attribute hapinessbar;
        public System.Attribute Hapinessbar { get { return hapinessbar; } private set { Hapinessbar = value; } }

        [Header("Player Settings")]
        [SerializeField, Min(1)] float speed;
        [SerializeField, Min(1)] float speedRun;
        [SerializeField, Min(1)] float godSeconds;
        [SerializeField, Min(1)] float knockbackForce;
        Vector2 moveDir;
        bool canTakeDamage;
        bool isRunning;

        bool isStoned;
        public bool IsStoned { get { return isStoned; } private set { IsStoned = value; } }

        bool isDrunk;
        public bool IsDrunk { get { return isDrunk; } private set { IsDrunk = value; } }

        bool isPoisoned;
        public bool IsPoisoned { get { return isPoisoned; } private set { IsPoisoned = value; } }

        bool isCold;
        public bool IsCold { get { return isCold; } private set { IsCold = value; } }

        bool isHot;
        public bool IsHot { get { return isHot; } private set { IsHot = value; } }


        [Header("Colliders")]
        [SerializeField] Collision hitbox;
        [SerializeField] Collision itemCollider;

        //Components
        Rigidbody2D rig;
        Animator anim;
        Inventory.AimController aim;
        SpriteRenderer spriteRenderer;

        GameObject statusController;
        Game.StatusController.StatusController statusPlayerController;

        bool gotInZero = false;

        void Start()
        {
            rig ??= GetComponent<Rigidbody2D>();
            anim ??= GetComponent<Animator>();
            aim ??= gameObject.GetComponentInChildren<Inventory.AimController>();
            anim ??= GetComponent<Animator>();
            spriteRenderer ??= GetComponent<SpriteRenderer>();
            statusController = GameObject.Find("Status Controller");
            statusPlayerController = statusController.GetComponent<StatusController.StatusController>();
            canTakeDamage = true;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            AnimationController();

            //Debug.Log($"Player: {life.CurrentValue} / {life.MaxValue}");

            if (hitbox.InCollision(transform, out Collider2D[] objects))
            {
                foreach (Collider2D c in objects)
                {
                    Enemy.AI.EnemyAI a = c.gameObject.GetComponent<Enemy.AI.EnemyAI>();
                    Damage(a.EnemyDamage);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                PickItem();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                aim.DropItem();
                aim.InvUpdate();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }

        void FixedUpdate()
        {
            float currentSpeed = isRunning ? speedRun : speed;
            rig.AddForce(moveDir * currentSpeed, ForceMode2D.Impulse);
        }

        void PickItem()
        {
            if (itemCollider.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    Itens.ItemScriptable _item = o.GetComponent<Itens.ItemController>().CurrentItem();
                    if (aim.AddItem(_item))
                    {
                        Destroy(o.gameObject);
                    }
                }
            }
        }

        void Move()
        {
            if (!gotInZero)
            {
                isRunning = Input.GetKey(KeyCode.LeftShift);
            }
            moveDir.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (isRunning && staminaBar.CurrentValue > staminaBar.MinValue) {
                statusPlayerController.StaminaDecrease();
            }
            else
            {
                if(staminaBar.CurrentValue == staminaBar.MinValue)
                {
                    gotInZero = true;
                    isRunning = false;
                }
                else if (staminaBar.CurrentValue > staminaBar.MaxValue/2)
                {
                    gotInZero = false;
                }
                statusPlayerController.StaminaIncrease();
            }
        }

        void AnimationController()
        {
            anim.SetFloat("playerSpeed", moveDir.x);
            anim.SetFloat("mousePosX", aim.lookingDir.x);
            anim.SetFloat("mousePosY", aim.lookingDir.y);
            anim.SetBool("walking", moveDir.magnitude != 0);
            //spriteRenderer.flipX = aim.lookingDir.x < 0;
        }
        void Death()
        {
            Debug.LogWarning("MORREU BURRO RUIM LIXO HORROROSO");
        }

        void Damage(int _damage)
        {
            if (!canTakeDamage)
            {
                return;
            }
            lifeBar.DecreaseValue(_damage);
            if (lifeBar.CurrentValue <= lifeBar.MinValue)
            {
                Death();
            }
            Vector2 knockbackDir = new(UnityEngine.Random.Range(-knockbackForce, knockbackForce), UnityEngine.Random.Range(-knockbackForce, knockbackForce));
            while (knockbackDir.magnitude < knockbackForce/3)
            {
                knockbackDir = new(UnityEngine.Random.Range(-knockbackForce, knockbackForce), UnityEngine.Random.Range(-knockbackForce, knockbackForce));
            }
            rig.AddForce(Vector2.one * knockbackDir, ForceMode2D.Impulse);
            StartCoroutine(DmageTaken());
        }

        IEnumerator DmageTaken()
        {
            canTakeDamage = false;
            for (int i = 0; i < godSeconds; i++)
            {
                spriteRenderer.color = Color.clear;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
            canTakeDamage = true;
        }

        private void OnDrawGizmos()
        {
            hitbox.DrawCollider(transform);
            itemCollider.DrawCollider(transform);
        }
    }
}

namespace Game.System
{
    [Serializable]
    public class Attribute
    {
        [SerializeField] string attributeName;
        [SerializeField] int currentValue = 100;
        [SerializeField] int startValue = 100;
        [SerializeField] int maxValue = 100;
        [SerializeField] int minValue = 0;
        public string AttributeName { get { return attributeName; } private set { AttributeName = value; } }
        public int CurrentValue { get { return currentValue; } private set { CurrentValue = value; } }
        public int StartValue { get { return startValue; } private set { StartValue = value; } }
        public int MaxValue { get { return maxValue; } private set { MaxValue = value; } }
        public int MinValue { get { return minValue; } private set { MinValue = value; } }
        public void SetValue()
        {
            SetValue(startValue);
        }
        public void SetValue(int _newValue)
        {
            currentValue = _newValue;
        }
        public void AddValue(int _addValue)
        {
            currentValue += _addValue;
            currentValue = currentValue > maxValue ? maxValue : currentValue;
        }
        public void DecreaseValue(int _decreaseValue)
        {
            currentValue -= _decreaseValue;
            currentValue = currentValue < minValue ? minValue : currentValue;
        }
    }
}