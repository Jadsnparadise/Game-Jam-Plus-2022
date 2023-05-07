using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusController : MonoBehaviour
{

    public float m_Hungry = 100;
    public float m_Thirst = 100;
    public float m_Happiness = 100;
    public float m_Stamin = 100;
    public float m_HungryRate = 0.01f;
    public float m_ThirstRate = 0.01f;
    public float m_HappinessRate = 0.01f;    
    public float m_StaminDecrementRate = 0.01f;
    public float m_StaminIncrementRate = 0.01f;

    public float m_ThirstRateDefaultNormal = 0.01f;
    public float m_ThirstRateDefaultRecuperation = 0.02f;
    public float m_HungryRateDefaultNormal = 0.005f;
    public float m_HungryRateDefaultRecuperation = 0.01f;

    public Slider m_StaminBar;
    public Slider m_HungryBar;
    public Slider m_ThirstyBar;
    public Slider m_HappinessBar;
    public Slider m_HealthBar;

    public GameObject m_Player;
    public GameObject m_Conditions;

    public int m_Timepoison = 5;
    

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        m_StaminBar.value = m_Stamin;
        m_HungryBar.value = m_Hungry;
        m_ThirstyBar.value = m_Thirst;
        m_HappinessBar.value = m_Happiness;
        m_HealthBar.value = m_Player.GetComponent<Game.Player.Player>().HealthAccess;

        //Condições
        //Se bebado
        if(m_Conditions.GetComponent<Conditions>().m_Drunk == true)
        {
            m_ThirstRateDefaultNormal = 0.05f;
            m_ThirstRateDefaultRecuperation = 0.1f;
        }
        else
        {
            m_ThirstRateDefaultNormal = 0.01f;
            m_ThirstRateDefaultRecuperation = 0.02f;
        }
        //Se chapado
        
        if (m_Conditions.GetComponent<Conditions>().m_Chapado == true)
        {
            //m_HungryRate = 0.1f;
            if(m_Happiness < 100)
            {
                m_Happiness = m_Happiness + 0.05f;
            }
        }
        else
        {
            //m_HungryRate = 0.01f;
        }
        //Se frio
        if(m_Conditions.GetComponent<Conditions>().m_Cold == true)
        {            
            m_HungryRateDefaultNormal = 0.05f;
            m_HungryRateDefaultRecuperation = 0.1f;
        }
        else
        {
            m_HungryRateDefaultNormal = 0.005f;
            m_HungryRateDefaultRecuperation = 0.01f;
        }
        //Se calor
        if (m_Conditions.GetComponent<Conditions>().m_Hot == true)
        {
            m_ThirstRate = 0.3f;
        }
        else
        {
            m_ThirstRate = m_ThirstRateDefaultNormal;
        }


    }

    public void StaminaDecrement()
    {
        if(m_Stamin > 0)
        {
            m_Stamin -= m_StaminDecrementRate;
        }
    }
    public void StaminaIncrement()
    {
        if (m_Stamin < 300)
        {
            m_ThirstRate = m_ThirstRateDefaultRecuperation;
            m_HungryRate = m_HungryRateDefaultRecuperation;
            m_Stamin += m_StaminIncrementRate;
        }
        else
        {
            m_ThirstRate = m_ThirstRateDefaultNormal;
            m_HungryRate = m_HungryRateDefaultNormal;
        }
    }
}
