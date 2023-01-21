using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Clock;
using Unity.VisualScripting;

namespace Game.StatusController
{
    public class StatusController : MonoBehaviour
    {
        [SerializeField] Slider lifeBar;
        [SerializeField] Slider staminaBar;
        [SerializeField] Slider waterBar;
        [SerializeField] Slider hungryBar;
        [SerializeField] Slider happinessBar;

        bool stoned;
        bool drunk;
        bool poisoned;
        bool cold;
        bool hot;

        GameObject player;
        Game.Player.Player playerStatus;

        GameObject Clock;
        Game.Clock.ClockController clock;

        float modifyStaminaRate = 0.2f;
        float clockStamina;

        private void Start()
        {
            player = GameObject.Find("Player");
            playerStatus = player.GetComponent<Game.Player.Player>();
            //clock = Clock.GetComponent<Game.Clock.ClockController>();
        }

        private void Update()
        {
            Debug.Log("Vida atual: " + playerStatus.LifeBar.CurrentValue);
            Debug.Log("Vida max: " + playerStatus.LifeBar.MaxValue);
            Debug.Log("fillamount: " + lifeBar.value);
            LifeControl();
        }

        private void LifeControl()
        {
            lifeBar.value = playerStatus.LifeBar.CurrentValue;
        }

        public void StaminaIncrease()
        {
            clockStamina += Time.deltaTime;
            if (clockStamina >= modifyStaminaRate)
            {
                playerStatus.StaminaBar.AddValue(2);
                staminaBar.value = playerStatus.StaminaBar.CurrentValue;
                clockStamina = 0;
            }
        }
        public void StaminaDecrease()
        {
            clockStamina += Time.deltaTime;
            if (clockStamina >= modifyStaminaRate)
            {
                playerStatus.StaminaBar.DecreaseValue(5);
                staminaBar.value = playerStatus.StaminaBar.CurrentValue;
                clockStamina = 0;
            }

            
        }

        private void WaterIncrease()
        {
            if (playerStatus.WaterBar.CurrentValue < playerStatus.WaterBar.MaxValue)
            {
                //se o objeto for água aumentar em 25 a barra de água
                waterBar.value += 25;
                playerStatus.WaterBar.AddValue(25);//modificar depois para pegar diretamente do atributo do item
            }
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

