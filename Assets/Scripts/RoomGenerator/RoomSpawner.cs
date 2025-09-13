using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 = need bottom door
    // 2 = need top door
    // 3 = need left door
    // 4 = need right door

    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    [SerializeField] private float roomSize = 10f; // size of each room prefab

    void Start()
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    void Spawn()
    {
        if (spawned) return;

        // First room always spawns (so you don't get empty map)
        if (templates.ROOM_COUNT > 0 && IsInsideOccupiedRoom(transform.position))
        {
            Debug.Log("Spawner killed at " + transform.position + " (already occupied)");
            Destroy(gameObject);
            spawned = true;
            return;
        }

        GameObject room = null;

        if (templates.ROOM_COUNT < templates.MAX_ROOMS)
        {
            if (openingDirection == 1) // need bottom door -> spawn top room
            {
                rand = Random.Range(0, templates.topRooms.Length);
                room = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
                RemoveOppositeSpawn(room, 2);
            }
            else if (openingDirection == 2) // need top door -> spawn bottom room
            {
                rand = Random.Range(0, templates.bottomRooms.Length);
                room = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
                RemoveOppositeSpawn(room, 1);
            }
            else if (openingDirection == 3) // need left door -> spawn right room
            {
                rand = Random.Range(0, templates.rightRooms.Length);
                room = Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
                RemoveOppositeSpawn(room, 4);
            }
            else if (openingDirection == 4) // need right door -> spawn left room
            {
                rand = Random.Range(0, templates.leftRooms.Length);
                room = Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
                RemoveOppositeSpawn(room, 3);
            }

            if (room != null)
            {
                templates.ROOM_COUNT++;
                templates.occupiedPositions.Add(transform.position); // mark occupied AFTER spawn
                Debug.Log("Spawned room at " + transform.position + " | Count: " + templates.ROOM_COUNT);
            }
        }
        else
        {
            Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
            Debug.Log("Closed room placed at " + transform.position);
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
                if (AreOpposite(openingDirection, otherSpawner.openingDirection))
                {
                    // valid connection → let them spawn
                    return;
                }
                else
                {
                    Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                    Debug.Log("Closed room due to mismatch at " + transform.position);
                }
            }

            spawned = true;
        }
    }

    bool IsInsideOccupiedRoom(Vector3 position)
    {
        foreach (Vector3 occupied in templates.occupiedPositions)
        {
            if (Vector3.Distance(position, occupied) < roomSize * 0.5f)
            {
                return true;
            }
        }
        return false;
    }

    void RemoveOppositeSpawn(GameObject room, int direction)
    {
        RoomSpawner[] spawners = room.GetComponentsInChildren<RoomSpawner>();
        foreach (RoomSpawner spawner in spawners)
        {
            if (spawner.openingDirection == direction)
            {
                spawner.enabled = false;
                Destroy(spawner.gameObject);
                return;
            }
        }
    }

    bool AreOpposite(int dir1, int dir2)
    {
        return (dir1 == 1 && dir2 == 2) ||
               (dir1 == 2 && dir2 == 1) ||
               (dir1 == 3 && dir2 == 4) ||
               (dir1 == 4 && dir2 == 3);
    }
}

