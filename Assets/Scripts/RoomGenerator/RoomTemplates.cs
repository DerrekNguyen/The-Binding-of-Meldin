using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] capRoom;

    public GameObject closedRoom;
    public List<GameObject> rooms;
    public float waitTime;
    public bool spawnedBoss;
    public GameObject boss;
    public int MAX_ROOMS = 40;
    public int ROOM_COUNT = 0;

    public List<Vector3> occupiedPositions = new List<Vector3>();
    public List<Bounds> occupiedRooms = new List<Bounds>();

    void Update()
    {
        if (waitTime <= 0 && spawnedBoss == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                }
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
