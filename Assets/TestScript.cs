using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : SerializedMonoBehaviour
{
    public int[,] LabledTable = new int[2, 2];

    public int xLength;
    public int yLength;

    public int Index;

    [Button]
    public void ButtonForTesting()
    {
        xLength = LabledTable.GetLength(0);
        yLength = LabledTable.GetLength(1);
    }

    [Button]
    public void Button2()
    {
        Index = LabledTable[2,2];
    }
}

