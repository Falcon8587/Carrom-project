using UnityEngine;

public class CarromPocket : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        int scoreToAdd = 0;

        if (other.CompareTag("CoinBlack"))
        {
            scoreToAdd = 10;
        }
        else if (other.CompareTag("CoinYellow"))
        {
            scoreToAdd = 20;
        }
        else if (other.CompareTag("CoinRed"))
        {
            scoreToAdd = 50;
        }

        if (scoreToAdd > 0)
        {
            // Add score here (later connect this with ScoreManager / Photon)
            Debug.Log($"{other.tag} pocketed! +{scoreToAdd} points");

            // Destroy the coin
            Destroy(other.gameObject);
        }
    }
}
