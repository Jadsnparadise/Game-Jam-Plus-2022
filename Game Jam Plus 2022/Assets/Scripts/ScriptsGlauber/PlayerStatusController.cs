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

    public Slider m_StaminBar;
    public Slider m_HungryBar;
    public Slider m_ThirstyBar;
    public Slider m_HappinessBar;

    public GameObject m_Player;

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
            m_ThirstRate = 0.02f;
            m_HungryRate = 0.01f;
            m_Stamin += m_StaminIncrementRate;
        }
        else
        {
            m_ThirstRate = 0.01f;
            m_HungryRate = 0.005f;
        }
    }
}
