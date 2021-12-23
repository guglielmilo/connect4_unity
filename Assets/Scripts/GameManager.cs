using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject coinPlayer1;
    public GameObject coinPlayer2;

    private State state = new State('1');

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

    public bool IsColValid(int col)
    {
        return state.isColValid(col);
    }

    public char PlayerTurn()
    {
        return state.PlayerTurn;
    }

    public void SelectCol(int col)
    {
        Debug.Log("playerTurn:" + state.PlayerTurn + " col:" + col);

        state.addPosition(col);

        Debug.Log("state:" + state);

        if (state.Done)
        {
            Debug.Log("board done, winner:" + Player.playerToString(state.PlayerWin));
        }
    }
}
