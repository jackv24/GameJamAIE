using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void NormalEvent();
    public event NormalEvent OnGameStart;

    public GameObject trainSmoke;

    [HideInInspector]
    public bool gameRunning = false;
    private bool gameStarted = false;
    private bool[] readyPlayers;
    private PlayerUI[] playersUI;

    public int gameLength = 120;
    public int gameTimeLeft;

    private int pickupCount = 0;

    [Space()]
    public Text timerText;
    public Text pickupText;
    private string pickupTextString;

    [Space()]
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public GameObject canvasPrefab;

    [Space()]
    public float spawnOffsetX = 2.0f;

    void Start()
    {

        instance = this;

        SpawnPlayers();

        if (timerText)
            timerText.gameObject.SetActive(false);

        if (pickupText)
            pickupTextString = pickupText.text;

        pickupItems[] pickups = FindObjectsOfType<pickupItems>();

        pickupCount = pickups.Length;

        if(pickupText)
            pickupText.text = string.Format(pickupTextString, pickupCount);

        foreach (pickupItems p in pickups)
        {
            p.OnPickup += delegate
            {
                pickupCount--;

                if(pickupText)
                    pickupText.text = string.Format(pickupTextString, pickupCount);

                if (pickupCount <= 0)
                    gameTimeLeft = 0;
            };
        }
    }

    public void ReadyPlayer(int index)
    {
        if (!gameStarted)
        {
            readyPlayers[index] = true;

            int readyAmount = 0;

            foreach (bool ready in readyPlayers)
                if (ready)
                    readyAmount++;

            if (playersUI[index])
                playersUI[index].readyText.gameObject.SetActive(false);

            if (readyAmount == readyPlayers.Length)
            {
                gameStarted = true;

                StartCoroutine("GameTimer");

                trainSmoke.SetActive(true);
            }
        }
    }

    IEnumerator GameTimer()
    {

        if(timerText)
            timerText.gameObject.SetActive(true);

        for (int i = 3; i >= 0; i--)
        {
            if (timerText)
                timerText.text = string.Format("<color=red><size=56>{0}</size></color>", i);

            yield return new WaitForSeconds(1f);
        }

        gameRunning = true;

        if (OnGameStart != null)
            OnGameStart();

        gameTimeLeft = gameLength;

        while(gameTimeLeft >= 0)
        {
            if (timerText)
            {
                if(gameTimeLeft > 10)
                    timerText.text = string.Format("{0}:{1}", Mathf.FloorToInt(gameTimeLeft / 60).ToString("00"), (gameTimeLeft % 60).ToString("00"));
                else
                    timerText.text = string.Format("<color=red><size=56>{0}</size></color>", gameTimeLeft);
            }

            yield return new WaitForSeconds(1f);

            gameTimeLeft--;
        }

        gameRunning = false;

        if (timerText)
            timerText.text = "Game Over";

        yield return new WaitForSeconds(5f);

        ObjectPooler.PurgePools();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SpawnPlayers()
    {
        int controllerCount = InputManager.Devices.Count;

        if (controllerCount < 1)
            controllerCount = 1;

        readyPlayers = new bool[controllerCount];
        playersUI = new PlayerUI[controllerCount];

        for (int i = 0; i < controllerCount; i++)
        {
            //Player
            GameObject player = (GameObject)Instantiate(playerPrefab, new Vector3(spawnOffsetX * i, 0, 0), Quaternion.identity);
            player.name = playerPrefab.name + (i + 1);

            PlayerInput input = player.GetComponent<PlayerInput>();
            input.controllerIndex = i;
            input.Setup();

            //Camera
            GameObject camera = (GameObject)Instantiate(cameraPrefab);
            camera.name = cameraPrefab.name + (i + 1);

            Camera cam = camera.GetComponent<Camera>();
            CameraControl camControl = camera.GetComponent<CameraControl>();

            camControl.target = player.transform;

            if (controllerCount <= 1)
                cam.rect = new Rect(0, 0, 1, 1);
            else if (controllerCount <= 2)
            {
                if (i == 0)
                    cam.rect = new Rect(0, 0, 0.5f, 1);
                else if (i == 1)
                    cam.rect = new Rect(0.5f, 0, 0.5f, 1);
            }
            else
            {
                if (i == 0)
                    cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                else if (i == 1)
                    cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                else if (i == 2)
                    cam.rect = new Rect(0, 0, 0.5f, 0.5f);
                else if (i == 3)
                    cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            }

            //UI
            GameObject canvasObj = (GameObject)Instantiate(canvasPrefab);
            canvasObj.name = canvasPrefab.name + (i + 1);

            Canvas canvas = canvasObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            canvas.planeDistance = 1.0f;

            PlayerUI hud = canvasObj.GetComponent<PlayerUI>();
            hud.targetStats = player.GetComponent<PlayerStats>();

            PlayerAttack attack = player.GetComponent<PlayerAttack>();
            attack.aimCam = cam;

            readyPlayers[i] = false;
            playersUI[i] = hud;
        }
    }
}
