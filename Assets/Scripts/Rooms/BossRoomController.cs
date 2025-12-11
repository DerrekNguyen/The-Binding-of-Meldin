using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles boss room behavior and objectives
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

    private bool roomFound = false;

    void Start()
    {
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

        if (doors != null)
        {
            doors.SetActive(false);
        }

        bossPedestal = FindInactiveObjectByName(bossPedestalName);
        if (bossPedestal != null)
        {
            bossPedestal.transform.position = transform.position;
            bossPedestal.SetActive(true);
            pedestalScript = bossPedestal.GetComponent<BossPedestal>();
        }

        boss = FindInactiveObjectByName(bossName);
        if (boss != null)
        {
            if (boss.transform.parent != null)
            {
                boss.transform.SetParent(null);
            }
            boss.SetActive(false);
        }

        exit = FindInactiveObjectByName(exitName);
        if (exit != null)
        {
            if (exit.transform.parent != null)
            {
                exit.transform.SetParent(null);
            }
            exit.SetActive(false);
        }

        roomFound = false;
    }

    void Update()
    {
        if (!bossSpawn)
        {
            if (pedestalScript != null && pedestalScript.playerInitiated && !countdownStarted)
            {
                StartCoroutine(StartBossCountdown());
            }
        }
        else if (!bossCleared)
        {
            if (enemyCount == 0 && bossCheckCoroutine == null)
            {
                bossCheckCoroutine = StartCoroutine(VerifyBossCleared());
            }
            else if (enemyCount > 0 && bossCheckCoroutine != null)
            {
                StopCoroutine(bossCheckCoroutine);
                bossCheckCoroutine = null;
            }
        }
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
                if(!roomFound) 
                {
                objText.text = "Start Boss";
                if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D("bossRoomEnter");                    
                }
                roomFound = true;
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

        if (bossPedestal != null)
        {
            Destroy(bossPedestal);
        }

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

        if (boss != null)
        {
            boss.transform.position = transform.position;
            boss.SetActive(true);
        }

        bossSpawn = true;
        objText.text = "Fight!";
    }

    private IEnumerator VerifyBossCleared()
    {
        yield return new WaitForSeconds(3f);
        
        if (enemyCount == 0)
        {
            bossCleared = true;
        }
        
        bossCheckCoroutine = null;
    }

    private void SpawnExit()
    {
        exitSpawned = true;

        if (exit != null)
        {
            exit.transform.position = transform.position;
            exit.SetActive(true);
            if (SoundManager.Instance != null) SoundManager.Instance.PlaySound2D("levelComplete");
        }

        objText.text = "Leave";
    }

    private GameObject FindInactiveObjectByName(string name)
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.hideFlags == HideFlags.None && obj.scene.IsValid() && obj.name == name)
            {
                return obj;
            }
        }
        
        return null;
    }
}
