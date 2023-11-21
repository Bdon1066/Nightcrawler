using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public bool playerIsInRoom;
    public GameManager gm;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInRoom = true;
            gm.deathLocation = name;
        }
        else
        {
            playerIsInRoom = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInRoom = false;
        }
    }
}
