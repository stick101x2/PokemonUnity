using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class World : MonoBehaviour
{
    public static World instance;
    [SerializeField] Animator screenEffect;
    [SerializeField] LayerMask GrassLayer;
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
