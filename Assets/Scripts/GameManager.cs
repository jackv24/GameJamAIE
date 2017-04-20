using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public GameObject canvasPrefab;

    [Space()]
    public float spawnOffsetX = 2.0f;

    void Start()
    {
        int controllerCount = InputManager.Devices.Count;

        if (controllerCount < 1)
            controllerCount = 1;

        for(int i = 0; i < controllerCount; i++)
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
        }
    }
}
