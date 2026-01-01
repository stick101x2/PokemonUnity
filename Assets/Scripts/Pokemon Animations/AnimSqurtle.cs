using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AnimSqurtle : PokemonAnimationBase
{
    public override IEnumerator Animation(SpriteHandler sprite)
    {
        UnityEngine.Transform transform = sprite.transform;
        for (int i = 0; i < 3; i++)
        {
            yield return sprite.ChangeSpriteAfter(20);
        }
        yield return new WaitForSeconds(0.125f);
        Tween jumpTween = transform.DOLocalJump(transform.localPosition, 0.5f, 3, 0.5f).SetEase(Ease.Linear);
        yield return jumpTween.WaitForCompletion();
        yield return new WaitForSeconds(0.125f);
        sprite.SetSpriteIndex(0);
    }
}
