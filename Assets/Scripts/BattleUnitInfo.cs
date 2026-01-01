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
    [SerializeField] Image genderIcon;
    [Space(5)]
    [SerializeField] GameObject bgFull;
    [SerializeField] GameObject bgSmall;
    [SerializeField] GameObject extra;



    Animator anim;
    int anim_idle = Animator.StringToHash("idle");
    int anim_static = Animator.StringToHash("static");
    int anim_damage = Animator.StringToHash("damage");
    int anim_faint = Animator.StringToHash("faint");


    bool showAllinfo = true;
    private void Awake()
    {
        hpbar = GetComponentInChildren<HPBar>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            showAllinfo = !showAllinfo;
            ShowAllInfo(showAllinfo);
        }
    }
    public void ShowAllInfo(bool show)
    {
        if (bgFull == null || bgSmall == null || extra == null)
            return;

        if(show)
        {
            extra.SetActive(true);
            bgFull.SetActive(true);
            bgSmall.SetActive(false);
            return;
        }
        extra.SetActive(false);
        bgFull.SetActive(false);
        bgSmall.SetActive(true);
    }
    public void SetData(Pokemon pokemonData,BattleManager battleManager)
    {
        hpbar.Setup(pokemonData.MaxHP, pokemonData.HP);

        genderIcon.sprite = battleManager.GetGenderIcon(pokemonData.Gender);
        p_name.text = pokemonData.Name;
        lvl.text = ""+ pokemonData.Level;
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
