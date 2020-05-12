using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondClearPath : MonoBehaviour
{
    /**
     * Destroy asteroids that are spawned too close to the diamonds. This ensures
     * that asteroids are not spawned too close to the diamonds which would make it
     * more difficult for the player to avoid hitting the asteroids on exhale.
     * @param: other - the other object in the collider
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Asteroid"))
        {
            Destroy(other.gameObject);
        }
    }
}
