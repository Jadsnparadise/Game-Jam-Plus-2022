using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Player
{
    using CollisionSystem;
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMove : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] Attribute life;
        [SerializeField] Attribute stamina;
        [SerializeField, Min(1)] float speed;
        Vector2 moveDir;

        [Header("Colliders")]
        [SerializeField] Collision hitbox;

        //Components
        Rigidbody2D rig;
        Animator anim;
        AimController aim;
        void Start()
        {
            rig ??= GetComponent<Rigidbody2D>();
            anim ??= GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            AnimationController();
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

        }
        void Death()
        {

        }

        void Damage(int _damage)
        {
            life.DecreaseValue(_damage);
            if (life.CurrentValue <= life.MinValue)
            {
                Death();
            }
        }

        private void OnDrawGizmos()
        {
            hitbox.DrawCollider(transform);
        }
    }

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