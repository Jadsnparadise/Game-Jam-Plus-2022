using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesMovementController : MonoBehaviour
{
    
    [SerializeField] ParticleSystem movementParticle;
    [Range(0,10)]
    [SerializeField] float walkingDustFormationClock;
    [Range(0, 10)]
    [SerializeField] float runningDustFormationClock;
    Game.Player.Player player;
    float currentClock;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Game.Player.Player>();
    }
    // Update is called once per frame
    void Update()
    {
        currentClock += Time.deltaTime;

        walkingDustFormationClock = player.isRunning ? 0.2f : 0.3f;
        if(currentClock >= walkingDustFormationClock && (player.isMoving || player.isRunning) )
        {            
                movementParticle.Play();
                currentClock = 0;            
        }
    }
}
