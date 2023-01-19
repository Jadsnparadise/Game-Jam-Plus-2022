using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] GameObject hand;
    SpriteRenderer handSpriteRenderer;
    public Vector2 lookingDir { get; private set; }
    [SerializeField] Game.Itens.ItemScriptable currentItem;

    void Start()
    {
        handSpriteRenderer = hand.GetComponent<SpriteRenderer>();
        currentItem.ItemStart();
    }

    // Update is called once per frame
    void Update()
    {
        handSpriteRenderer.sprite = currentItem.itemSprite;
        Aim();
        currentItem.ItemUpdate();
        if (Input.GetButton("Fire1"))
        {
            currentItem.Atacking(hand.transform.position, hand.transform.rotation);
        }
    }

    void Aim()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        lookingDir = new(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(lookingDir.y, lookingDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        handSpriteRenderer.flipY = mousePos.x < screenPoint.x;
    }
}
