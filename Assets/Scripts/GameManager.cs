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
    private int computerMs = 0;

    public GameObject computerCanvas;
    private const int computerLevelMin = 1;
    private const int computerLevelMax = 5;
    private int computerLevel = computerLevelMin;
    public Button computerLevelMinusButton;
    public Button computerLevelPlusButton;
    public Text computerLevelText;

    public GameObject winCanvas;
    private int player1score = 0;
    private int player2score = 0;

    public GameObject pauseCanvas;
    private bool pause = false;

    public GameObject LogCanvas;
    public Text logGameText;

    private State state = null;

    // Start is called before the first frame update
    void Start()
    {
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && state != null && !state.Done)
        {
            if (pause)
            {
                Continue();
            }
            else
            {
                ShowPauseScreen();
            }
        }

        if (IsGameActive())
        {
            logGameText.text= "";
            if (computerPending)
            {
                logGameText.text = "Computer...";
            }
            else
            {
                logGameText.text += PlayerTurn() == '1' ? "Red's turn" : "Yellow's turn";
                if (computer)
                {
                    logGameText.text += " (Computer took " + computerMs + "ms)";
                }
            }
            LogCanvas.SetActive(true);
        }
        else
        {
            LogCanvas.SetActive(false);
        }

        if (computerCanvas.activeSelf)
        {
            computerLevelText.text = computerLevel.ToString();
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
        computerCanvas.SetActive(false);
        StartCoroutine(ComputerTurn());
    }

    public void ShowPlayer1Menu()
    {
        mainMenuCanvas.SetActive(false);
        computerCanvas.SetActive(true);
    }

    public void PlusComputerLevel()
    {
        if (computerLevel < computerLevelMax)
        {
            computerLevel += 1;
        }
    }

    public void MinusComputerLevel()
    {
        if (computerLevel > computerLevelMin)
        {
            computerLevel -= 1;
        }
    }

    public void Start2PlayersGame()
    {
        computer = false;
        computerPending = false;
        state = new State('1');
        pause = false;
        mainMenuCanvas.SetActive(false);
    }

    public bool IsComputerRunning()
    {
        return computerPending;
    }

    public bool IsGameActive()
    {
        return state != null && !pause;
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

        (int col, int timeMs) = Computer.getCol(state, 1 + computerLevel);
        computerMs = timeMs;
        Debug.Log("computerTurn: playerTurn:" + state.PlayerTurn + " col:" + col + " timeMs=" + timeMs);

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
        }
        else if (state.PlayerWin == '2')
        {
            player2score++;
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
        ClearGame();

        winCanvas.SetActive(false);
        pauseCanvas.SetActive(false);

        StartGame();
    }

    public void ShowMainMenu()
    {
        ClearGame();

        player1score = 0;
        player2score = 0;

        mainMenuCanvas.SetActive(true);
        winCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void ClearGame()
    {
        foreach(GameObject playerCoin in GameObject.FindGameObjectsWithTag("coins"))
        {
            Destroy(playerCoin);
        }

        state = null;
    }

}
