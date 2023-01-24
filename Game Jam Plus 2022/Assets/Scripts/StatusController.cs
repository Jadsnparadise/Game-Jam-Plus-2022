using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Clock;
using Unity.VisualScripting;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

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

        int DamageByStatus = 2;
        public int damageByStatus { get { return DamageByStatus; } private set { damageByStatus = value; } }

        float DamageByStatusCD = 3f;
        public float damageByStatusCD { get { return DamageByStatusCD; } private set { damageByStatusCD = value; } }

        float CurrentCDStatusDamage;
        public float currentCDStatusDamage { get { return CurrentCDStatusDamage; } private set { currentCDStatusDamage = value; } }

        float modifyStaminaRate = 0.2f;
        float clockStamina;

        float waterDecreaseRate = 14f;
        float currentWaterDecrease;
        int decreaseWater = 1;

        float foodDecreaseRate = 14f;
        float currentFoodDecrease;
        int decreaseFood = 1;
        bool stoneAlreadyDecresedFood;

        float happinessDecreaseRate = 14f;
        float currentHappinessDecrease;
        int decreaseHappiness = 1;

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

            HungryDecrease();
            HungryIncrease();
            WaterIncrease();
            waterDecrease();
            HappinessIncrease();
            HapinessDecrease();
            LifeControl();
            StonedControl();
            DrunkControl();
            PoisonedControl();
            HotControl();
            ColdControl();

            

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
            if (playerStatus.WaterBar.CurrentValue < playerStatus.WaterBar.MaxValue && Input.GetKeyDown(KeyCode.R)) // depois modificar para ser quando consumir o item
            {
                //se o objeto for água aumentar em 25 a barra de água
                waterBar.value += 25;
                playerStatus.WaterBar.AddValue(25);//modificar depois para pegar diretamente do atributo do item
                happinessBar.value += 10;
            }
        }

        private void waterDecrease()
        {
            currentWaterDecrease += Time.deltaTime;
            if (currentWaterDecrease >= waterDecreaseRate)
            {
                playerStatus.WaterBar.DecreaseValue(decreaseWater);//modificar depois para pegar diretamente do atributo do item
                waterBar.value = playerStatus.WaterBar.CurrentValue;
                currentWaterDecrease = 0;
            }
        }
        private void HungryIncrease()
        {
            hungryBar.value = playerStatus.HungryBar.CurrentValue;
            

        }

        private void HungryDecrease()
        {
            currentFoodDecrease += Time.deltaTime;
            if (currentFoodDecrease >= foodDecreaseRate)
            {
                playerStatus.HungryBar.DecreaseValue(decreaseFood);//modificar depois para pegar diretamente do atributo do item
                hungryBar.value = playerStatus.HungryBar.CurrentValue;
                currentFoodDecrease = 0;
            }
            if (playerStatus.IsStoned && !stoneAlreadyDecresedFood)
            {
                playerStatus.HungryBar.DecreaseValue(33);
                stoneAlreadyDecresedFood = true;
            }

        }

        private void HappinessIncrease()
        {
            happinessBar.value = playerStatus.HappinessBar.CurrentValue;
            if (playerStatus.IsStoned)
            {
                playerStatus.HappinessBar.AddValue(playerStatus.HappinessBar.MaxValue);

            }
        }

        private void HapinessDecrease()
        {
            currentHappinessDecrease += Time.deltaTime;
            if (currentHappinessDecrease >= happinessDecreaseRate)
            {
                playerStatus.HappinessBar.DecreaseValue(decreaseHappiness);//modificar depois para pegar diretamente do atributo do item
                happinessBar.value = playerStatus.HappinessBar.CurrentValue;
                currentHappinessDecrease = 0;
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