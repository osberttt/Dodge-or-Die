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

    [Header("Audio")]
    public AudioSource audioSource;
    
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
        GameManager.Instance.onCompleteTurn?.Invoke(-1);
    }

    protected virtual void PoolEffect(Dice dice)
    {

    }

    public void PlaySound()
    {
        if (audioSource != null && !audioSource.isPlaying)
            audioSource.Play();
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
