using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    // Regular offset for camera.
    private static Vector3 offset = new Vector3(0f, 34.3f, -87.5f);

    private float speed = 10f;
    public RocketController playerScript;

    Camera mainCamera;

    /**
     * Start is called before the first frame update
     */
    void Start()
    {
        mainCamera = Camera.main;
        playerScript = player.GetComponent<RocketController>();
    }

    /**
     * Update is called once per frame
     */
    void Update()
    {
        // Keep camera at a position behind the player.
        transform.position = player.transform.position + offset;
    }
}
