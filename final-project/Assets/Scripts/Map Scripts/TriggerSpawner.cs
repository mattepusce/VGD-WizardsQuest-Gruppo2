using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {

    }
    IEnumerator EnemyDrop()
    {
        Instantiate(enemy, new Vector3(), Quaternion.identity);
        Instantiate(enemy, new Vector3(), Quaternion.identity);
        Instantiate(enemy, new Vector3(), Quaternion.identity);

        yield return new WaitForSeconds(0.1f);


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(EnemyDrop());
            Destroy(gameObject);
        }
    }
}
