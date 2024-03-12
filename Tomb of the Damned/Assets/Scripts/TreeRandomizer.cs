using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRandomizer : MonoBehaviour
{
    public bool Apply;

    public void OnValidate()
    {
        if(Application.isPlaying)
        {
            return;
        }

        if(Apply)
        {
            Apply = false;
            SetAll();
        }
    }

    public void SetAll()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        for(int i = 0; i < transforms.Length; i++)
        {
            if(transforms[i] == this.transform)
            {
                continue;
            }

            SetRandomValues(transforms[i]);
        }
    }

    public void SetRandomValues(Transform t)
    {
        float randomYRotation = t.rotation.y + Random.Range(-100f, 100f);
        t.rotation = Quaternion.Euler(t.rotation.x, randomYRotation, t.rotation.z);

        float randomScale = Random.Range(0.98f, 1.02f);
        t.localScale = Vector3.one * randomScale;
    }
}
