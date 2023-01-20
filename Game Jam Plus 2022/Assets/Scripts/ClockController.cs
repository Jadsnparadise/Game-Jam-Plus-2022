using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.ComponentModel.Design.Serialization;
using JetBrains.Annotations;
using Unity.VisualScripting;


namespace Game.Clock
{
    public class ClockController : MonoBehaviour
    {
        //1minuto = 0.5 segundos
        //1hora = 30 segundos
        //12h = 360 segundos
        //24h = 720 segundos
        [SerializeField] float seconds;
        [SerializeField] TextMeshProUGUI minutesInClock;
        [SerializeField] TextMeshProUGUI hoursInClock;
        [SerializeField] TextMeshProUGUI currentDay;
        int daysSurvived = 0;
        float minInThisWorld = 0.5f;
        int minutes = 0;
        public int Minutes { get { return minutes; } private set { Minutes = value; } }

        int minutesLimit = 59;
        int hours = 0;
        int hoursLimit = 23;

        private void FixedUpdate()
        {
            seconds += Time.deltaTime;
            IncreaseMinute();
            IncreaseHour();
        }

        private void IncreaseMinute()
        {
            if (seconds >= minInThisWorld)
            {
                minutes++;
                minutesInClock.text = minutes.ToString("00");
                seconds = 0;
            }
        }

        private void IncreaseHour()
        {
            if (minutes >= minutesLimit && hours < hoursLimit)
            {
                hours++;
                hoursInClock.text = hours.ToString("00");
                minutes = 0;
            }
            else if(minutes >= minutesLimit && hours == hoursLimit)
            {
                hours = 0;
                IncreaseDaysSurvived();
            }
        }

        private void IncreaseDaysSurvived()
        {
            daysSurvived++;
            currentDay.text = daysSurvived.ToString("00");
        }
    
    }
}