using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart2Spawn : MonoBehaviour
{
    public GameObject carPart2;

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
            Debug.Log("0 - part2");
            Vector3 randomSpawnPosition2 = new Vector3(310.913f, 3.251977f, 552.17f);
            Instantiate(carPart2, randomSpawnPosition2, Quaternion.identity);
        }
        else if (randomNum == 1)
        {
            Debug.Log("1 - part2");
            Vector3 randomSpawnPosition2 = new Vector3(309.088f, 2.231f, 550.782f);
            Instantiate(carPart2, randomSpawnPosition2, Quaternion.identity);
        }
        else
        {
            Debug.Log("2 - part2");
            Vector3 randomSpawnPosition2 = new Vector3(319.5f, 2.231f, 552.078f);
            Instantiate(carPart2, randomSpawnPosition2, Quaternion.identity);
        }
    }
}
