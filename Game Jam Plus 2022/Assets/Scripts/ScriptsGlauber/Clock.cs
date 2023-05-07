using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{

    public int m_Hour;
    public int m_Minutes;
    public float m_Seconds;
    public int m_Day;

    public bool m_IsNight;
    public string m_PeriodOfDay;

    public int m_DefaultHour = 24;
    public int m_DefaultMinutes = 60;
    public float m_DefaultSeconds = 0.5f;
    public int m_DefaultDay = 0;


    public GameObject m_Conditions;
    public GameObject m_StatusController;
    public GameObject m_Player;

    public TextMeshProUGUI m_HourTx;
    public TextMeshProUGUI m_PeriodOfDayTx;
    public TextMeshProUGUI m_DayTx;




    // Start is called before the first frame update
    void Start()
    {
        m_StatusController = GameObject.Find("StatusControllerNew");
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Conditions = GameObject.Find("ConditionsSV");

    }

    // Update is called once per frame
    void Update()
    {
         m_Seconds += Time.deltaTime;

        //Controle dos minutos
        if(m_Seconds >= m_DefaultSeconds)
        {
            ++m_Minutes;
            m_Seconds = 0;
            StatusControllerTimeForMinutes();
        }
        //Controle das horas
        if(m_Minutes >= m_DefaultMinutes)
        {
            ++m_Hour;
            m_Minutes = 0;
            StatusControllerTimeForHours();
        }
        //Controle dos dias
        if(m_Hour >= m_DefaultHour)
        {
            ++m_Day;
            m_Hour = 0;
        }
        //Controle do periodo do dia Siginificado = AM: ante meridiem, antes do meio-dia / PM: post meridiem, após o meio-dia
        if (m_Hour == 0)
        {            
            m_PeriodOfDay = "AM";
        }
        if(m_Hour == 12)
        {            
            m_PeriodOfDay = "PM";
        }
        //Controle do dia e da noite
        if (m_Hour == 18 && m_IsNight == false)
        {
            m_IsNight = true;            
        }
        if (m_Hour == 6 && m_IsNight == true)
        {
            m_IsNight = false;            

        }
        //Mostrar horas, periodo do dia, dias que se passou
        m_HourTx.text = m_Hour.ToString();
        m_PeriodOfDayTx.text = m_PeriodOfDay;
        m_DayTx.text = m_Day.ToString();

    }
    
    //Função de chamada por minutos
    public void StatusControllerTimeForMinutes()
    {
        //
        //Controle da fome
        if (m_StatusController.GetComponent<PlayerStatusController>().m_Hungry > 0)
        {
            m_StatusController.GetComponent<PlayerStatusController>().m_Hungry -= m_StatusController.GetComponent<PlayerStatusController>().m_HungryRate;
        }
        //Controle da sede
        if (m_StatusController.GetComponent<PlayerStatusController>().m_Thirst > 0)
        {
            m_StatusController.GetComponent<PlayerStatusController>().m_Thirst -= m_StatusController.GetComponent<PlayerStatusController>().m_ThirstRate;
        }
        //Controle da felicidade
        if (m_StatusController.GetComponent<PlayerStatusController>().m_Happiness > 0)
        {
            m_StatusController.GetComponent<PlayerStatusController>().m_Happiness -= m_StatusController.GetComponent<PlayerStatusController>().m_HappinessRate;
        }
        //Controle da Stamina
        if(m_Player.GetComponent<Game.Player.Player>().isMoving == true)
        {            
            m_StatusController.GetComponent<PlayerStatusController>().StaminaDecrement();
        }
        else if(m_Player.GetComponent<Game.Player.Player>().isMoving == false)
        {            
            m_StatusController.GetComponent<PlayerStatusController>().StaminaIncrement();
        }
        
        
    }
    //Função de chamada por horas
    public void StatusControllerTimeForHours()
    {
        //Controle de saude se fome
        if (m_Player.GetComponent<Game.Player.Player>().HealthAccess > 0 && m_StatusController.GetComponent<PlayerStatusController>().m_Hungry <= 0)
        {
            m_Player.GetComponent<Game.Player.Player>().HealthAccess = m_Player.GetComponent<Game.Player.Player>().HealthAccess - 1;
        }
        //Controle de saude se sede
        if (m_Player.GetComponent<Game.Player.Player>().HealthAccess > 0 && m_StatusController.GetComponent<PlayerStatusController>().m_Thirst <= 0)
        {
            m_Player.GetComponent<Game.Player.Player>().HealthAccess = m_Player.GetComponent<Game.Player.Player>().HealthAccess - 3;
        }
        //Controle de saude se triste
        if (m_Player.GetComponent<Game.Player.Player>().HealthAccess > 5 && m_StatusController.GetComponent<PlayerStatusController>().m_Happiness <= 0)
        {
            m_Player.GetComponent<Game.Player.Player>().HealthAccess = 5;
        }
        //Controle de saude se envenenado
        if (m_Player.GetComponent<Game.Player.Player>().HealthAccess > 0 && m_Conditions.GetComponent<Conditions>().m_Poison == true && m_StatusController.GetComponent<PlayerStatusController>().m_Timepoison > 0)
        {
            m_StatusController.GetComponent<PlayerStatusController>().m_Timepoison = m_StatusController.GetComponent<PlayerStatusController>().m_Timepoison - 1;
            m_Player.GetComponent<Game.Player.Player>().HealthAccess = m_Player.GetComponent<Game.Player.Player>().HealthAccess - 1;
            if(m_StatusController.GetComponent<PlayerStatusController>().m_Timepoison <= 0)
            {
                m_Conditions.GetComponent<Conditions>().m_Poison = false;
                m_StatusController.GetComponent<PlayerStatusController>().m_Timepoison = 5;

            }
        }
        m_Conditions.GetComponent<Conditions>().CheckConditions();

    }
}
