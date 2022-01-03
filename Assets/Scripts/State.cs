using System;
using System.Collections;
using System.Collections.Generic;
using static Player;

public class State : ICloneable
{
    private  bool  done = false;
    private  char  playerWin = Player.P0;
    private  char  playerTurn;

    private Board board;

    public State(char firstTurn)
    {
        playerTurn = firstTurn;
        board = new Board();
    }

    public State(State state)
    {
        done = state.done;
        playerWin = state.playerWin;
        playerTurn = state.playerTurn;
        board = state.board.Clone() as Board;
    }

    public object Clone()
    {
        return new State(this);
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

    public Board Board
    {
        get => board;
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
        bool isDone = board.isDone(playerTurn);
        if (!isDone)
        {
            if (board.isFull())
            {
                done = true;
            }
            else
            {
                playerTurn = getOpponent(playerTurn);
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
