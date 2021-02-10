using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    int score;
    [SerializeField] SpawnManager spawnHandler;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        spawnHandler.GetSpawnData();
    }

    public void FinishGame()
    {

    }

    public void ChangeScore(int value)
    {
        score += value;
    }
}
