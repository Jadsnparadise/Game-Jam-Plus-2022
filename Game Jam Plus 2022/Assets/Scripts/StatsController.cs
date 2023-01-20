using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.System;
using Game.Player;

public class StatsController : MonoBehaviour
{
    [SerializeField] Image lifeBar;
    GameObject player;
    Game.Player.Player playerStats;



    private void Start()
    {
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<Game.Player.Player>();
    }

    private void Update()
    {
        Debug.Log("Vida atual: " + playerStats.LifeBar.CurrentValue);
        Debug.Log("Vida max: " + playerStats.LifeBar.MaxValue);
        Debug.Log("fillamount: " + lifeBar.fillAmount);
        LifeControl();
    }

    public void LifeControl()
    {
        lifeBar.fillAmount = playerStats.LifeBar.StartValue / playerStats.LifeBar.MaxValue;

    }
}
