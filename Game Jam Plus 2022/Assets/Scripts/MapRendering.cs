using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.System.Map
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MapRendering : MonoBehaviour
    {
        Transform playerTransform;
        SpriteRenderer spriteRenderer;
        [SerializeField] float offset;
        void Start()
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            spriteRenderer.sortingOrder = playerTransform.position.y + offset <= transform.position.y ? -1 : 1;
        }
    }
}