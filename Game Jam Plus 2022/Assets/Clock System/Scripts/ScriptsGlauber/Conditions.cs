using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Conditions : MonoBehaviour
{
    public RectTransform prefab;    
    public RectTransform content;
    public ScrollRect scrollView;

    public bool m_Cold  = false;
    public bool m_Hot = false;
    public bool m_Drunk = false;
    public bool m_Poison = false;
    public bool m_Chapado = false;

    public List<Lista> m_Lista = new List<Lista>();

    public List<string> m_Itens = new List<string>();

    public List<Sprite> m_SpritesList = new List<Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        //ShowCondition();
        //ShowCondition();
    }

    // Update is called once per frame
    void Update()
    {
       CheckConditions();

    }
    public void ShowCondition()
    {
        if(content.childCount < m_Itens.Count)
        {
            DestroyItenList();
            foreach (string i in m_Itens)
            {
                Debug.Log("O que tem em I: "+ i);
                var instance = GameObject.Instantiate(prefab.gameObject, transform.position, Quaternion.identity) as GameObject;
                instance.GetComponentInChildren<Text>().text = i;                
                instance.transform.SetParent(content, false);
            }
        }
        
        
    }

    public void DestroyItenList()
    {
        foreach(Transform child in content)
        {
            Destroy(child);
        }
    }
    public void DestroyPrafab(string name)
    {
        var Itens = GameObject.FindGameObjectsWithTag("TextIcon");
        foreach(GameObject i in Itens)
        {
            if(i.GetComponentInChildren<Text>().text == name)
            {
                Destroy(i);
            }
            //Debug.Log(i.GetComponent<Text>().text);
        }
    }
    public void CheckConditions()
    {
        //Add na lista
        if(m_Cold == true && !m_Itens.Contains("Cold"))
        {
            m_Itens.Add("Cold");
        }
        if (m_Hot == true && !m_Itens.Contains("Hot"))
        {
            m_Itens.Add("Hot");
        }
        if (m_Drunk == true && !m_Itens.Contains("Drunk"))
        {
            m_Itens.Add("Drunk");
        }
        if (m_Poison == true && !m_Itens.Contains("Poison"))
        {
            m_Itens.Add("Poison");
        }
        if (m_Chapado == true && !m_Itens.Contains("Chapado"))
        {
            m_Itens.Add("Chapado");
        }

        //Retirar da lista
        if (m_Cold == false && m_Itens.Contains("Cold"))
        {
            m_Itens.Remove("Cold");
        }
        if (m_Hot == false && m_Itens.Contains("Hot"))
        {
            m_Itens.Remove("Hot");
        }
        if (m_Drunk == false && m_Itens.Contains("Drunk"))
        {
            m_Itens.Remove("Drunk");
        }
        if (m_Poison== false && m_Itens.Contains("Poison"))
        {
            m_Itens.Remove("Poison");
        }
        if (m_Chapado == false && m_Itens.Contains("Chapado"))
        {
            m_Itens.Remove("Chapado");
        }

        PopularLista();

    }
    public void PopularLista()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
            m_Lista.Clear();
        }

        for (int i = 0; i < m_Itens.Count; i++)
        {
            m_Lista.Add(new Lista(m_Itens[i]));            
        }
        ShowConditions();

    }
    public void ShowConditions()
    {
        foreach (var model in m_Lista)
        {

            
            prefab.transform.Find("textShow").GetComponent<Text>().text = model.m_NameCondition;
            if(model.m_NameCondition == "Cold")
            {
                prefab.GetComponent<Image>().sprite = m_SpritesList[0];
            }
            if (model.m_NameCondition == "Hot")
            {
                prefab.GetComponent<Image>().sprite = m_SpritesList[1];
            }
            if (model.m_NameCondition == "Drunk")
            {
                prefab.GetComponent<Image>().sprite = m_SpritesList[2];
            }
            if (model.m_NameCondition == "Poison")
            {
                prefab.GetComponent<Image>().sprite = m_SpritesList[3];
            }
            if (model.m_NameCondition == "Chapado")
            {
                prefab.GetComponent<Image>().sprite = m_SpritesList[4];
            }



            var instance = GameObject.Instantiate(prefab.gameObject, transform.position, Quaternion.identity) as GameObject;
            instance.transform.SetParent(content, false);

        }
    }
}
