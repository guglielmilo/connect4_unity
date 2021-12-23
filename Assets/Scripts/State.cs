using System.Collections;
using System.Collections.Generic;
using static Player;

public class State
{
    public  bool  done = false;
    public  char  playerWin = Player.P0;
    public  char  playerTurn;

    private Board board = new Board();

    public State(char firstTurn)
    {
        playerTurn = firstTurn;
    }

    public bool Done
    {
        get => done;
    }

    public char PlayerWin
    {
        get => playerWin;
    }

    public char PlayerTurn
    {
        get => playerTurn;
    }

    public char getLastPayer()
    {
        return Player.getOpponent(playerTurn);
    }

    public bool isColValid(int col)
    {
        return board.isColValid(col);
    }

    public void addPosition(int col)
    {
        board[board.getTopRow(col), col] = playerTurn;
        if (!board.isDone(playerTurn))
        {
            playerTurn = getOpponent(playerTurn);
            if (board.isFull())
            {
                done = true;
            }
        }
        else
        {
            done = true;
            playerWin = playerTurn;
        }
    }

    public override string ToString()
    {
        string str = "TURN: " + Player.playerToString(playerTurn) + "\n" + board;
        return str;
    }

}
