using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Clock;

namespace Game.StatusController
{
    public class StatusController : MonoBehaviour
    {
        [SerializeField] Slider lifeBar;
        [SerializeField] Slider stamninaBar;
        [SerializeField] Slider waterBar;
        [SerializeField] Slider hungryBar;
        [SerializeField] Slider happinessBar;

        bool stoned;
        bool drunk;
        bool poisoned;
        bool cold;
        bool hot;

        GameObject player;
        Game.Player.Player playerStats;

        GameObject Clock;
        Game.Clock.ClockController clock;



        private void Start()
        {
            player = GameObject.Find("Player");
            playerStats = player.GetComponent<Game.Player.Player>();
            clock = Clock.GetComponent<Game.Clock.ClockController>();
        }

        private void Update()
        {
            Debug.Log("Vida atual: " + playerStats.LifeBar.CurrentValue);
            Debug.Log("Vida max: " + playerStats.LifeBar.MaxValue);
            Debug.Log("fillamount: " + lifeBar.value);
            LifeControl();
        }

        private void LifeControl()
        {
            lifeBar.value = playerStats.LifeBar.CurrentValue;
        }

        public void StaminaIncrease()
        {
            stamninaBar.value++;
        }
        public void StaminaDecrease()
        {
            stamninaBar.value--;
        }

        private void WaterControl()
        {

        }

        private void HungryControl()
        {

        }

        private void Happiness()
        {

        }

        private void StonedControl()
        {

        }

        private void DrunkControl()
        {

        }

        private void PoisonedControl()
        {

        }
        private void ColdControl()
        {

        }
        private void HotControl()
        {

        }
    }
}

