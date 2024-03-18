using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPart3Spawn : MonoBehaviour
{
    public GameObject carPart3;

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
            Debug.Log("0 - part3");
            Vector3 randomSpawnPosition3 = new Vector3(309.606f, 2.654f, 546.614f);
            Instantiate(carPart3, randomSpawnPosition3, Quaternion.identity);
        }
        else if (randomNum == 1)
        {
            Debug.Log("1 - part3");
            Vector3 randomSpawnPosition3 = new Vector3(311.441f, 3.561f, 548.035f);
            Instantiate(carPart3, randomSpawnPosition3, Quaternion.identity);
        }
        else
        {
            Debug.Log("2 - part3");;
            Vector3 randomSpawnPosition3 = new Vector3(310.143f, 2.784f, 543.611f);
            Instantiate(carPart3, randomSpawnPosition3, Quaternion.identity);
        }
    }
}
