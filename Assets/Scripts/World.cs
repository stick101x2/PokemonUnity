using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    public static World instance;
    [SerializeField] Animator screenEffect;
    [SerializeField] LayerMask GrassLayer;

    [SerializeField] Tilemap walkable;
    [SerializeField] Tilemap grass;
    public enum BattleTransition
    {
        WildWeak,
        WildStrong,
    }
    private void Awake()
    {
        instance = this;
    }
    public static LayerMask GetGrassLayer()
    {
        return instance.GrassLayer;
    }
    public static void Fade(bool fadeIn)
    {
        if (fadeIn) instance.screenEffect.Play("fade_in");
        else instance.screenEffect.Play("fade_out");

    }
    public static bool IsGrass(Vector2 position)
    {
        Vector3Int globalToLocal = instance.grass.WorldToCell(position);
        TileBase tile = instance.grass.GetTile(globalToLocal);

        if (tile == null) return false;

        return true;

    }
    public static float GetTileHeight(Vector2 position)
    {
        Vector3Int globalToLocal = instance.walkable.WorldToCell(position);
        TileBase tile = instance.walkable.GetTile(globalToLocal);

        if (tile == null) return float.NaN;

        Matrix4x4 setMatrix = instance.walkable.GetTransformMatrix(globalToLocal);

        Vector4 col = setMatrix.GetColumn(3);
        Vector3 offset = (Vector3)col;

        return offset.z;
    }
    public static void DoBattleTransition(BattleTransition type)
    {
        switch(type)
        {
            case BattleTransition.WildWeak:
                instance.screenEffect.Play("wild_weak");
                break;
            case BattleTransition.WildStrong:
                instance.screenEffect.Play("wild_strong");
                break;
            default:
                break;
        }
    }
    public static void SetActive(bool active)
    {
        instance.gameObject.SetActive(active);
    }
}
