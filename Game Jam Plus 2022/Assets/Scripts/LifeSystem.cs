using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Game.StatusController
{
    public class LifeSystem : MonoBehaviour
    {
        [SerializeField] System.Attribute life;
        [SerializeField] DropType dropType;
        [SerializeField] List<Drop> drop;
        [SerializeField] GameObject dropGameObject;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Damage(int _damage)
        {
            life.DecreaseValue(_damage);
            if (life.CurrentValue <= life.MinValue)
            {
                Death();
            }
        }

        void Death()
        {
            switch (dropType)
            {
                case DropType.AllDrop:
                    foreach (Drop d in drop)
                    {
                        for (int i = 0; i < UnityEngine.Random.Range(d.minMaxItens.x, d.minMaxItens.y); i++)
                        {
                            Drop(d.itemDrop);
                        }
                    }
                    break;
                case DropType.RandomDrop:
                    int currentDrop = UnityEngine.Random.Range(0, drop.Count-1);
                    Drop(drop[currentDrop].itemDrop);
                    break;
            }
            Destroy(gameObject);
        }

        void Drop(Itens.ItemScriptable _itemDrop)
        {
            //ItemController i = Instantiate(dropGameObject, transform.position, transform.rotation);
            //i.SetItem(_itemDrop);
            Instantiate(dropGameObject, transform.position, transform.rotation);
        }
    }

    [Serializable]
    public enum DropType
    {
        Null, RandomDrop, AllDrop
    }

    [Serializable]
    public struct Drop
    {
        public Itens.ItemScriptable itemDrop;
        public Vector2Int minMaxItens;
    }
}