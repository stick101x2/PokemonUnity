using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    public static WorldMap instance;

    public LayerMask GrassLayer;

    private void Awake()
    {
        instance = this;
    }
}
