using Fusion;
using UnityEngine;

public class CarromPocket : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!Runner.IsServer) return;

        if (other.CompareTag("CoinBlack") || other.CompareTag("CoinYellow") || other.CompareTag("CoinRed"))
        {
            int score = 0;
            if (other.CompareTag("CoinBlack")) score = 10;
            else if (other.CompareTag("CoinYellow")) score = 20;
            else if (other.CompareTag("CoinRed")) score = 50;

            // Get reference to GameManager
            CarromGameManager gm = FindFirstObjectByType<CarromGameManager>();
            if (gm != null)
            {
                gm.AddScore(gm.CurrentTurnIndex, score);
            }

            // Despawn coin
            var coinObject = other.GetComponent<NetworkObject>();
            if (coinObject != null)
            {
                Runner.Despawn(coinObject);
            }
        }
    }
}
