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

        [SerializeField] GameObject stonedUI;
        [SerializeField] GameObject drunkUI;
        [SerializeField] GameObject poisonedUI;
        [SerializeField] GameObject coldUI;
        [SerializeField] GameObject hotUI;

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
            //Debug.Log("Vida atual: " + playerStatus.LifeBar.CurrentValue);
            //Debug.Log("Vida max: " + playerStatus.LifeBar.MaxValue);
            //Debug.Log("fillamount: " + lifeBar.value);
            LifeControl();
            StonedControl();
            DrunkControl();
            PoisonedControl();
            HotControl();
            ColdControl();
            Happiness();
            HungryControl();
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
            hungryBar.value = playerStatus.Hungrybar.CurrentValue;
            if (playerStatus.IsStoned)
            {
                playerStatus.Hungrybar.DecreaseValue(1);
            }
        }

        private void Happiness()
        {
            happinessBar.value = playerStatus.Hapinessbar.CurrentValue;
            if (playerStatus.IsStoned)
            {
                playerStatus.Hapinessbar.AddValue(playerStatus.Hapinessbar.MaxValue);

            }
        }

        private void StonedControl()
        {
            if (playerStatus.IsStoned)
            {
                stonedUI.SetActive(true);
            }
            else
            {
                stonedUI.SetActive(false);
            }
        }

        private void DrunkControl()
        {
            if (playerStatus.IsDrunk)
            {
                drunkUI.SetActive(true);
            }
            else
            {
                drunkUI.SetActive(false);
            }
        }

        private void PoisonedControl()
        {
            if (playerStatus.IsPoisoned)
            {
                poisonedUI.SetActive(true);
            }
            else
            {
                poisonedUI.SetActive(false);
            }
        }
        private void ColdControl()
        {
            if (playerStatus.IsCold)
            {
                coldUI.SetActive(true);
            }
            else
            {
                coldUI.SetActive(false);
            }
        }
        private void HotControl()
        {
            if (playerStatus.IsHot)
            {
                hotUI.SetActive(true);
            }
            else
            {
                hotUI.SetActive(false);
            }
        }
    }
}