using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int number;
    public Coord coord;
    public RectTransform rect;
    public Player player;

    public TextMeshProUGUI numberText;
    
    [Header("Visuals")]
    public Color normalColor = Color.white;
    public Color highlightColor = Color.green;

    public Image image;

    public bool hasBooster = false;
    private void Awake()
    {
        image = GetComponent<Image>();
        if (image != null) image.color = normalColor;
        rect = GetComponent<RectTransform>();
    }

    public void Init(int x, int y, int num)
    {
        this.number = num;
        this.coord = new Coord(x, y);
        numberText.text = num.ToString();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (!eventData.pointerDrag.TryGetComponent<Dice>(out var dice))
            return;
        if (dice.number != number) return;
        if (player == null || player.neighbours == null) return;
        if (!player.neighbours.Contains(this)) return;
        
        if (image != null) image.color = normalColor; // reset color after drop
        
        if (dice != null)
        {
            dice.transform.SetParent(transform); // snap parent
            dice.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // snap position
        }

        player.MoveTo(this);
        dice.diceSlot.currentDice = null;
        Destroy(dice.gameObject);
        GameManager.Instance.onCompleteTurn?.Invoke(1);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (!eventData.pointerDrag.TryGetComponent<Dice>(out var dice))
            return;
        if (dice.number != number) return;
        if (player == null || player.neighbours == null) return;
        if (!player.neighbours.Contains(this)) return;
        
        if (eventData.pointerDrag != null) // only highlight if dragging something
        {
            if (image != null)
                image.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        if (!eventData.pointerDrag.TryGetComponent<Dice>(out var dice))
            return;
        if (dice.number != number) return;
        if (player == null || player.neighbours == null) return;
        if (!player.neighbours.Contains(this)) return;
        if (image != null)
            image.color = normalColor;
    }
}
