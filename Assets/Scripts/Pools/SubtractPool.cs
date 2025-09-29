using DG.Tweening;
using UnityEngine;

public class SubtractPool : Pool
{
    protected override void PoolEffect(Dice dice)
    {
        base.PoolEffect(dice);
        dice.transform.SetParent(transform); // snap parent
        dice.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // snap position

        var newNumber = dice.number - 1;
        if (newNumber < 1) newNumber = 6;
        dice.SetNumber(newNumber);
        
        dice.transform.SetParent(dice.originalParent);
        UIManager.Instance.BlockInput();
        dice.rectTransform
            .DOAnchorPos(dice.originalPosition, 0.5f, true)
            .OnComplete(() => UIManager.Instance.UnblockInput());
    }
    
}