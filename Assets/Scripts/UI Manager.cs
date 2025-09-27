using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button RerollBtn;
    public Button EndTurnBtn;

    public DiceManager diceManager;
    private void OnEnable()
    {
        EndTurnBtn.onClick.AddListener(EndTurn);
        RerollBtn.onClick.AddListener(Reroll);
    }

    private void OnDisable()
    {
        EndTurnBtn.onClick.RemoveAllListeners();
        RerollBtn.onClick.RemoveAllListeners();
    }

    private void EndTurn()
    {
        GameManager.Instance.onCompleteTurn?.Invoke();
    }

    private void Reroll()
    {
        diceManager.DeleteDice();
        GameManager.Instance.onCompleteTurn?.Invoke();
    }
}
