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
        [SerializeField] TextMeshProUGUI DayPeriod;
        [SerializeField] TextMeshProUGUI hoursInClock;
        [SerializeField] TextMeshProUGUI currentDay;
        int daysSurvived = 0;
        float minInThisWorld = 0.5f;
        int minutes = 0;
        [SerializeField] bool am = true;
        [SerializeField] bool pm = false;
        public bool isNight;

        public int Minutes { get { return minutes; } private set { Minutes = value; } }

        int minutesLimit = 59;
        int hours = 0;
        int hoursLimit = 12;

        private void FixedUpdate()
        {
            seconds += Time.deltaTime;
            IncreaseMinute();
            IncreaseHour();
            dayTime();
        }

        void setDayPeriod()
        {
            if(hours >= 12 && am)
            {
                am = false;
                pm = true;
                DayPeriod.text = "pm";
            }
            else if(hours >= 12 && pm)
            {
                pm = false;
                am = true;
                DayPeriod.text = "am";
                IncreaseDaysSurvived();
            }
        }

        private void IncreaseMinute()
        {
            if (seconds >= minInThisWorld)
            {
                minutes++;
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
                setDayPeriod();
                hours = 0;
                
            }
        }

        private void IncreaseDaysSurvived()
        {
            daysSurvived++;
            currentDay.text = daysSurvived.ToString("00");
        }

        void dayTime()
        {
            if (hours > 6 && pm || hours < 5 && am)
            {
                isNight = true;
            }
        }
    
    }
}