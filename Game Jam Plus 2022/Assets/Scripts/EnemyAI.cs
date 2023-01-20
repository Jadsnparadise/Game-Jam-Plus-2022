using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

namespace Game.Enemy.AI
{
    using CollisionSystem;
    public class EnemyAI : MonoBehaviour
    {

        [SerializeField] NavMeshAgent enemy;

        [SerializeField] Transform player;

        [SerializeField] LayerMask groundLayer, playerLayer;
        [SerializeField] System.Attribute life;
        
        [SerializeField] float CDWalkPoint;
        [SerializeField] float currentCDWalkPoint;
        
        //vigiando
        [SerializeField] Vector3 walkPoint; //o ponto o qual o inimigo ira se mover
        [SerializeField] bool walkPointSet; //controla se o walkPoint está setado
        [SerializeField] float walkPointRange; //o range que o walkPoint poderá estar
        [SerializeField, Min(1)] float minRange = 1f;

        //Atacando
        [SerializeField] float coolDownAttack;
        [SerializeField] int enemyDamage;
        public int EnemyDamage { get { return enemyDamage; } private set { EnemyDamage = value; } }
        
        bool attacked;

        //controle dos estados
        [SerializeField] float sightRange;
        [SerializeField] float attackRange;
        [SerializeField] bool playerInSightRange;
        [SerializeField] bool playerInAttackRange;

        private void Awake()
        {          
            player = GameObject.Find("Player").transform;
            enemy = GetComponent<NavMeshAgent>();
            enemy.updateRotation = false; 
            enemy.updateUpAxis = false;
        }

        private void Update()
        {
            currentCDWalkPoint += Time.deltaTime;

            playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, playerLayer);
            playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

            //3 estados possíveis para a IA
            if (!playerInSightRange && !playerInAttackRange) Patrol();
            if (playerInSightRange && !playerInAttackRange) Chase();
            if (playerInSightRange && playerInAttackRange) Attack();

        }

        private void Patrol()
        {
            if (walkPointSet)
            {
                enemy.SetDestination(walkPoint);
            }
            else if(currentCDWalkPoint >= CDWalkPoint)
            {
                WalkPointGeneration();
            }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (Mathf.Abs(distanceToWalkPoint.magnitude) < minRange)
            {
                Debug.Log("Distancia até o ponto que tenho q andar: " + distanceToWalkPoint.magnitude);
                Debug.Log("Distancia até o ponto que tenho q andar: " + distanceToWalkPoint.magnitude);
                walkPointSet = false ;
                //currentCDWalkPoint = 0;
            }
            
        }

        private void WalkPointGeneration()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomY = Random.Range(-walkPointRange, walkPointRange);
            Debug.Log("passou aqui");

            walkPoint = new Vector2(transform.position.x + randomX, transform.position.y + randomY);

            //if (Physics2D.Raycast(walkPoint, -transform.up, 10f, groundLayer))
            if(Physics2D.OverlapCircle(walkPoint, 2f, groundLayer))
            {
                //(origin, direction, maxDistance, layerMask)
                walkPointSet = true;
                currentCDWalkPoint = 0;
            }
        }

        private void Chase()
        {
            enemy.SetDestination(player.position);
        }

        private void Attack()
        {
            enemy.SetDestination(player.position);

            //transform.LookAt(player);

            if (!attacked)
            {
                Debug.Log("Atacou");


                attacked = true;
                Invoke(nameof(AttackCoolDown), coolDownAttack);
            }
        }

        private void AttackCoolDown()
        {
            attacked = false;
        }

        void Death()
        {
            Destroy(gameObject);
        }

        public void Damage(int _damage)
        {
            life.DecreaseValue(_damage);
            if (life.CurrentValue <= life.MinValue)
            {
                Death();
            }
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
