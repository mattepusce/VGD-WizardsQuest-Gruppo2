using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    List<Transform> Spawn = new List<Transform>();
    private int count=9;
    private bool CanSpawn=true;
    private float spawnTime = 5f;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnpoints = GameObject.FindGameObjectWithTag("Spawnpoints");   //Trova i figli del parent con tag "Spawnpoints"
        foreach (Transform t in spawnpoints.transform)
            Spawn.Add(t);                   //Aggiunge gli oggetti trovati nella lista
    }

    private void Update()
    {
        //Debug.Log("nemici spawn rimasti:" + count);
        if(KillCounter.killcount<=11)
        {
            if (count <= 9 && count > 0)
            {

                if (CanSpawn)
                {
                    //Spawna i nemici randomicamente nei 3 spawnpoints della mappa
                    Instantiate(enemyPrefab, Spawn[Random.Range(0, Spawn.Count)].position, Spawn[Random.Range(0, Spawn.Count)].rotation);
                    
                    StartCoroutine(SpawnTimer());
                    count--;
                }
            }
        }
    }

    IEnumerator SpawnTimer()      //aggiunge un cooldown allo spawn dei nemici
    {
        CanSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        CanSpawn = true;
    }
}
