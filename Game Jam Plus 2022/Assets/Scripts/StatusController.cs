using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Game.StatusController
{
    public class StatusController : MonoBehaviour
    {
        [SerializeField] Slider lifeBar;
        [SerializeField] Slider staminaBar;
        [SerializeField] Slider waterBar;
        [SerializeField] Slider hungryBar;
        [SerializeField] Slider happinessBar;
        [SerializeField] System.Attribute temperature;


        [SerializeField] List<RawImage> conditionsUi;
        public List<Condition> conditions;
        [SerializeField] List<Condition> currentCondition;

        Player.Player playerStatus;


        int damageByStatus = 2;
        float damageByStatusCD = 3f;
        float currentCDStatusDamage;

        float modifyStaminaRate = 0.2f;
        float clockStamina;
        int staminaDecreseRunning = 5;
        int staminaDecreaseWalking = 1;
        int staminaIncreaseNormal = 2;
        int staminaIncreaseCold = 1;

        [SerializeField] bool isRegeneringStamina = false;


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

        public bool canRegenStamina = true;


        private void Start()
        {

            playerStatus = GameObject.Find("Player").GetComponent<Game.Player.Player>();
            foreach (RawImage r in conditionsUi)
            {
                r.color = Color.clear;
            }
            currentCondition = new();
        }

        private void Update()
        {

            StaminaIncrease();
            StaminaDecrease();
            UIUpdate();
            HungryDecrease(decreaseFood);
            WaterDecrease(decreaseWater);
            HappinessIncrease();
            HapinessDecrease();
            LifeControl();
            HotControl();
            ColdControl();


            if (waterBar.value == 0 || hungryBar.value == 0 || happinessBar.value == 0)
            {
                DamageStatus();
            }

        }

        private void LifeControl()
        {
            lifeBar.value = playerStatus.LifeBar.CurrentValue;
        }

        public void StaminaIncrease()
        {
            clockStamina += Time.deltaTime;
            if (clockStamina >= modifyStaminaRate && canRegenStamina)
            {
                if(staminaBar.value == playerStatus.StaminaBar.MaxValue)
                {
                    isRegeneringStamina = false;
                }
                playerStatus.StaminaBar.AddValue(playerStatus.IsCold ? staminaIncreaseCold : staminaIncreaseNormal);
                //playerStatus.HungryBar.AddValue(1);
                //playerStatus.WaterBar.AddValue(1);
                clockStamina = 0;
            }
        }
        public void StaminaDecrease()
        {
            clockStamina += Time.deltaTime;
            if (clockStamina >= modifyStaminaRate && !canRegenStamina)
            {
                playerStatus.StaminaBar.DecreaseValue(playerStatus.isRunning ? staminaDecreseRunning : staminaDecreaseWalking);
                clockStamina = 0;
            }
        }

        private void WaterDecrease(int _value)
        {
            currentWaterDecrease += Time.deltaTime;
            if (currentWaterDecrease >= waterDecreaseRate * (isRegeneringStamina ? 1 : 0.5f))
            {
                playerStatus.WaterBar.DecreaseValue(_value);//modificar depois para pegar diretamente do atributo do item
                waterBar.value = playerStatus.WaterBar.CurrentValue;
                currentWaterDecrease = 0;
            }
        }

        private void HungryDecrease(int _value)
        {
            currentFoodDecrease += Time.deltaTime;
            if (currentFoodDecrease >= foodDecreaseRate *(isRegeneringStamina? 1 : 0.5f))
            {
                playerStatus.HungryBar.DecreaseValue(_value);//modificar depois para pegar diretamente do atributo do item
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

        private void DamageStatus()
        {
            currentCDStatusDamage += Time.deltaTime;
            if (currentCDStatusDamage >= damageByStatusCD)
            {
                canRegenStamina = false;
                playerStatus.Damage(damageByStatus);
                currentCDStatusDamage = 0;
            }

        }

        private void ColdControl()
        {
            if (temperature.CurrentValue <= 20) //e não tiver usando casaco
            {
                playerStatus.Cold();
                StaminaIncrease();
            }
            else
            {
                playerStatus.DesCold();
            }
        }

        private void HotControl()
        {

            if (temperature.CurrentValue >= 30)
            {
                playerStatus.Hot();
                HungryDecrease(decreaseFood * 2);
                WaterDecrease(decreaseWater * 2);
            }
            else
            {
                playerStatus.DesHot();
            }


        }

        void UIUpdate()
        {
            lifeBar.value = playerStatus.LifeBar.CurrentValue;
            staminaBar.value = playerStatus.StaminaBar.CurrentValue;
            waterBar.value = playerStatus.WaterBar.CurrentValue;
            hungryBar.value = playerStatus.HungryBar.CurrentValue;
            happinessBar.value = playerStatus.HappinessBar.CurrentValue;
        }

        public Sprite PickConditionSprite(Effect currentCondition)
        {
            return conditions.Find(x => x.effect == currentCondition).sprite;
        }

        public void UiAddCondition(Condition _newCondition)
        {
            if (currentCondition.Exists(x => x.effect == _newCondition.effect))
            {
                return;
            }
            currentCondition.Add(_newCondition);
            UiConditionUpdate();
        }

        public void UiRemoveCondition(Condition _condition)
        {
            if (!currentCondition.Exists(x => x.effect == _condition.effect))
            {
                return;
            }
            currentCondition.Remove(_condition);
            UiConditionUpdate();
        }

        void UiConditionUpdate()
        {
            for (int i = 0; i < conditionsUi.Count; i++)
            {
                conditionsUi[i].texture = currentCondition.Count > i ? currentCondition[i].sprite.texture : null;
                if (conditionsUi[i].texture != null)
                {
                    conditionsUi[i].color = Color.white;
                }
                else
                {
                    conditionsUi[i].color = Color.clear;
                }
                conditionsUi[i].SetNativeSize();
            }
        }
    }

    [Serializable]
    public struct Condition
    {
        public Effect effect;
        public Sprite sprite;
    }
}