using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Area : MonoBehaviour
{
    public EncounterTable encounters;
    Tilemap bounds;
    // Start is called before the first frame update
    void Start()
    {
        bounds = GetComponentInChildren<Tilemap>();
        bounds.CompressBounds();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
