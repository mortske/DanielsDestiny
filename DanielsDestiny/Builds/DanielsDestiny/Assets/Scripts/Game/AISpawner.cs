using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AISpawner : MonoBehaviour 
{
    BoxCollider collider;
    public int maxMonstersInArea;
    public float maxSpawnTime;
    public float minSpawnTime;
    public GameObject[] monsterPrefabs;
    public List<GameObject> myMonsters;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
        for (int i = 0; i < maxMonstersInArea; i++)
        {
            SpawnMonster();
        }
    }

    void SpawnMonster()
    {
        float spawnX = Random.Range(transform.position.x - collider.size.x,transform.position.x + collider.size.x);
        float spawnZ = Random.Range(transform.position.z - collider.size.z,transform.position.z + collider.size.z);
        GameObject newMonster = (GameObject)Instantiate(monsterPrefabs[Random.Range(0, monsterPrefabs.Length)],
                                                        new Vector3(spawnX, transform.position.y, spawnZ),
                                                        Quaternion.identity);
        newMonster.transform.parent = transform;
        myMonsters.Add(newMonster);
        newMonster.GetComponent<AnimalAI>().mySpawner = this;
    }

    public void KillMonster(GameObject monster)
    {
        myMonsters.Remove(gameObject);
        Invoke("SpawnMonster", Random.Range(minSpawnTime, maxSpawnTime));
    }
}
