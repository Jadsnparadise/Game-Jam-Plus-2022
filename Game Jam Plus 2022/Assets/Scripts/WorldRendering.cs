using Game.Itens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRendering : MonoBehaviour
{
    void Update()
    {
        if (isInScreen())
        {
            EnableDisableComponent(true);
        }
        else
        {
            EnableDisableComponent(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        

    }

    private void EnableDisableComponent(bool state) //true = enable Component // False = disable Component
    {
        if (gameObject.GetComponent<SpriteRenderer>() != null)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = state; // ativa o objeto
        }
        if (gameObject.GetComponent<ItemController>() != null)
        {
            gameObject.GetComponent<ItemController>().enabled = state; // desativa o objeto
        }
    }
}
