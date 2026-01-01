using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AnimCharmander : PokemonAnimationBase
{
    public override IEnumerator Animation(SpriteHandler sprite)
    {
        UnityEngine.Transform transform = sprite.transform;
        float speedMod = 1.5f;

        Tween jumpTween = null;
        float jumpHalfDuration = 0.0833f;
        jumpHalfDuration *= speedMod;
        jumpTween = transform.DOLocalMoveY(0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear);
        jumpTween = transform.DOLocalMoveY(-0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 1);

        jumpTween = transform.DOLocalMoveY(0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 2);
        jumpTween = transform.DOLocalMoveY(-0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 3);

        jumpTween = transform.DOLocalMoveY(0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 4);
        jumpTween = transform.DOLocalMoveY(-0.5f, jumpHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(jumpHalfDuration * 5);



        Tween moveTween = null;
        float moveHalfDuration = 0.125f;
        moveHalfDuration *= speedMod;

        float moveDistance = 0.75f;
        moveTween = transform.DOLocalMoveX(-moveDistance, moveHalfDuration).SetRelative(true).SetEase(Ease.Linear);
        moveTween = transform.DOLocalMoveX(moveDistance * 2, moveHalfDuration * 2).SetRelative(true).SetEase(Ease.Linear).SetDelay(moveHalfDuration);
        moveTween = transform.DOLocalMoveX(-moveDistance, moveHalfDuration).SetRelative(true).SetEase(Ease.Linear).SetDelay(moveHalfDuration + moveHalfDuration * 2);

        float animDuration = speedMod * 0.5f;
        float quarterStep = animDuration * 0.25f;

        sprite.FlipSpriteIndex();
        yield return new WaitForSeconds(quarterStep);
        sprite.FlipSpriteIndex();
        yield return new WaitForSeconds(quarterStep);
        sprite.FlipSpriteIndex();
        yield return new WaitForSeconds(quarterStep);
        sprite.FlipSpriteIndex();
        yield return new WaitForSeconds(quarterStep);

        sprite.SetSpriteIndex(0);
    }
}
