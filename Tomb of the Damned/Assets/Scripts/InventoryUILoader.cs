using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryUILoader : MonoBehaviour
{
    [Tooltip("When this key is pressed, the inventory UI will toggle open or closed.")]
    public KeyCode inventoryInput;
    public SceneReference sceneReference;
    public bool loaded = false;
    public bool loading = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inventoryInput))
        {
            if (!loaded && !loading)
            {
                loading = true;
                AsyncOperation op = SceneManager.LoadSceneAsync(sceneReference, LoadSceneMode.Additive);
                op.completed += (result) =>
                {
                    loading = false;
                    loaded= true;
                };
            }
            else if(loaded && !loading)
            {
                loading = true;
                AsyncOperation op = SceneManager.UnloadSceneAsync(sceneReference);
                op.completed += (result) =>
                {
                    loading = false;
                    loaded = false;
                };
            }
        }
    }
}
