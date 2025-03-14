using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "Palette", menuName = "MMX/Palette")]
public class Palette : ScriptableObject
{
    [InlineButton("GetColors")]
    public Texture2D palette;
    [InlineButton("ApplyPalette")]
    public Material material;
    [Space(10)]
    public Color[] colors = new Color[15];
    [Button]
    public void GetAndApplyColors()
    {
        GetColors();
        ApplyPalette();
    }
    public Color[] GetColorsArray()
    {
        return GetColorsArray(palette);
    }
    public Color[] GetColorsArray(Texture2D tex)
    {
        Color[] c = new Color[0];
        if (tex == null)
        {
            Debug.LogWarning("ScrpiableObject (" + name + ") Palette input is missing. Please apply texture");
            return c;
        }
        c = new Color[tex.width];
        for (int i = 0; i < tex.width; i++)
        {
            c[i] = tex.GetPixel(i, 0);
        }
        return c;
    }

    public void ApplyPaletteToMaterial(Material mat)
    {
        ApplyPaletteToMaterial(mat, colors);
    }
    public void ApplyPaletteToMaterial(Material mat, Color[] palette)
    {
        for (int i = 0; i < palette.Length; i++)
        {
            if (!mat.HasColor("_Color" + i))
                return;
            mat.SetColor("_Color" + i, palette[i]);
        }
    }

    void ApplyPalette()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            if (!material.HasColor("_Color" + i))
                return;
            material.SetColor("_Color" + i, colors[i]);
        }
    }
    void GetColors()
    {
        if (palette == null)
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
