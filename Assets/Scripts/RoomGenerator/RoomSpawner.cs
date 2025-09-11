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
    private bool spawned = false;

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.occupiedPositions.Add(transform.position);
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (spawned) return;

        if (IsInsideOccupiedRoom(transform.position))
        {
            Destroy(gameObject);
            spawned = true;
            return;
        }
        GameObject room = null;

        // If we have room left to spawn
        if (templates.ROOM_COUNT < templates.MAX_ROOMS)
        {
            if (openingDirection == 1)
            {
                rand = Random.Range(0, templates.topRooms.Length);
                room = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                room = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                room = Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                room = Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
            }

            if (room != null)
            {
                templates.ROOM_COUNT++; // Only count real rooms
            }
        }
        else
        {
            // Cap if we've hit the limit
            Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
        }

        spawned = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnPoint"))
        {
            RoomSpawner otherSpawner = other.GetComponent<RoomSpawner>();

            if (!spawned && !otherSpawner.spawned)
            {
                // If two unspawned spawners meet, place a cap
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }

            spawned = true;
        }
    }

    bool IsInsideOccupiedRoom(Vector3 position)
    {
        foreach (Bounds occupied in templates.occupiedRooms)
        {
            if (occupied.Contains(position))
            {
                return true;
            }
        }
        return false;
    }
}

