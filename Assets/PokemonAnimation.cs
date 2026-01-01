using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonAnimation : MonoBehaviour
{
    [SerializeField]PokemonBase PokemonData;
    [SerializeField]int test = 0;
    [SerializeField]int index = 0;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] bool play;
    [SerializeField] int animIndex;
    [SerializeField] Ease ease;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float duration = 0.5f;
    [SerializeField] float yHeight = 1.5f;
    [SerializeField] float strength = 0.1f;
    /*
    // Start is called before the first frame update
    void Start()
    {
        

        SetSpriteIndex(0);
        Material mat = sprite.material;
        Texture2D pal = PokemonData.NormalPalette.palette;
        mat.SetTexture("_PaletteIn", pal);
        PokemonData.NormalPalette.ApplyPaletteToMaterial(mat);

        PlayAnimation(1f);
    }
    private void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.T) || play)
        {
            play = false;
            PlayAnimation(0f);
        }
    }
   
    
    public void FlipSpriteIndex()
    {
        index = 1 - index;

        sprite.sprite= PokemonData.FrontSprites[index];
    }

    public void SetSpriteIndex(int value)
    {
        index = value;
        sprite.sprite = PokemonData.FrontSprites[index];
    }

    IEnumerator ChangeSpriteAfter(int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            yield return null;
        }

        FlipSpriteIndex();
    }
  
    IEnumerator Animation(float delay)
    {
        yield return new WaitForSeconds(delay);
        for (int i = 0; i < 3; i++)
        {
            yield return ChangeSpriteAfter(20);
        }
        yield return new WaitForSeconds(0.25f);
        Tween jumpTween = transform.DOLocalJump(transform.localPosition, 0.5f, 3, 0.5f).SetEase(Ease.Linear);
        yield return jumpTween.WaitForCompletion();
        SetSpriteIndex(0);
    }
    IEnumerator Animation2(float delay)
    {
        yield return new WaitForSeconds(delay);

        float speedMod = 1.5f;

        Tween jumpTween = null;
        float jumpHalfDuration = 0.0833f;
        jumpHalfDuration *= speedMod;
        jumpTween = transform.DOLocalMoveY(0.5f, jumpHalfDuration) .SetRelative(true).SetEase(Ease.Linear);
        jumpTween = transform.DOLocalMoveY(-0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 1);
        
        jumpTween = transform.DOLocalMoveY(0.5f, jumpHalfDuration) .SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 2);
        jumpTween = transform.DOLocalMoveY(-0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 3);

        jumpTween = transform.DOLocalMoveY(0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 4);
        jumpTween = transform.DOLocalMoveY(-0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 5);



        Tween moveTween = null;
        float moveHalfDuration = 0.125f;
        moveHalfDuration *= speedMod;

        float moveDistance = 0.75f;
        moveTween = transform.DOLocalMoveX(-moveDistance, moveHalfDuration).SetRelative(true).SetEase(Ease.Linear);
        moveTween = transform.DOLocalMoveX(moveDistance*2, moveHalfDuration*2).SetRelative(true).SetEase(Ease.Linear).SetDelay(moveHalfDuration);
        moveTween = transform.DOLocalMoveX(-moveDistance, moveHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(moveHalfDuration + moveHalfDuration*2);

        float animDuration = speedMod * 0.5f;
        float quarterStep = animDuration * 0.25f;

        FlipSpriteIndex();
        yield return new WaitForSeconds(quarterStep);
        FlipSpriteIndex();
        yield return new WaitForSeconds(quarterStep);
        FlipSpriteIndex();
        yield return new WaitForSeconds(quarterStep);
        FlipSpriteIndex();
        yield return new WaitForSeconds(quarterStep);

        SetSpriteIndex(0);
    }

    IEnumerator Animation3(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.DOScaleY(yHeight, duration).SetEase(ease);
        transform.DOScaleY(defualtScale.y, duration).SetEase(Ease.Linear).SetDelay(duration);
      
        yield return new WaitForSeconds(duration*0.25f);

        StartCoroutine(Animation3_2());
        FlipSpriteIndex();

        yield return new WaitForSeconds(duration * 2f);

        SetSpriteIndex(0);
    }       
    
    IEnumerator Animation3_2()
    {
        Tween moveTween = null;
        float oneHalfShakeDuration = 0.05f;
        int shakeTimes = 4;

        for (int i = 0; i < shakeTimes; i++)
        {
            moveTween = transform.DOMoveY(0.1f,  oneHalfShakeDuration).SetRelative(true).SetEase(Ease.Linear);
            yield return moveTween.WaitForCompletion();

            moveTween = transform.DOMoveY(-0.1f, oneHalfShakeDuration).SetRelative(true).SetEase(Ease.Linear);
            yield return moveTween.WaitForCompletion();
        }
    }
    
    */
}
