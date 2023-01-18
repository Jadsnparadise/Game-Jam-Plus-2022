using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField, Min(1)] float speed;
    [SerializeField, Range(0, 3)] float sprintMultiplier;
    float sprint;
    Vector2 moveDir;
    Vector2 lookingDir;
    Rigidbody2D rig;
    void Start()
    {
        rig ??= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        sprint = Input.GetKey(KeyCode.LeftShift) ? 1 : 0;
        
        moveDir.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void FixedUpdate()
    {
        float currentSpeed = speed + (sprint * sprintMultiplier);
        rig.AddForce(moveDir * currentSpeed, ForceMode2D.Impulse);
    }
}
