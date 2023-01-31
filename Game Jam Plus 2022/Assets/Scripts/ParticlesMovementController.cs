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

       
        if(currentClock >= walkingDustFormationClock)
        {
            if (player.isMoving)
            {
                movementParticle.Play();
                currentClock = 0;
            }
        }
    }
}
