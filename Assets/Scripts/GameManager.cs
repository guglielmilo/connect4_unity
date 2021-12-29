using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject coinPlayer1;
    public GameObject coinPlayer2;

    public InputCol[] inputCols;
    
    public GameObject mainMenuCanvas;
    private bool computer;
    private char computerPlayer = '1';
    private bool computerPending = false;

    public GameObject winCanvas;
    private int player1score = 0;
    private int player2score = 0;

    public GameObject pauseCanvas;
    private bool pause = false;

    public Text logGameText;

    private State state;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseScreen();
        }

        logGameText.color = Color.black;
        if (IsGameActive())
        {
            logGameText.text = PlayerTurn() == '1' ? "Red's turn" : "Yellow's turn";
        }
        else if (computerPending)
        {
            logGameText.text = "Computer...";
        }
        else
        {
            logGameText.text = "";
        }
    }

    private void StartGame()
    {
        if (computer)
        {
            Start1PlayerGame();
        }
        else
        {
            Start2PlayersGame();
        }
    }

    public void Start1PlayerGame()
    {
        computer = true;
        computerPending = false;
        state = new State('1');
        pause = false;
        mainMenuCanvas.SetActive(false);
        StartCoroutine(ComputerTurn());
    }

    public void Start2PlayersGame()
    {
        computer = false;
        computerPending = false;
        state = new State('1');
        pause = false;
        mainMenuCanvas.SetActive(false);
    }

    public bool IsGameActive()
    {
        return state != null && !pause && !computerPending;
    }

    public bool IsColValid(int col)
    {
        if (state == null)
        {
            return false;
        }
        return state.isColValid(col);
    }

    public char PlayerTurn()
    {
        if (state == null)
        {
            return '0';
        }
        return state.PlayerTurn;
    }

    private IEnumerator ComputerTurn()
    {

        computerPending = true;

        int col;
        do // simulate computer
        {
            col = UnityEngine.Random.Range(0, 6);
        }
        while(!IsColValid(col));

        Debug.Log("computerTurn: playerTurn:" + state.PlayerTurn + " col:" + col);
    
        yield return new WaitForSeconds(0.5f);

        computerPending = false;

        inputCols[col].AddComputerSpawn(state.PlayerTurn);
    }

    public void SelectCol(int col)
    {
        if (state == null)
        {
            Debug.Log("selectCol: state is null");
            return;
        }

        Debug.Log("playerTurn:" + state.PlayerTurn + " col:" + col);

        state.addPosition(col);

        Debug.Log("state:" + state);

        if (state.Done)
        {
            Debug.Log("board done, winner:" + Player.playerToString(state.PlayerWin));
            UpdateScores();
            ShowWinScreen();
        }
        else
        {
            if (computer && computerPlayer == state.PlayerTurn)
            {
                StartCoroutine(ComputerTurn());
            }
        }
    }

    private void UpdateScores()
    {
        if (state.PlayerWin == '1')
        {
            player1score++;
            logGameText.text = "Red wins";
            logGameText.color = Color.red;
        }
        else if (state.PlayerWin == '2')
        {
            player2score++;
            logGameText.text = "Yellow wins";
            logGameText.color = Color.yellow;
        }
        else
        {
            logGameText.text = "Draw";
            logGameText.color = Color.black;
        }
    }

    private void ShowWinScreen()
    {
        pause = true;

        Transform playerWonTextTransform = winCanvas.transform.Find("PlayerWonText");
        Text playerWonText = playerWonTextTransform.GetComponent<Text>();

        if (state.PlayerWin == '1')
        {
            playerWonText.text = "Red wins";
            playerWonText.color = Color.red;
        }
        else if (state.PlayerWin == '2')
        {
            playerWonText.text = "Yellow wins";
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

    private void ShowPauseScreen()
    {
        pause = true;

        Transform player1ScoreTextTransform = pauseCanvas.transform.Find("Player1ScoreText");
        Text player1ScoreText = player1ScoreTextTransform.GetComponent<Text>();
        player1ScoreText.text = player1score.ToString();
        
        Transform player2ScoreTextTransform = pauseCanvas.transform.Find("Player2ScoreText");
        Text player2ScoreText = player2ScoreTextTransform.GetComponent<Text>();
        player2ScoreText.text = player2score.ToString();

        pauseCanvas.SetActive(true);
    }

    public void Continue()
    {
        pause = false;
        pauseCanvas.SetActive(false);
    }

    public void Restart()
    {
        ClearCoins();
        winCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        StartGame();
    }

    public void ShowMainMenu()
    {
        ClearCoins();

        player1score = 0;
        player2score = 0;

        mainMenuCanvas.SetActive(true);
        winCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
    }

    private void ClearCoins()
    {
        foreach(GameObject playerCoin in GameObject.FindGameObjectsWithTag("coins"))
        {
            Destroy(playerCoin);
        }
    }

}
