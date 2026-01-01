using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Pokemon Animation", menuName = "PokemonUE/Animation")]
public class PokemonAnimationBase : ScriptableObject
{
    public virtual IEnumerator Animation(SpriteHandler sprite)
    {
        yield break;
    }
}
