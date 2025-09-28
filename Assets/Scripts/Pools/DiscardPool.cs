using UnityEngine;

public class DiscardPool : Pool
{
    public override void PoolEffect(Dice dice)
    {
        base.PoolEffect(dice);
        dice.diceSlot.currentDice = null;
        Destroy(dice.gameObject);
        GameManager.Instance.onCompleteTurn?.Invoke();
    }
}
