using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int number;
    public TextMeshProUGUI numberText;
    public DiceSlot diceSlot;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector3 originalPosition;
    private Transform originalParent;
    private Canvas canvas;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        number = Random.Range(1, 7);
        numberText.text = number.ToString();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;

        // Make item draggable over everything
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move with mouse
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // If not dropped on a DropZone, snap back
        if (transform.parent == canvas.transform)
        {
            rectTransform.anchoredPosition = originalPosition;
            transform.SetParent(originalParent);
        }
    }
    

}
