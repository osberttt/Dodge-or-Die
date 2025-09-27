using System;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public GameObject dicePrefab;
    public DiceSlot[] diceSlots;

    private void OnEnable()
    {
        GameManager.Instance.onCompleteTurn.AddListener(RollTheDie);
    }

    private void OnDisable()
    {
        GameManager.Instance.onCompleteTurn.RemoveListener(RollTheDie);
    }

    private void Start()
    {
        RollTheDie();
    }

    private void RollTheDie()
    {
        foreach (var diceSlot in diceSlots)
        {
            if (diceSlot.currentDice == null) RollADice(diceSlot);
        }
    }
    private void RollADice(DiceSlot diceSlot)
    {
        var diceObj = Instantiate(dicePrefab, diceSlot.transform);
        var dice = diceObj.GetComponent<Dice>();
        diceSlot.currentDice = dice;
        dice.diceSlot = diceSlot;
    }

    public void DeleteDice()
    {
        foreach (var diceSlot in diceSlots)
        {
            if (diceSlot.currentDice != null)
            {
                Destroy(diceSlot.currentDice.gameObject);
                diceSlot.currentDice = null;
            }
        }
    }
}
