using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject coinPlayer1; // coins
    public GameObject coinPlayer2;

    public GameObject[] spawnCoins;

    private State state = new State('1');

    private int nextPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        Debug.Log("state:" + state);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SelectCol(int col)
    {
        Debug.Log("playerTurn:" + state.PlayerTurn + " col:" + col);
        if (state.isColValid(col))
        {
            state.addPosition(col);
            Instantiate(getCoinPlayer(), spawnCoins[col].transform.position, Quaternion.Euler(-90, 0, 0));

            Debug.Log("state:" + state);

            if (state.Done)
            {
                Debug.Log("board done, winner:" + Player.playerToString(state.PlayerWin));
            }
        }
        else
        {
            Debug.LogWarning("col:" + col + " is not valid");
        }
    }

    private GameObject getCoinPlayer()
    {
        if (nextPlayer == 1)
        {
            nextPlayer = 2;
            return coinPlayer1;
        }

        nextPlayer = 1;
        return coinPlayer2;
    }
}
