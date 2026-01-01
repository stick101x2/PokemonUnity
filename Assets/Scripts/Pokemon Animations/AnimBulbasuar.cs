using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBulbasuar : PokemonAnimationBase
{
    [SerializeField] float duration = 0.25f;
    [SerializeField] float yHeight = 1.25f;
    [SerializeField] Ease ease = Ease.InBounce;
    public override IEnumerator Animation(SpriteHandler sprite)
    {
        Transform transform = sprite.transform;

        transform.DOScaleY(yHeight, duration).SetEase(ease);
        transform.DOScaleY(1f, duration).SetEase(Ease.Linear).SetDelay(duration);

        yield return new WaitForSeconds(duration * 0.25f);

        sprite.StartCoroutine(Animation3_2(transform));
        sprite.FlipSpriteIndex();

        yield return new WaitForSeconds(duration * 2f);

        sprite.SetSpriteIndex(0);
    }

    IEnumerator Animation3_2(Transform transform)
    {
        Tween moveTween = null;
        float oneHalfShakeDuration = 0.05f;
        int shakeTimes = 4;

        for (int i = 0; i < shakeTimes; i++)
        {
            moveTween = transform.DOMoveY(0.1f, oneHalfShakeDuration).SetRelative(true).SetEase(Ease.Linear);
            yield return moveTween.WaitForCompletion();

            moveTween = transform.DOMoveY(-0.1f, oneHalfShakeDuration).SetRelative(true).SetEase(Ease.Linear);
            yield return moveTween.WaitForCompletion();
        }
    }
}
