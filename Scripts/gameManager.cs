using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour 
{

    bool gameHasEnded = false;

    public GameObject completeLevelUI;

    public float restartDelay = 2f;

    GameObject player;

    [Header("Replay")]
    bool instantReplay = false;
    float replayStart;
    public GameObject Replay;

    void Start()
    {
        Replay.SetActive(false);
        playerMovement PlayerMovement = FindObjectOfType<playerMovement>();
        player = PlayerMovement.gameObject;

        if (CommandLog.commands.Count > 0)
        {
            instantReplay = true;
            Replay.SetActive(true);
            replayStart = Time.timeSinceLevelLoad;
        }
    }

    void Update()
    {
        if (instantReplay)
        {
            InstantReplay();
        }
    }

    public void completeLevel()
    {
        completeLevelUI.SetActive(true);
    }

    public void endGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Debug.Log("Game Over");
            Invoke("Restart", restartDelay);
        } 
        
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void InstantReplay()
    {
        if (CommandLog.commands.Count == 0)
        {
            return;
        }

        Command command = CommandLog.commands.Peek();
        if (Time.timeSinceLevelLoad >= command.time)
        {
            command = CommandLog.commands.Dequeue();
            command.Player = player.GetComponent<Rigidbody>();
            Invoker invoker = new Invoker();
            invoker.disableLog = true;
            invoker.SetCommand(command);
            invoker.ExeCommand();
        }
    }

}
