using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
    The LayerMask.GetMask("Terrain") function in Unity is used to create a layer mask that represents a single layer 
    or a combination of layers. This layer mask is then used in raycasting operations, such as with the Physics.Raycast 
    method, to filter which objects or colliders the ray should interact with.
*/

/*
    How dynamic sound should work 
    1. Whenever the player is in the walking, running, crouch walking, 
*/ 

public class TerrainDetector : MonoBehaviour
{
    // Reference to the terrain
    public Terrain terrain;
    
    private void testingFunction()
    {
        //Debug.Log(transform.position);
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
            //Debug.Log("hit.point = "+hit.textureCoord);
            
            // Get the alpha map values at the coordinates
            float[,,] alphaMapData = terrain.terrainData.GetAlphamaps(
                Mathf.FloorToInt(terrainCoords.x * terrain.terrainData.alphamapWidth),
                Mathf.FloorToInt(terrainCoords.z * terrain.terrainData.alphamapHeight),
                1, 1
            );

            // Find the dominant texture based on alpha values
            int dominantTextureIndex = 0;
            float maxAlpha = 0f;

            for (int i = 0; i < alphaMapData.GetLength(1); i++)
            {
                
                if (alphaMapData[0, 0, i] > maxAlpha)
                {
                    maxAlpha = alphaMapData[0, 0, i];
                    dominantTextureIndex = i;
                }
            }
            //Debug.Log("Alpha Map Data Sand: "+alphaMapData[0, 0, 0]);
            //Debug.Log("Alpha Map Data BS: "+alphaMapData[0, 0, 1]);

            
            Debug.Log("Player is on terrain type: " + terrain.terrainData.terrainLayers[dominantTextureIndex].name);
        }
    }

    private float[] getTextureMix()
    {
        Vector3 tpos = terrain.transform.position;
        TerrainData tData = terrain.terrainData;
        int mapX = Mathf.RoundToInt((transform.position.x - tpos.x) / tData.size.x * tData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((transform.position.z - tpos.z) / tData.size.z * tData.alphamapHeight);
        float[,,] splatMapData = tData.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellmix = new float[splatMapData.GetUpperBound(2)+1];
        for(int i = 0; i < cellmix.Length; i++)
        {
            cellmix[i] = splatMapData[0,0,i];
        }
        // Debug.Log("cellmix[1] = " + cellmix[0]);
        // Debug.Log("cellmix[2] = " + cellmix[1]);

        return cellmix;        
    }

    public string getLayerName()
    {
        float [] cellMix = getTextureMix();
        float strongest = 0;
        int maxIndex = 0;
        
        for(int i = 0; i < cellMix.Length; i++)
        {
            if(cellMix[i] > strongest)
            {
                maxIndex = i;
                strongest = cellMix[i];
            }
        }

        //Debug.Log("Terrain = "+terrain.terrainData.terrainLayers[maxIndex].name);
        return terrain.terrainData.terrainLayers[maxIndex].name;
    }
}