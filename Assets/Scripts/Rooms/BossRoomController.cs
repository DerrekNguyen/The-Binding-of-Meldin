using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossRoomController : MonoBehaviour
{
    [Header("Object Names")]
    [SerializeField] private string bossPedestalName = "BossPedestal";
    [SerializeField] private string bossName = "SlimeKing";
    [SerializeField] private string exitName = "Exit";

    [Header("Prefabs and GameObjects")]
    public GameObject doors;

    [Header("Flags")]
    public bool bossSpawn;
    public bool bossCleared;

    private TextMeshProUGUI objText;
    private GameObject bossPedestal;
    private GameObject boss;
    private GameObject exit;
    private BossPedestal pedestalScript;
    private bool countdownStarted = false;
    private bool exitSpawned = false;
    private Coroutine bossCheckCoroutine = null;
    private int enemyCount = 0;

    void Start()
    {
        // Find objText by navigating through HudCanvas hierarchy
        GameObject hudCanvas = GameObject.Find("HudCanvas");
        if (hudCanvas != null)
        {
            Transform mainHud = hudCanvas.transform.Find("MainHud");
            if (mainHud != null)
            {
                Transform objBox = mainHud.Find("ObjBox");
                if (objBox != null)
                {
                    Transform textTransform = objBox.Find("Text");
                    if (textTransform != null)
                    {
                        objText = textTransform.GetComponent<TextMeshProUGUI>();
                    }
                }
            }
        }

        // Initialize doors by finding the Doors tilemap in the TileMap gameobject in shared parent
        if (doors == null)
        {
            Transform parent = transform.parent;
            if (parent != null)
            {
                Transform tilemapObj = parent.Find("TileMap");
                if (tilemapObj != null)
                {
                    Transform doorsTransform = tilemapObj.Find("Doors");
                    if (doorsTransform != null)
                    {
                        doors = doorsTransform.gameObject;
                    }
                }
            }
        }

        // Ensure doors start disabled
        if (doors != null)
        {
            doors.SetActive(false);
        }

        // Find boss pedestal by name (including inactive objects)
        bossPedestal = FindInactiveObjectByName(bossPedestalName);
        if (bossPedestal != null)
        {
            bossPedestal.transform.position = transform.position;
            bossPedestal.SetActive(true);
            pedestalScript = bossPedestal.GetComponent<BossPedestal>();
        }

        // Find boss and exit by name (keep inactive for now)
        boss = FindInactiveObjectByName(bossName);
        if (boss != null)
        {
            // IMPORTANT: Unparent the boss from any room hierarchy to prevent cascade deletion
            if (boss.transform.parent != null)
            {
                boss.transform.SetParent(null);
            }
            boss.SetActive(false);
        }

        exit = FindInactiveObjectByName(exitName);
        if (exit != null)
        {
            // Unparent exit as well
            if (exit.transform.parent != null)
            {
                exit.transform.SetParent(null);
            }
            exit.SetActive(false);
        }
    }

    void Update()
    {
        // Phase 1: Boss not spawned yet
        if (!bossSpawn)
        {
            // Check if player initiated on pedestal
            if (pedestalScript != null && pedestalScript.playerInitiated && !countdownStarted)
            {
                StartCoroutine(StartBossCountdown());
            }
        }
        // Phase 2: Boss spawned, check if cleared
        else if (!bossCleared)
        {
            // Start verification coroutine if no enemies and not already checking
            if (enemyCount == 0 && bossCheckCoroutine == null)
            {
                bossCheckCoroutine = StartCoroutine(VerifyBossCleared());
            }
            // Cancel verification if enemies appear again
            else if (enemyCount > 0 && bossCheckCoroutine != null)
            {
                StopCoroutine(bossCheckCoroutine);
                bossCheckCoroutine = null;
            }
        }
        // Phase 3: Boss cleared, spawn exit
        else if (!exitSpawned)
        {
            SpawnExit();
            if (doors != null)
            {
                doors.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (bossSpawn && other.CompareTag("Enemy"))
        {
            enemyCount++;
        }
        if(other.CompareTag("Player")) 
        {
            if(!exitSpawned) 
            {
                objText.text = "Start Boss";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (bossSpawn && other.CompareTag("Enemy"))
        {
            enemyCount--;
        }
    }

    private IEnumerator StartBossCountdown()
    {
        countdownStarted = true;

        // Destroy pedestal
        if (bossPedestal != null)
        {
            Destroy(bossPedestal);
        }

        // Activate doors
        if (doors != null)
        {
            doors.SetActive(true);
        }

        float countdown = 3f;
        while (countdown > 0f)
        {
            if (objText != null)
            {
                objText.text = countdown.ToString("F1");
            }
            yield return new WaitForSeconds(0.1f);
            countdown -= 0.1f;
        }

        // Teleport boss to center and activate
        if (boss != null)
        {
            boss.transform.position = transform.position;
            boss.SetActive(true);
        }

        bossSpawn = true;
        objText.text = "! Fight !";
    }

    private IEnumerator VerifyBossCleared()
    {
        // Wait 3 seconds to ensure no new enemies spawn
        yield return new WaitForSeconds(3f);
        
        // Double-check enemies are still dead after waiting
        if (enemyCount == 0)
        {
            bossCleared = true;
        }
        
        bossCheckCoroutine = null;
    }

    private void SpawnExit()
    {
        exitSpawned = true;

        // Teleport exit to center and activate
        if (exit != null)
        {
            exit.transform.position = transform.position;
            exit.SetActive(true);
        }

        objText.text = "Leave";
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        // Find all objects including inactive ones
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // Filter out prefabs and editor-only objects
            if (obj.hideFlags == HideFlags.None && obj.scene.IsValid() && obj.name == name)
            {
                return obj;
            }
        }
        
        return null;
    }
}
