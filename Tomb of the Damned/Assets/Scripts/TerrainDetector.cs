using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    The LayerMask.GetMask("Terrain") function in Unity is used to create a layer mask that represents a single layer 
    or a combination of layers. This layer mask is then used in raycasting operations, such as with the Physics.Raycast 
    method, to filter which objects or colliders the ray should interact with.
*/


public class TerrainDetector : MonoBehaviour
{
    // Reference to the terrain
    public Terrain terrain;

    void Update()
    {
        testingFunction();
        detectTerrain();
        
        
    }

    private void testingFunction()
    {
        // Debug.Log(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Terrain")));
        // Debug.Log("Layer Mask: "+LayerMask.GetMask("whatIsGround"));
        // Debug.Log("Vector3 Down =  "+ Vector3.down);
        //Debug.Log();
        // if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("whatIsGround")))
        // {
        //     //Debug.Log("Hit Point: " + hit.point);
        //     Debug.Log("Detected Layer: " + hit.collider.gameObject.layer);
        //     // ... rest of the script
        // }
        
    }

    private void detectTerrain()
    {
        //Cast a ray from the player's position downward to detect the terrain
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("whatIsGround")))
        {
            // Get the terrain texture coordinates
            Vector3 terrainCoords = hit.textureCoord;
            //Debug.Log("hit.point = "+hit.point);
            Debug.Log("hit.point = "+hit.textureCoord);
            
            // Get the alpha map values at the coordinates
            float[,,] alphaMapData = terrain.terrainData.GetAlphamaps(
                Mathf.FloorToInt(terrainCoords.x * terrain.terrainData.alphamapWidth),
                Mathf.FloorToInt(terrainCoords.z * terrain.terrainData.alphamapHeight),
                1, 1
            );

            // Find the dominant texture based on alpha values
            int dominantTextureIndex = 0;
            float maxAlpha = 0f;

            for (int i = 0; i < alphaMapData.GetLength(2); i++)
            {
                if (alphaMapData[0, 0, i] > maxAlpha)
                {
                    maxAlpha = alphaMapData[0, 0, i];
                    dominantTextureIndex = i;
                }
            }
        
            // Print the dominant terrain type
            //Debug.Log("Running");
            //Debug.Log("Player is on terrain type: " + terrain.terrainData.terrainLayers[dominantTextureIndex].name);
        }
    }
}