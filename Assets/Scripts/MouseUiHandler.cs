using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseUiHandler : MonoBehaviour
{
    public Camera UIcam;
    public Vector2 mousePosition;
    List<RectTransform> rects = new List<RectTransform>();
    
    public Vector2 targetSize = new Vector2(384, 216);
    public Vector2 screenSize;
    public Vector2 screenFactor;
    // Start is called before the first frame update
    void Awake()
    {
        targetSize.x = UIcam.targetTexture.width;
        targetSize.y = UIcam.targetTexture.height;
    }

    // Update is called once per frame
    void Update()
    {

        screenSize = new Vector2(Screen.width, Screen.height);
        screenFactor = screenSize / targetSize;
        mousePosition = Mouse.current.position.value;
        mousePosition /= screenFactor;

        if(rects.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < rects.Count; i++)
        {
            GameObject g = rects[i].gameObject;

            bool mouseIsOver = RectTransformUtility.RectangleContainsScreenPoint(rects[i], mousePosition, UIcam);
            if (mouseIsOver && EventSystem.current.currentSelectedGameObject != g)
            {
                MenuOption option = rects[i].GetComponent<MenuOption>();
                option.HighLight();

            }
        }
    }

    public void AddRects(RectTransform rect)
    { 
        rects.Add(rect);
    }
    public void AddRects(List<RectTransform> rectList)
    {
        rects.AddRange(rectList);
    }
    public void ClearRects()
    {
        rects.Clear();
    }
}
