using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Player
{
    using CollisionSystem;
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [Header("Player Attributes")]
        [SerializeField] System.Attribute lifeBar;
        [SerializeField] System.Attribute staminaBar;
        [SerializeField] System.Attribute hungrybar;
        [SerializeField] System.Attribute hapinessbar;

        [Header("Player Settings")]
        [SerializeField, Min(1)] float speed;
        [SerializeField, Min(1)] float godSeconds;
        [SerializeField, Min(1)] float knockbackForce;
        Vector2 moveDir;
        bool canTakeDamage;

        [Header("Colliders")]
        [SerializeField] Collision hitbox;

        //Components
        Rigidbody2D rig;
        Animator anim;
        AimController aim;
        SpriteRenderer spriteRenderer;
        void Start()
        {
            rig ??= GetComponent<Rigidbody2D>();
            anim ??= GetComponent<Animator>();
            aim ??= gameObject.GetComponentInChildren<AimController>();
            anim ??= GetComponent<Animator>();
            spriteRenderer ??= GetComponent<SpriteRenderer>();
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
        }

        void FixedUpdate()
        {
            rig.AddForce(moveDir * speed, ForceMode2D.Impulse);
        }

        void Move()
        {
            moveDir.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        void AnimationController()
        {
            anim.SetFloat("playerSpeed", moveDir.magnitude);
            anim.SetFloat("mousePosX", aim.lookingDir.x);
            anim.SetFloat("mousePosY", aim.lookingDir.y);
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