using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    [Header("Room Prefabs")]
    public GameObject[] topRooms;
    public GameObject[] bottomRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    public GameObject[] capRoom;
    public GameObject closedRoom;
    [Header("BossEnemyPrefab")]
    public GameObject bossEnemyPrefab;

    [Header("Dungeon Settings")]
    public int MAX_ROOMS = 40;
    public int ROOM_COUNT = 0;
    [SerializeField] private float roomSize = 11f;

    [Header("Runtime Tracking")]
    public List<Vector3> occupiedPositions = new List<Vector3>();
    public List<GameObject> rooms = new List<GameObject>();

    public GameObject startRoom; // first room spawned
    public Vector3 startRoomPosition;
    public bool startRoomSet = false;

    [Header("Debug Options")]
    public bool showDebugLines = true;

    private void Start()
    {
        StartCoroutine(AssignBossRoomAfterGeneration());
    }

    private IEnumerator AssignBossRoomAfterGeneration()
    {
        yield return new WaitForSeconds(5f);

        if (rooms.Count == 0)
        {
            Debug.LogWarning("No rooms found for boss assignment!");
            yield break;
        }

        GameObject farA = FindFurthestRoom(startRoom);
        GameObject boss = FindFurthestRoom(farA);

        boss.name = "BossRoom";
        var sr = boss.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.red;

        // Optionally spawn a boss prefab inside the room
        if (bossEnemyPrefab != null)
        {
            Transform spawnPoint = boss.transform.Find("BossSpawnPoint");
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : boss.transform.position;

            Instantiate(bossEnemyPrefab, spawnPos, Quaternion.identity);
        }

        Debug.Log("Boss Room assigned at " + boss.transform.position);
    }

    private GameObject FindFurthestRoom(GameObject fromRoom)
    {
        Vector3 fromPos = fromRoom.transform.position;
        GameObject furthest = fromRoom;
        float maxDistance = 0f;

        foreach (GameObject room in rooms)
        {
            float dist = Vector3.Distance(fromPos, room.transform.position);
            if (dist > maxDistance)
            {
                maxDistance = dist;
                furthest = room;
            }
        }

        return furthest;
    }

    public void RegisterRoom(GameObject room)
    {
        // Call this from RoomSpawner after instantiating a room
        rooms.Add(room);

        if (!startRoomSet)
        {
            startRoom = room;
            startRoomPosition = room.transform.position;
            startRoomSet = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showDebugLines || rooms.Count == 0 || startRoom == null) return;

        Gizmos.color = Color.yellow;
        foreach (GameObject room in rooms)
        {
            foreach (GameObject neighbor in GetConnectedRooms(room))
            {
                Gizmos.DrawLine(room.transform.position, neighbor.transform.position);
            }
        }

        // Draw start room in green
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(startRoom.transform.position, 0.3f);
    }

    private List<GameObject> GetConnectedRooms(GameObject room)
    {
        List<GameObject> connected = new List<GameObject>();

        foreach (GameObject other in rooms)
        {
            if (other == room) continue;
            if (Vector3.Distance(room.transform.position, other.transform.position) <= roomSize + 0.1f)
            {
                connected.Add(other);
            }
        }

        return connected;
    }
}
