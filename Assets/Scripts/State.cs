using System.Collections;
using System.Collections.Generic;
using static Player;

public class State
{
    public  bool  done = false;
    public  char  playerWin = Player.P0;
    public  char  playerTurn;

    private Board board;

    public State(char firstTurn)
    {
        playerTurn = firstTurn;
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

}
