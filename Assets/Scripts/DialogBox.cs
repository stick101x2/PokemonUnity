using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textbox;
    [Range(1,120)]
    [SerializeField] int lettersPerSecond = 1;

    [SerializeField] string sampleText = "Sample Text";
    [SerializeField] Vector2 textPos;
    [SerializeField] GameObject cursor;

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

    public IEnumerator TypeDialog(string dialog, bool requireInput = false)
    {
        cursor.SetActive(false);

        if (lettersPerSecond <= 0)
            lettersPerSecond = 1;

        textbox.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            textbox.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

        if(requireInput)
        {
            cursor.SetActive(true);
            textPos = GetPositionOfLastLetter(textbox);
            cursor.transform.position = textPos + new Vector2(0.5f,0);
            yield return new WaitUntil(() => Keyboard.current.pKey.wasPressedThisFrame);
            AudioManager.Play("select",7);
        }
        cursor.SetActive(false);
    }
    public Vector2 GetPositionOfLastLetter(TextMeshProUGUI tmp_text)
    {

        tmp_text.ForceMeshUpdate();

        Vector3[] vertices = tmp_text.mesh.vertices;
        TMP_CharacterInfo charInfo = tmp_text.textInfo.characterInfo[tmp_text.textInfo.characterCount - 1];
        int vertexIndex = charInfo.vertexIndex;

        Vector2 charMidTopLine = new Vector2((vertices[vertexIndex + 0].x + vertices[vertexIndex + 2].x) / 2, (charInfo.bottomLeft.y + charInfo.topLeft.y) / 2);
        Vector3 worldPos = tmp_text.transform.TransformPoint(charMidTopLine);

        return worldPos;
    }
}
