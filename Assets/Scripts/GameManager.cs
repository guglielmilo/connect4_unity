using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player1; // coins
    public GameObject player2;

    public GameObject[] spawnCoins;

    private int nextPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        nextPlayer = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectCol(int col)
    {
        Debug.Log("SelectCol:" + col + " nextPlayer:" + nextPlayer);
        Instantiate(getPlayer(), spawnCoins[col].transform.position, Quaternion.Euler(-90, 0, 0));
    }

    private GameObject getPlayer()
    {
        if (nextPlayer == 1)
        {
            nextPlayer = 2;
            return player1;
        }

        nextPlayer = 1;
        return player2;
    }
}
