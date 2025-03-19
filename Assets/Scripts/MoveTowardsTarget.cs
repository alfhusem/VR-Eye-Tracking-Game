using System.Collections;
using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{
    public Transform target; // The player's transform
    public float speed = 0;
    private bool shouldMove = true;
    private bool isDamagingPlayer = false;

    void Update()
    {
        if (target != null && shouldMove)
        {
            // Check the distance between the quad and the target
            if (Vector3.Distance(transform.position, target.position) < 1f)
            {
                // If the distance is less than 1, stop moving
                shouldMove = false;
                if (!isDamagingPlayer)
                {
                    isDamagingPlayer = true;
                    StartCoroutine(DamagePlayerEverySecond());
                }
            }
            else
            {
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            }
        }
    }

    private IEnumerator DamagePlayerEverySecond()
    {
        while (!shouldMove)
        {
            PlayerHealthManager.Instance.TakeDamage();
            yield return new WaitForSeconds(1f);
        }
    }
}
