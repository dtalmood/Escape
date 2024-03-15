using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTerrain : MonoBehaviour
{
    public Terrain terrain; // Reference to your terrain

    private void Start()
    {
        // Ensure terrain reference is set
        if (terrain == null)
        {
            Debug.LogError("Terrain reference not set!");
            return;
        }

        // Loop through each child of the "Tree" GameObject
        foreach (Transform child in transform)
        {
            // Get the position of the tree in world space
            Vector3 treePosition = child.position;

            // Calculate the corresponding height of the terrain at the tree's position
            float terrainHeight = terrain.SampleHeight(treePosition);

            // Set the Y position of the tree to match the terrain height
            treePosition.y = terrainHeight;

            // Update the tree's position
            child.position = treePosition;
        }
    }
}