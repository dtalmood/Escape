using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCarPartSpawns : MonoBehaviour
{
    public GameObject carPart1;
    // public GameObject carPart2;
    // public GameObject carPart3;
    // public GameObject carPart4;

    int randomNum = 0;

    // get random num from 0 to 3
    private int RandomValue()
    {
        return Random.Range(0, 3);
    }

    // Start is called before the first frame update
    void Start()
    {
        // stores random num from 0 to 2
        randomNum = RandomValue();
        if (randomNum == 0)
        {
            Debug.Log("0");
            Vector3 randomSpawnPosition = new Vector3(191.100006f, 7.22800016f, 652.599976f);
            Instantiate(carPart1, randomSpawnPosition, Quaternion.identity);
        }
        else if (randomNum == 1) 
        {
            Debug.Log("1");
            Vector3 randomSpawnPosition = new Vector3(193.100006f, 7.22800016f, 650.599976f);
            Instantiate(carPart1, randomSpawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("2");
            Vector3 randomSpawnPosition = new Vector3(195.100006f, 7.22800016f, 659.599976f);
            Instantiate(carPart1, randomSpawnPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
