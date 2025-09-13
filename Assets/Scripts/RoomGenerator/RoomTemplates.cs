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
    public int MAX_ROOMS = 40;
    public int ROOM_COUNT = 0;

    public List<Vector3> occupiedPositions = new List<Vector3>();
    public List<Bounds> occupiedRooms = new List<Bounds>();


}
