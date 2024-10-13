using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 originalScale;
    public float scaleFactor;
    public void Start()
    {
        originalScale = transform.localScale;
    }
    void IPointerEnterHandler.OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
        gameObject.transform.localScale *= scaleFactor;
    }

    void IPointerExitHandler.OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
        gameObject.transform.localScale = originalScale;
    }

    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        gameObject.transform.localScale = originalScale;
    }
}
