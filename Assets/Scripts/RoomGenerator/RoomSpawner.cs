using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1--> need bottom door
    // 2--> need top door
    // 3--> need left door
    // 4--> need right door

    private RoomTemplates templates;
    private int rand;
    [SerializeField]
    private bool spawned = false;

    void Start()
    {
        Debug.Log("RoomSpawner started at " + transform.position + " with openingDirection " + openingDirection);

        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        //Invoke("Spawn", 0.2f);
        Spawn();
    }

    void Spawn()
    {
        if (spawned) return;

        if (templates.ROOM_COUNT >= templates.MAX_ROOMS)
        {
            Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
            spawned = true;
            return;
        }

        if (spawned == false)
        {
            Debug.Log("Spawning room at: " + transform.position + " with opening direction: " + openingDirection);

            GameObject newRoom = null;

            if (openingDirection == 1)
            {
                rand = Random.Range(0, templates.topRooms.Length);
                newRoom = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                newRoom = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                newRoom = Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                newRoom = Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }

            if (newRoom != null)
            {
                Debug.Log("Spawned new room: " + newRoom.name + " with children: " + newRoom.transform.childCount);
                foreach (Transform child in newRoom.transform)
                {
                    Debug.Log("Child: " + child.name + " | Active: " + child.gameObject.activeSelf);
                }
            }
            templates.ROOM_COUNT++;
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            RoomSpawner otherSpawner = other.GetComponent<RoomSpawner>();
            if (otherSpawner != null && otherSpawner.spawned && !spawned)
            {
                Destroy(gameObject, 0.1f);
            }
        }

    }


}
