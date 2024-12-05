using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textbox;
    [Range(1,120)]
    [SerializeField] int lettersPerSecond = 1;

    string sampleText = "Sample Text";
    // Start is called before the first frame update
    public void SetDialog(string dialog)
    {
        textbox.text = dialog;
    }
    public void InstantDialog(string dialog, float delay = 0f)
    {
        StopAllCoroutines();
        StartCoroutine(EInstantDialog(dialog, delay));
    }
    IEnumerator EInstantDialog(string dialog,float delay)
    {
        textbox.text = "";
        yield return new WaitForSeconds(delay);
        textbox.text = dialog;
    }

    public void ReadDialog(string dialog)
    {
        StopAllCoroutines();
        StartCoroutine(TypeDialog(dialog));
    }

    private void Update()
    {
        if (UnityEngine.InputSystem.Keyboard.current.qKey.wasPressedThisFrame)
        { 
            ReadDialog(sampleText);
        }
    }

    public IEnumerator TypeDialog(string dialog)
    {
        
        if (lettersPerSecond <= 0)
            lettersPerSecond = 1;

        textbox.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            textbox.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        
    }

}
