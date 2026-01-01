using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePositions : MonoBehaviour
{
    List<Transform> positions = new List<Transform>();
    // Start is called before the first frame update
    public void Setup()
    {
        positions.AddRange (transform.GetComponentsInChildren<Transform>());
        positions.RemoveAt(0);
    }

    public Vector3 GetPosition(int index)
    {
        return positions[index].localPosition;
    }
}
