using Fusion;
using UnityEngine;

public class CarromGameManager : NetworkBehaviour
{
    [Networked] public int CurrentTurnIndex { get; set; }
    [Networked] public int Player1Score { get; set; }
    [Networked] public int Player2Score { get; set; }

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            CurrentTurnIndex = 0; // Start with Player 0
            Player1Score = 0;
            Player2Score = 0;
        }
    }

    public void AddScore(int playerIndex, int value)
    {
        if (!Runner.IsServer) return;

        if (playerIndex == 0) Player1Score += value;
        else Player2Score += value;
    }

    public void NextTurn()
    {
        if (!Runner.IsServer) return;
        CurrentTurnIndex = (CurrentTurnIndex + 1) % 2;
    }
}
