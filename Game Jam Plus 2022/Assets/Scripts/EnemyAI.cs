using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

namespace Game.Enemy.AI
{


    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] NavMeshAgent enemy;

        [SerializeField] Transform player;

        [SerializeField] LayerMask groundLayer, playerLayer;

        //vigiando
        [SerializeField] Vector3 walkPoint; //o ponto o qual o inimigo ira se mover
        [SerializeField] bool walkPointSet; //controla se o walkPoint est� setado
        [SerializeField] float walkPointRange; //o range que o walkPoint poder� estar
        [SerializeField, Min(1)] float minRange = 1f;

        //Atacando
        [SerializeField] float coolDownAttack;
        [SerializeField] bool attacked;

        //controle dos estados
        [SerializeField] float sightRange, attackRange;
        [SerializeField] bool playerInSightRange, playerInAttackRange;

        private void Awake()
        {          
            player = GameObject.Find("Player").transform;
            enemy = GetComponent<NavMeshAgent>();
            enemy.updateRotation = false; enemy.updateUpAxis = false;
        }

        private void Update()
        {
            //playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
            playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, playerLayer);
            playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);

            //3 estados poss�veis para a IA
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
            else
            {
                WalkPointGeneration();
            }

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (Mathf.Abs(distanceToWalkPoint.magnitude) < minRange) walkPointSet = false;
        }

        private void WalkPointGeneration()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomY = Random.Range(-walkPointRange, walkPointRange);
            

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY,0f);

            //if (Physics2D.Raycast(walkPoint, -transform.up, 10f, groundLayer))
            if(Physics2D.OverlapCircle(walkPoint, 0.1f, groundLayer))
            {
                //(origin, direction, maxDistance, layerMask)
                Debug.Log("passou aqui");
                walkPointSet = true;
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
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sightRange);
        }

    }
}
