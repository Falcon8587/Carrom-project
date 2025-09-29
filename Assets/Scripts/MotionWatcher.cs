using System.Collections;
using UnityEngine;

public class MotionWatcher : MonoBehaviour
{
    public CarromGameManager gm;
    public Rigidbody2D[] allBodies;

    public void StartChecking()
    {
        StartCoroutine(CheckMotion());
    }

    private IEnumerator CheckMotion()
    {
        yield return new WaitForSeconds(2f); // wait a bit before checking

        bool moving = true;
        while (moving)
        {
            moving = false;
            foreach (var rb in allBodies)
            {
                if (rb != null && rb.velocity.magnitude > 0.05f)
                {
                    moving = true;
                    break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        // Now safe to pass turn
        gm.NextTurn();
    }
}