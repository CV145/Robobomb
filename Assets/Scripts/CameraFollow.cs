using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    PlayerController PlayerController;
    GameObject Player;

    private void Start()
    {
        Player = GameObject.Find("RoboPlayer");
        PlayerController = Player.GetComponent<PlayerController>();
    }

    void Update()
    {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
            // Camera follows the player with specified offset position
        
    }
}

