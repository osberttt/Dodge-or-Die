using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class AddPool : Pool
{
    protected override void PoolEffect(Dice dice)
    {
        base.PoolEffect(dice);
        dice.transform.SetParent(transform); // snap parent
        dice.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // snap position

        var newNumber = dice.number + 1;
        if (newNumber > 6) newNumber = 1;
        dice.SetNumber(newNumber);

        dice.transform.SetParent(dice.originalParent);
        UIManager.Instance.BlockInput();
        dice.rectTransform
            .DOAnchorPos(dice.originalPosition, 0.5f, true)
            .OnComplete(() => UIManager.Instance.UnblockInput());

        if (audioSource != null)
            audioSource.Play();
    }
    
}
