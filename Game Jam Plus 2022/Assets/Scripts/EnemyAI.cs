using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.UI.Image;

namespace Game.Enemy.AI
{
    using CollisionSystem;
    using Unity.Burst.Intrinsics;

    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] NavMeshAgent enemy;
        SpriteRenderer sprite;

        [SerializeField] Transform player;

        [SerializeField] LayerMask groundLayer, playerLayer;

        [SerializeField] float CDWalkPoint;
        [SerializeField] float currentCDWalkPoint;

        [SerializeField] float lookingDirX = 0;
        [SerializeField] float lookingDirY = 0;
        Animator anim;

        //vigiando
        [SerializeField] Vector3 walkPoint; //o ponto o qual o inimigo ira se mover
        [SerializeField] bool walkPointSet; //controla se o walkPoint está setado
        [SerializeField] float walkPointRange; //o range que o walkPoint poderá estar
        [SerializeField, Min(1)] float minRange = 1f;

        //Atacando
        [SerializeField] float coolDownAttack;
        [SerializeField] int enemyDamage;

        public int EnemyDamage { get { return enemyDamage; } private set { EnemyDamage = value; } }
        Collision hitbox;
        bool attacked;

        //controle dos estados
        [SerializeField] float sightRange;
        [SerializeField] float attackRange;
        [SerializeField] bool playerInSightRange;
        [SerializeField] bool playerInAttackRange;
        bool isChasing;
        bool isWalking;
        [SerializeField] Vector3 enemyOffSet;


        private void Awake()
        {
            player = GameObject.Find("Player").transform;
            enemy ??= GetComponent<NavMeshAgent>();
            anim ??= GetComponent<Animator>();
            sprite ??= GetComponent<SpriteRenderer>();
            enemy.updateRotation = false;
            enemy.updateUpAxis = false;
        }

        private void Update()
        {
            sprite.flipX = player.transform.position.x > transform.position.x;
            /*if (hitbox.InCollision(transform, out Collider2D[] objects))
            {
                foreach (Collider2D c in objects)
                {
                    Player.Player playerStatus = c.GetComponent<Player.Player>();
                    playerStatus.Damage(EnemyDamage);
                }
            }*/

            currentCDWalkPoint += Time.deltaTime;

            playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, playerLayer);
            playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

            //3 estados possíveis para a IA
            if (!playerInSightRange && !playerInAttackRange) Patrol();
            if (playerInSightRange && !playerInAttackRange) Chase();
            if (playerInSightRange && playerInAttackRange) Attack();

            LookingDir();
            isIdle();
            AnimationController();
        }

        void AnimationController()
        {
            anim.SetFloat("LookingDirX", lookingDirX);
            anim.SetFloat("LookingDirY", lookingDirY);
            anim.SetBool("Attacking", attacked);
            anim.SetBool("isWalking", !isIdle());
            anim.SetBool("Idle", isIdle());
        }

        void LookingDir()
        {
            walkPoint = isChasing ? player.transform.position : walkPoint;

            lookingDirX = walkPoint.x - transform.position.x;
            lookingDirY = walkPoint.y - transform.position.y;
            Debug.Log("LookX: " + lookingDirX);
            Debug.Log("LookY: " + lookingDirY);
            //lookingDirX = walkPoint.x + enemyOffSet.x;
            //lookingDirY = walkPoint.y + enemyOffSet.y;
        }

        private void Patrol()
        {
            isChasing = false;
            //isWalking = true;
            if (walkPointSet)
            {
                if (enemy.velocity == Vector3.zero)
                {
                    //isWalking = false;
                    walkPointSet = false;
                }
                enemy.SetDestination(walkPoint);
            }
            else if (currentCDWalkPoint >= CDWalkPoint)
            {
                WalkPointGeneration();
            }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (Mathf.Abs(distanceToWalkPoint.magnitude) < minRange)
            {

                walkPointSet = false;
            }
        }

        private void WalkPointGeneration()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomY = Random.Range(-walkPointRange, walkPointRange);
            Debug.Log("passou aqui");

            walkPoint = new Vector2(transform.position.x + randomX, transform.position.y + randomY);

            if (Physics2D.OverlapCircle(walkPoint, 2f, groundLayer))
            {
                //(origin, direction, maxDistance, layerMask)
                walkPointSet = true;
                currentCDWalkPoint = 0;
            }
        }

       

        bool isIdle()
        {
            return enemy.velocity == Vector3.zero;
        }

        private void Chase()
        {
            isChasing = true;
            //isWalking = true;
            enemy.SetDestination(player.position);
        }

        private void Attack()
        {
            enemy.SetDestination(player.position);
            if (!attacked)
            {
                isChasing = false;
                //isWalking = false;
                attacked = true;
                Invoke(nameof(AttackCoolDown), coolDownAttack);
            }
        }

        private void AttackCoolDown()
        {
            attacked = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }
    }
}
