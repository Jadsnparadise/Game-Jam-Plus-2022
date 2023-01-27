using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Game.Map
{
    
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] List<Spawn> objectsToSpawn;
        [SerializeField] Vector2Int startPoint; 
        [SerializeField] Vector2Int endPoint;
        void Start()
        {
            foreach (Spawn s in objectsToSpawn)
            {
                for (int x = startPoint.x; x < endPoint.x; x += UnityEngine.Random.Range(s.spawnIntervalMinimun.x, s.spawnIntervalMaximun.x))
                {
                    for (int y = startPoint.y; y < endPoint.y; y += UnityEngine.Random.Range(s.spawnIntervalMinimun.x, s.spawnIntervalMaximun.x))
                    {
                        int spawnChance = UnityEngine.Random.Range(0, 100);
                        if (spawnChance > s.density)
                        {
                            continue;
                        }
                        Spawn(s.objects[UnityEngine.Random.Range(0, s.objects.Count)], s.parent, new (x, y));
                    }
                }
            }
        }

        void Spawn(GameObject _obj, GameObject _parent, Vector2 _pos)
        {
            //Instantiate(_obj, _pos, Quaternion.identity);
            Instantiate(_obj, _pos, Quaternion.identity, _parent.transform);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

    [Serializable]
    public class Spawn
    {
        public GameObject parent;
        public List<GameObject> objects;
        [Range(0, 100)] public int density = 50;
        [Min(10)] public Vector2Int spawnIntervalMinimun = Vector2Int.one;
        [Min(10)] public Vector2Int spawnIntervalMaximun = Vector2Int.one;
        //public List<Vector2> offSpawn;
    }
}