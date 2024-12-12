using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "Palette", menuName = "MMX/Palette")]
public class Palette : ScriptableObject
{
    public Texture2D palette;
    public Color[] colors = new Color[15];

    [Button("Create Colors From Palette")]
    public void GetColors()
    {
        if(palette == null)
        {
            Debug.LogWarning("ScrpiableObject (" + name + ") Palette input is missing. Please apply texture");
            return;
        }
        colors = new Color[palette.width];
        for (int i = 0; i < palette.width; i++)
        {
            colors[i] = palette.GetPixel(i, 0);
        }
    }
    

}
