using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities 
{
    public static Probability GetWeightedProbability(Probability[] probabilities)
    {
        Range[] ranges = new Range[probabilities.Length];

        int max = 0;

        for (int i = 0; i < probabilities.Length; i++)
        {
            ranges[i].min = max;
            max += probabilities[i].probability;
            ranges[i].max = max;
        }

        int random = Random.Range(0, max + 1);

        for (int i = 0; i < ranges.Length; i++)
        {
            if (ranges[i].IsInsideRange(random))
                return probabilities[i];           
        }
        Debug.LogError("Probability Failed");
        return null;
    }
}
public struct Range
{
    public int min;
    public int max;

    public bool IsInsideRange(int value)
    {
        if (value == max && value == min)
            return false;
        if (min == 0 && value == 0)
            return true;
        
        if(value > min && value <= max)
            return true;
        return false;
    }
}
[System.Serializable]
public class Probability
{
    public int probability = 0;
}