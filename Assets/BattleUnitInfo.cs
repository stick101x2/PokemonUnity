using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleUnitInfo : MonoBehaviour
{
    [SerializeField] HPBar hpbar;

    [SerializeField] TextMeshProUGUI p_name;
    [SerializeField] TextMeshProUGUI lvl;

    const string male = "♂";
    const string female = "♀";
    Animator anim;
    int anim_idle = Animator.StringToHash("idle");
    int anim_static = Animator.StringToHash("static");
    int anim_damage = Animator.StringToHash("damage");
    int anim_faint = Animator.StringToHash("faint");

    

    private void Awake()
    {
        hpbar = GetComponentInChildren<HPBar>();
        anim = GetComponent<Animator>();
    }

    public void SetData(Pokemon pokemonData)
    {
        hpbar.Setup(pokemonData.MaxHP, pokemonData.HP);
        string gender = "";
        if (pokemonData.gender == 0)
            gender = male;
        else if (pokemonData.gender == 1)
            gender = female;
        p_name.text = pokemonData.Name.ToUpper() + gender;
        lvl.text = "_" + pokemonData.Level;
    }
    public void AnimEnter(bool player)
    {
        string s = player ? "p" : "e";
        anim.Play("intro_" + s + "_enter", 0, 0);

    }
    public void AnimIntro(bool player)
    {
        string s = player ? "p" : "e";
        anim.Play("intro_" + s, 0, 0);

    }
    public void AnimIdle()
    {
        anim.Play(anim_idle,0,0);
    }
    public void AnimStatic()
    {
        anim.Play(anim_static,0,0);
    }
    public void AnimDamage()
    {
        anim.Play(anim_damage, 0, 0);
    }
    public void AnimFaint()
    {
        anim.Play(anim_faint, 0, 0);
    }
    public HPBar HealthBar { get { return hpbar; } }
}
