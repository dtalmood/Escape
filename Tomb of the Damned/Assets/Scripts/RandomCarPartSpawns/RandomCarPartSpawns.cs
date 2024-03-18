using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCarPartSpawns : MonoBehaviour
{
    public GameObject carPart1;

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
            Debug.Log("0");
            // Vector3 randomSpawnPosition = new Vector3(191.100006f, 7.22800016f, 652.599976f);
            Vector3 randomSpawnPosition = new Vector3(310.5302f, 3.440027f, 555.7f);
            Instantiate(carPart1, randomSpawnPosition, Quaternion.identity);
        }
        else if (randomNum == 1) 
        {
            Debug.Log("1");
            // Vector3 randomSpawnPosition = new Vector3(193.100006f, 7.22800016f, 650.599976f);
            Vector3 randomSpawnPosition = new Vector3(308.084f, 2.606f, 556.929f);
            Instantiate(carPart1, randomSpawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("2");
            // Vector3 randomSpawnPosition = new Vector3(195.100006f, 7.22800016f, 659.599976f);
            Vector3 randomSpawnPosition = new Vector3(312.75f, 2.438f, 558.71f);
            Instantiate(carPart1, randomSpawnPosition, Quaternion.identity);
        }
    }
}
