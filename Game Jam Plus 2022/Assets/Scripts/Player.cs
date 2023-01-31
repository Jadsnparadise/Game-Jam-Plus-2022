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

        [SerializeField] System.Attribute hungryBar;
        public System.Attribute HungryBar { get { return hungryBar; } private set { HungryBar = value; } }

        [SerializeField] System.Attribute happinessBar;
        public System.Attribute HappinessBar { get { return happinessBar; } private set { HappinessBar = value; } }

        [Header("Player Settings")]
        [SerializeField, Min(1)] float speed;
        [SerializeField, Min(1)] float speedRun;
        [SerializeField, Min(1)] float godSeconds;
        //[SerializeField, Min(1)] float knockbackForce;
        Vector2 moveDir;
        bool canTakeDamage;
        public bool isRunning;
        public bool isMoving;

        [SerializeField] bool isStoned;
        public bool IsStoned { get { return isStoned; } private set { IsStoned = value; } }

        [SerializeField] bool isDrunk;
        public bool IsDrunk { get { return isDrunk; } private set { IsDrunk = value; } }

        [SerializeField] bool isCold;
        public bool IsCold { get { return isCold; } private set { IsCold = value; } }

        [SerializeField] bool isHot;
        public bool IsHot { get { return isHot; } private set { IsHot = value; } }


        [Header("Colliders")]
        [SerializeField] Collision hitbox;
        [SerializeField] Collision itemCollider;

        //Components
        Rigidbody2D rig;
        Animator anim;
        Inventory.AimController aim;
        SpriteRenderer spriteRenderer;
        Game.System.Cam.CameraScript cam;

        StatusController.StatusController statusPlayerController;

        bool gotInZero = false;

        void Start()
        {
            rig ??= GetComponent<Rigidbody2D>();
            anim ??= GetComponent<Animator>();
            aim ??= gameObject.GetComponentInChildren<Inventory.AimController>();
            anim ??= GetComponent<Animator>();
            spriteRenderer ??= GetComponent<SpriteRenderer>();
            statusPlayerController = GameObject.Find("Status Controller").GetComponent<StatusController.StatusController>();
            cam = GameObject.FindGameObjectWithTag("Cinemachine").GetComponent<System.Cam.CameraScript>();
            canTakeDamage = true;
        }

        [ContextMenu("Bebeu")]
        public void Beber()
        {
            isDrunk = true;
            statusPlayerController.UiAddCondition(statusPlayerController.conditions.Find(x => x.effect == StatusController.Effect.Drunk));
        }
        
        [ContextMenu("Chapou")]
        public void Chapou()
        {
            isStoned = true;
            statusPlayerController.UiAddCondition(statusPlayerController.conditions.Find(x => x.effect == StatusController.Effect.Stoned));
        }
        [ContextMenu("Hot")]
        public void Hot()
        {
            isHot = true;
            statusPlayerController.UiAddCondition(statusPlayerController.conditions.Find(x => x.effect == StatusController.Effect.Hot));
        }
        
        [ContextMenu("Cold")]
        public void Cold()
        {
            isCold = true;
            statusPlayerController.UiAddCondition(statusPlayerController.conditions.Find(x => x.effect == StatusController.Effect.Cold));
        }
        [ContextMenu("Não Bebeu")]
        public void DesBeber()
        {
            isDrunk = false;
            statusPlayerController.UiRemoveCondition(statusPlayerController.conditions.Find(x => x.effect == StatusController.Effect.Drunk));
        }
        
        [ContextMenu("Não Chapou")]
        public void DesChapou()
        {
            isStoned = false;
            statusPlayerController.UiRemoveCondition(statusPlayerController.conditions.Find(x => x.effect == StatusController.Effect.Stoned));
        }
        [ContextMenu("Não Hot")]
        public void DesHot()
        {
            isHot = false;
            statusPlayerController.UiRemoveCondition(statusPlayerController.conditions.Find(x => x.effect == StatusController.Effect.Hot));
        }
        
        [ContextMenu("Não Cold")]
        public void DesCold()
        {
            isCold = false;
            statusPlayerController.UiRemoveCondition(statusPlayerController.conditions.Find(x => x.effect == StatusController.Effect.Cold));
        }


        void Update()
        {
            Move();
            AnimationController();

            /*
             * TIRAR ISSO AQUI E COLOCAR NO ENEMYIA SCRIPT ---------------------------->
            */
            if (hitbox.InCollision(transform, out Collider2D[] objects))
            {
                foreach (Collider2D c in objects)
                {
                    Enemy.AI.EnemyAI a = c.gameObject.GetComponent<Enemy.AI.EnemyAI>();
                    Damage(a.EnemyDamage);
                }
            }//<----------------------------
            PickItem();
            Inputs();
        }

        void FixedUpdate()
        {
            //Vector2 currentMoveDir = isDrunk ? -moveDir : moveDir;
            float currentSpeed = isRunning ? speedRun : speed;
            rig.AddForce(moveDir * currentSpeed * (isDrunk ? -1 : 1) * (isCold ? 0.5f : 1f), ForceMode2D.Impulse);
        }

        void PickItem()
        {
            if (itemCollider.InCollision(transform, out Collider2D[] obj))
            {
                foreach (Collider2D o in obj)
                {
                    Itens.ItemController _item = o.GetComponent<Itens.ItemController>();
                    //aim.canAttack = _item.mouseOn;
                    if (!_item.canPick)
                    {
                        continue;
                    }
                    if (aim.AddItem(_item.CurrentResource()))
                    {
                        System.Ui.TextMeshProController.Instance.SetManager(false);
                        Destroy(o.gameObject);
                    }
                }
            }
        }

        void Move()
        {
            isMoving = rig.velocity.normalized.magnitude != 0 && Moving();
            if (isMoving)
            {
                //statusPlayerController.StaminaDecrease(1);
                statusPlayerController.canRegenStamina = false;
            }
            if (!gotInZero)
            {
                isRunning = Input.GetKey(KeyCode.LeftShift) && (LookingForwardX() || LookingForwardY()) && !Input.GetMouseButton(0);
                statusPlayerController.canRegenStamina = false;
            }
            if (isRunning && staminaBar.CurrentValue > staminaBar.MinValue && isMoving) {
                //statusPlayerController.StaminaDecrease(2);
                statusPlayerController.canRegenStamina = false;
            }
            if(!isMoving && !isRunning)
            {
                statusPlayerController.canRegenStamina = true;
            }
            else
            {
                
                if (staminaBar.CurrentValue == staminaBar.MinValue)
                {
                    gotInZero = true;
                    isRunning = false;
                    
                }
                else if (staminaBar.CurrentValue > staminaBar.MaxValue/2)
                {
                    gotInZero = false;
                }
            }
        }

        bool LookingForwardX()
        {
            return (moveDir.x > 0 && aim.lookingDir.normalized.x > 0) || (moveDir.x < 0 && aim.lookingDir.normalized.x < 0);
        }

        bool LookingForwardY()
        {
            return (moveDir.y > 0 && aim.lookingDir.normalized.y > 0) || (moveDir.y < 0 && aim.lookingDir.normalized.y < 0);
        }

        void Inputs()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKeyDown(KeyCode.F1))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
        
        void AnimationController()
        {
            anim.SetFloat("mousePosX", aim.lookingDir.x);
            anim.SetFloat("mousePosY", aim.lookingDir.y);
            anim.SetBool("walking", isMoving);
            anim.SetBool("running", isRunning && isMoving);
            //spriteRenderer.flipX = aim.lookingDir.x < 0;
        }

        public bool Moving()
        {
            moveDir.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            return moveDir.magnitude != 0;
        }
        public void CamShake(float _duration, float _magnitude)
        {
            StartCoroutine(cam.CamShake(_duration, _magnitude));
        }

        void Death()
        {
            Debug.LogWarning("MORREU BURRO RUIM LIXO HORROROSO");
        }

        public void Damage(int _damage)
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
            StartCoroutine(DmageTaken());
        }

        public void Damage(int _damage, float _knockbackForce, Vector2 _dir)
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
            Knockback(_knockbackForce, _dir);
            StartCoroutine(DmageTaken());
        }

        public void Knockback(float _knockbackForce, Vector2 _dir)
        {
            rig.AddForce(_dir.normalized * _knockbackForce, ForceMode2D.Impulse);
        }

        IEnumerator DmageTaken()
        {
            canTakeDamage = false;
            for (int i = 0; i < godSeconds; i++)
            {
                spriteRenderer.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                spriteRenderer.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
            canTakeDamage = true;
        }

        public void Effect(Game.Itens.ItemEffect _effect)
        {
            switch (_effect.effect)
            {
                case StatusController.Effect.Life:
                    lifeBar.SetValue(Value(_effect.operation, lifeBar.CurrentValue, _effect.value));
                    break;
                case StatusController.Effect.Food:
                    hungryBar.SetValue(Value(_effect.operation, hungryBar.CurrentValue, _effect.value));
                    break;
                case StatusController.Effect.Water:
                    waterBar.SetValue(Value(_effect.operation, waterBar.CurrentValue, _effect.value));
                    break;
                case StatusController.Effect.Hapiness:
                    happinessBar.SetValue(Value(_effect.operation, happinessBar.CurrentValue, _effect.value));
                    break;
                case StatusController.Effect.Drunk:
                    isDrunk = Value(_effect.operation, 0, 0) == 1;
                    break;
                case StatusController.Effect.Stoned:
                    isStoned = Value(_effect.operation, 0, 0) == 1;
                    break;
            }
        }

        int Value(System.ArithmeticOperations _operation, int _currentValue, int _newValue)
        {
            switch (_operation)
            {
                case System.ArithmeticOperations.Plus:
                    return _currentValue + _newValue;
                case System.ArithmeticOperations.Minus:
                    return _currentValue - _newValue;
                case System.ArithmeticOperations.SetValue:
                    return _newValue;
                case System.ArithmeticOperations.SetTrue:
                    return 1;
                case System.ArithmeticOperations.SetFalse:
                    return 0;
                default: return 0;
            }
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
            if (_newValue >= maxValue)
            {
                _newValue = maxValue;
            }
            else if (_newValue <= minValue)
            {
                _newValue = minValue;
            }
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