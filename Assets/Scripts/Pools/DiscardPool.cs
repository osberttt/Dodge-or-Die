using UnityEngine;

public class DiscardPool : Pool
{
    protected override void PoolEffect(Dice dice)
    {
        base.PoolEffect(dice);
        dice.diceSlot.currentDice = null;
        Destroy(dice.gameObject);
    }
}
