using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SpriteHandler : MonoBehaviour
{
    
    [SerializeField] Palette spritePalette;
    [SerializeField] Sprite[] spriteArray;
    [SerializeField] PokemonAnimationBase anim;

    int index = 0;

    SpriteRenderer render;
    Material spriteMaterial;

    Vector3 defualtPos;
    Quaternion defualtRot;
    Vector3 defualtScale;
    public SpriteRenderer Sprite { get { return render; } }

    public void Setup(Sprite[] sprites,Palette palette,PokemonAnimationBase animation)
    {
        render = GetComponent<SpriteRenderer>();
        spriteMaterial = render.material;

        defualtPos = transform.localPosition;
        defualtScale = transform.localScale;
        defualtRot = transform.localRotation;

        SetSpriteArray(sprites);
        SetSpriteIndex(0);
        SetPaletteTextureRef(palette.palette);
        ApplyPalette(palette);
        SetAnimation(animation);
    }
    public void ResetTransform()
    {
        transform.localPosition = defualtPos;
        transform.localScale = defualtScale;
        transform.localRotation = defualtRot;
    }
    public void SetPaletteTextureRef(Texture2D paletteRef)
    {
        Texture2D palTex = paletteRef;
        spriteMaterial.SetTexture("_PaletteIn", palTex);
    }
    public void ApplyPalette(Palette palette)
    {
        palette.ApplyPaletteToMaterial(spriteMaterial);
    }

    public void SetSpriteArray(Sprite[] sprites)
    {
        spriteArray = sprites;
    }
    public void FlipSpriteIndex()
    {
        index = 1 - index;

        render.sprite = spriteArray[index];
    }

    public void SetSpriteIndex(int value)
    {
        index = value;
        render.sprite = spriteArray[index];
    }

    public IEnumerator ChangeSpriteAfter(int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            yield return null;
        }

        FlipSpriteIndex();
    }

    public void SetAnimation(PokemonAnimationBase pokeAnimation)
    {
        anim = Instantiate(pokeAnimation);
    }
    public void PlayAnimation()
    {
        SetSpriteIndex(0);
        //DOTween.KillAll();
        StopAllCoroutines();
        ResetTransform();

        StartCoroutine(anim.Animation(this));
    }

}
