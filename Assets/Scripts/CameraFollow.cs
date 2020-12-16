using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    //PlayerController PlayerController;
    //GameObject Player;
    Vector3 velocity = new Vector3(1, 1, 1);

    private void Start()
    {
        //Player = GameObject.Find("RoboPlayer");
        //PlayerController = Player.GetComponent<PlayerController>();
        transform.position = new Vector3(player.position.x, transform.position.y + 3, offset.z);
    }

    void Update()
    {
        // Define a target position above and behind the target transform
        Vector3 targetPosition = player.transform.TransformPoint(new Vector3(0, 5, -10));

        if (player.transform.position.y < transform.position.y - 30)
        {
            //if player is falling camera should at least try and keep player on screen
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.1f);
        }
        else
        {
            // Smoothly move the camera towards that target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 0.45f);
        }
    }

    public void setTarget(Transform target)
    {
        player = target;
    }
}
