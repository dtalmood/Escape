using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart4Spawn : MonoBehaviour
{
    public GameObject carPart4;

    int randomNum = 0;

    // get random num from 0 to 3
    private int RandomValue()
    {
        return Random.Range(0, 3);
    }

    // Start is called before the first frame update
    void Start()
    {
        // stores random num from 0 to 2 - 0, 1, 2
        randomNum = RandomValue();
        if (randomNum == 0)
        {
            Debug.Log("0 - part4");
            Vector3 randomSpawnPosition4 = new Vector3(310.255f, 3.516f, 548.685f);
            Instantiate(carPart4, randomSpawnPosition4, Quaternion.Euler(100f, 0f, 0f));
        }
        else if (randomNum == 1)
        {
            Debug.Log("1 - part4");
            Vector3 randomSpawnPosition4 = new Vector3(309.883f, 3.516f, 550.523f);
            Instantiate(carPart4, randomSpawnPosition4, Quaternion.Euler(100f, 0f, 0f));
        }
        else
        {
            Debug.Log("2 - part4"); ;
            Vector3 randomSpawnPosition4 = new Vector3(307.217f, 2.605f, 549.3f);
            Instantiate(carPart4, randomSpawnPosition4, Quaternion.Euler(100f, 0f, 0f));
        }
    }
}
