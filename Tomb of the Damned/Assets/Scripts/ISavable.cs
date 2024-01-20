using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    // Start is called before the first frame update
    public void Save();

    // Update is called once per frame
    public void Load();
}
