using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameController : MonoBehaviour
{
    public static GameController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [Header("SpawnPoint Variables")]
    public Transform spawnPoints;
    public List<Vector3> SpawnPointList = new List<Vector3>();

    [Header("Mole Variables")]

    public GameObject molePrefab;
    public List<GameObject> MolesInScene = new List<GameObject>();
    public GameObject BossMole;
    int hitBossMoleCount;
    public Transform playerVR;
    int score;
    float durationMole = 2f;

    [Header("Timer variables")]
    float timer;
    bool canCount = false;
    int minutes;
    int seconds;
    string timeStr;

    [Header("UI variables")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public GameObject startButton;


    public void StartButtonFunction()
    {
        startButton.SetActive(false);
        score = 0;
        SpawnPointList.Clear();
        foreach (Transform item in spawnPoints)
        {
            SpawnPointList.Add(item.localPosition);
        }
        durationMole = 2f;
        timer = 60f;
        hitBossMoleCount = 2;
        canCount = true;
        timeText.text = " 1 : 00";
        scoreText.text = "";
        InvokeRepeating("GetMole", 0f, durationMole);
    }
    private void Update()
    {
        if (timer > 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            minutes = Mathf.FloorToInt(timer / 60f);
            seconds = Mathf.FloorToInt(timer - minutes * 60);
            timeStr = string.Format("{0:0} : {1:00}", minutes, seconds);
            timeText.text = timeStr;
        }
        else if (timer <= 0.0f && canCount)
        {
            Finish();
        }
    }

    void Finish()
    {
        CancelInvoke("GetMole");
        startButton.SetActive(true);
        canCount = false;
        timeText.text = "0 : 00";
        foreach (GameObject mole in MolesInScene.ToArray())
        {
            MolesInScene.Remove(mole);
            Destroy(mole);
        }
        BossMole.SetActive(false);
    }
    void GetMole()
    {
        if (SpawnPointList.Count > 0)
        {
            Vector3 randomPosition = SpawnPointList[Random.Range(0, SpawnPointList.Count)];
            SpawnPointList.Remove(randomPosition);
            GameObject currentMole = Instantiate(molePrefab, randomPosition, Quaternion.identity, transform);
            currentMole.transform.LookAt(new Vector3(playerVR.position.x, currentMole.transform.position.y, playerVR.position.z));
            currentMole.SetActive(true);
            MolesInScene.Add(currentMole);
        }
        else
        {
            Debug.Log("Reach Max moles");
        }
        
        if (score > 0 && !BossMole.activeSelf && score % 40 == 0 && Random.Range(0, 2) == 0)
        {
            hitBossMoleCount = 2;
            BossMole.SetActive(true);
        }
    }

    public void DestroyMole(GameObject _mole)
    {
        if (_mole.name == "BossMole")
        {
            if (hitBossMoleCount == 1)
            {
                BossMole.SetActive(false);
                score += 40;
            }
            else
            {
                hitBossMoleCount--;
            }
        }
        else
        {
            SpawnPointList.Add(_mole.transform.localPosition);
            MolesInScene.Remove(_mole);
            Destroy(_mole);
            score += 10;
        }
        scoreText.text = score.ToString();

        if (durationMole >= 0.5f)
        {
            durationMole -= 0.25f;
        }
    }
}
