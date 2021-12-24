using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject coinPlayer1;
    public GameObject coinPlayer2;
    
    public GameObject winCanvas;
    private int player1score = 0;
    private int player2score = 0;

    private State state = new State('1');

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        Debug.Log("state:" + state);
        winCanvas.SetActive(false);
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
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        Transform playerWonTextTransform = winCanvas.transform.Find("PlayerWonText");
        Text playerWonText = playerWonTextTransform.GetComponent<Text>();

        if (state.PlayerWin == '1')
        {
            player1score++;
            playerWonText.text = "Player 1 Won";
            playerWonText.color = Color.red;
        }
        else if (state.PlayerWin == '2')
        {
            player2score++;
            playerWonText.text = "Player 2 Won";
            playerWonText.color = Color.yellow;
        }
        else
        {
            playerWonText.text = "Draw";
            playerWonText.color = Color.black;
        }

        Transform player1ScoreTextTransform = winCanvas.transform.Find("Player1ScoreText");
        Text player1ScoreText = player1ScoreTextTransform.GetComponent<Text>();
        player1ScoreText.text = player1score.ToString();
        
        Transform player2ScoreTextTransform = winCanvas.transform.Find("Player2ScoreText");
        Text player2ScoreText = player2ScoreTextTransform.GetComponent<Text>();
        player2ScoreText.text = player2score.ToString();

        winCanvas.SetActive(true);
    }

}
