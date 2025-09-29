using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int number;
    public DiceSlot diceSlot;

    public RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector3 originalPosition;
    public Transform originalParent;
    private Canvas canvas;

    public GameObject[] faces;
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        Initialize();
    }

    private void Initialize()
    {
        UIManager.Instance.BlockInput();
        
        // have 5 for rotate animation
        number = 5;
        UpdateFace();
        
        rectTransform
            .DORotate(new Vector3(0, 0, 360), 0.2f, RotateMode.FastBeyond360)
            .SetLoops(3, LoopType.Restart) // 5 loops
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                number = Random.Range(1, 7);
                UpdateFace();
                UIManager.Instance.UnblockInput();
            });
    }

    private void UpdateFace()
    {
        foreach (var face in faces)
        {
            face.SetActive(false);
        }
        faces[number - 1].SetActive(true);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = new Vector3(rectTransform.anchoredPosition.x, rectTransform.anchoredPosition.y, rectTransform.position.z);
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
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }

    public void SetNumber(int number)
    {
        this.number = number;
        UpdateFace();
    }

}
