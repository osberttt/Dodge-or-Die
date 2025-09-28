using UnityEngine;

public class SubtractPool : Pool
{
    private Dice diceRef;
    public override void PoolEffect(Dice dice)
    {
        base.PoolEffect(dice);
        diceRef = dice;
        dice.transform.SetParent(transform); // snap parent
        dice.GetComponent<RectTransform>().anchoredPosition = Vector2.zero; // snap position
        
        var newNumber = ((dice.number - 1 + 1) % 6) - 1;
        dice.SetNumber(newNumber);
        
        Invoke(nameof(SendDiceBack), 0.1f);
    }

    private void SendDiceBack()
    {
        diceRef.transform.SetParent(diceRef.originalParent);
        diceRef.rectTransform.anchoredPosition = diceRef.originalPosition;
        GameManager.Instance.onCompleteTurn?.Invoke();
    }
}