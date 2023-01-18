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
        [SerializeField] bool walkPointSet; //controla se o walkPoint está setado
        [SerializeField] float walkPointRange; //o range que o walkPoint poderá estar

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
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

            //3 estados possíveis para a IA
            if (!playerInSightRange && !playerInAttackRange) Patrol();
            if (playerInSightRange && !playerInAttackRange) Chase();
            if (playerInSightRange && playerInAttackRange) Attack();
        }

        private void Patrol()
        {
            if (!walkPointSet) WalkPointGeneration();

            if (walkPointSet) enemy.SetDestination(walkPoint);

            Vector3 distanceToWalkPoint = transform.position - walkPoint;

            if (distanceToWalkPoint.magnitude < 0.2f) walkPointSet = false;
        }

        private void WalkPointGeneration()
        {
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            float randomY = Random.Range(-walkPointRange, walkPointRange);
            

            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y + randomY,0f);

            if (Physics.Raycast(walkPoint, -transform.right, 10f, groundLayer))
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

            transform.LookAt(player);

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
