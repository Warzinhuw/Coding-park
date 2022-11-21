using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public enum JoyStickDirection { Horizontal, Vertical, Both}
public class FixedJoystick : MonoBehaviour,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;
    public JoyStickDirection joyStickDirection = JoyStickDirection.Horizontal;
    public RectTransform handle;
    [Range(0,2f)] public float handleLimit = 1f;
    Vector2 input = Vector2.zero;
    //Output
    public float Vertical { get { return input.y; } }
    public float Horizontal { get { return input.x; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData) {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData) {
        Vector2 joyDirection = eventData.position - RectTransformUtility.WorldToScreenPoint(new Camera(), background.position);
        input = (joyDirection.magnitude > background.sizeDelta.x / 2f) ? joyDirection.normalized : joyDirection / (background.sizeDelta.x / 2f);
        if(joyStickDirection == JoyStickDirection.Horizontal) {
            input = new Vector2(input.x, 0f);
        }
        if(joyStickDirection == JoyStickDirection.Vertical) {
            input = new Vector2(0f, input.y);
        }
        handle.anchoredPosition = (input * background.sizeDelta.x / 2f) * handleLimit;
    }

    public void OnPointerUp(PointerEventData eventData) {
        input = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
