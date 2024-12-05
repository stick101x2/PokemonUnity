using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HPBar : MonoBehaviour
{
    [Tooltip("Health and Number Value will change Every (The value you set) Frames")]
    [Range(1,60)]
    [SerializeField] float changeSpeed = 1;

    [SerializeField] Image hpbar;
    [SerializeField] TextMeshProUGUI text;

    bool changeHPBar;
    bool left;

    float currentSmooth;

    int targetValue;
    int current;
    int target;
    int max;
    int frameCount;
    // Start is called before the first frame update
    void Awake()
    {
        hpbar = GetComponent<Image>();
        if(!text)
        text = GetComponentInChildren<TextMeshProUGUI>();
        enabled = false;
    }
    public void Setup(int MaxHP, int currentHP)
    {
        max = MaxHP;
        current = currentHP;
        currentSmooth = current;

        hpbar.fillAmount = GetHpNormalized(currentHP);
        SetText(currentHP);
    }

    public void ChangeHPBar(int s_targetValue,bool changeInstantly = false)
    {
        int s_value = Mathf.Clamp(s_targetValue, 0, 100);

        if (changeInstantly)
        {
            hpbar.fillAmount = GetHpNormalized(s_value);
            current = s_value;
            currentSmooth = current;
            SetText(s_value);
        }
        else
        {
            targetValue = s_value;
            if (targetValue > current)
                left = false;
            else
                left = true;

            changeHPBar = true;
            enabled = true;
        }
        //text.text = value;
    }

    private void Update()
    {
        if(left)
        {

            currentSmooth -= Time.deltaTime * changeSpeed;
            hpbar.fillAmount = currentSmooth/max;

            current = Mathf.RoundToInt(currentSmooth);
            SetText(current);
            if (currentSmooth < targetValue)
            {
                HPChangeEnd();
            }
        }else
        {

            currentSmooth += Time.deltaTime * changeSpeed;
            hpbar.fillAmount = currentSmooth / max;

            current = Mathf.RoundToInt(currentSmooth);
            SetText(current);
            if (current > targetValue)
            {
                HPChangeEnd();
            }
        }
        
    }
    /*
     * if (Time.frameCount % changeEveryXFrames == 0)
            {
                current -= 1;
            }
            hpbar.fillAmount = GetHpNormalized(current);
            SetText(current);
            if (current > targetValue)
            {
                HPChangeEnd();
            }
     */

    float GetHpNormalized(int current)
    {
        float value = (float)current / max;
        //Debug.Log(value);
        return value;
        
    }
    void SetText(int value)
    {
        if (text == null)
            return;
        text.text = "" + value + "/" + max;
    }
    void HPChangeEnd()
    {
        current = targetValue;
        currentSmooth = current;
        hpbar.fillAmount = GetHpNormalized(current);
        SetText(current);
        changeHPBar = false;
        enabled = false;
    }

}
