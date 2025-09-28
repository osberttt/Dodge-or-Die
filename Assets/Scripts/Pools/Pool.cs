using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pool : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;

    [Header("Visuals")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.green;
    
    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (!eventData.pointerDrag.TryGetComponent<Dice>(out var dice))
            return;
        if (image != null)
            image.color = normalColor;
        PoolEffect(dice);
    }

    public virtual void PoolEffect(Dice dice)
    {
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (!eventData.pointerDrag.TryGetComponent<Dice>(out var dice))
            return;
        if (image != null)
            image.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (!eventData.pointerDrag.TryGetComponent<Dice>(out var dice))
            return;
        if (image != null)
            image.color = normalColor;
    }
}
