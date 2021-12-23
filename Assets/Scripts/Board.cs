using System.Collections;
using System.Collections.Generic;

public class Board
{
    private const int Width = 7;
    private const int Height = 6;

    private const char Empty = ' ';

    char[,] arr = new char[Height, Width];

    public Board()
    {
        for (int row=0; row < Height; ++row)
        {
            for (int col=0; col < Width; ++col)
            {
                arr[row, col] = Empty;
            }
        }
    }

   public char this[int row, int col]
   {
      get { return arr[row, col]; }
      set { arr[row, col] = value; }
   }

    public bool isColValid(int col)
    {
        return arr[Height - 1, col] == Empty;
    }

    public int getTopRow(int col)
    {
        for (int row=0; row < Height; ++row)
        {
            if (arr[row, col] == Empty)
            {
                return row;
            }
        }
        return Height - 1;
    }

    public bool addPosition(int col, char player)
    {
        if (isColValid(col))
        {
            arr[getTopRow(col), col] = player;
            return true;
        }
        return false;
    }

    public bool isDone(char player)
    {
        // check col
        for (int row=0; row < 3; ++row)
        {
            for (int col=0; col < Width; ++col)
            {
                if (arr[row  , col] == player &&
                    arr[row+1, col] == player &&
                    arr[row+2, col] == player &&
                    arr[row+3, col] == player)
                {
                    return true;
                }
            }
        }

        // check row
        for (int row=0; row < Height; ++row)
        {
            for (int col=0; col < 4; ++col)
            {
                if (arr[row, col  ] == player &&
                    arr[row, col+1] == player &&
                    arr[row, col+2] == player &&
                    arr[row, col+3] == player)
                {
                    return true;
                }
            }
        }

        // check diag
        for (int row=0; row < 3; ++row)
        {
            for (int col=0; col < 4; ++col)
            {
                if (arr[row  , col  ] == player &&
                    arr[row+1, col+1] == player &&
                    arr[row+2, col+2] == player &&
                    arr[row+3, col+3] == player)
                {
                    return true;
                }
            }
        }
        for (int row=0; row < 3; ++row)
        {
            for (int col=3; col < Width; ++col)
            {
                if (arr[row  , col  ] == player &&
                    arr[row+1, col-1] == player &&
                    arr[row+2, col-2] == player &&
                    arr[row+3, col-3] == player)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool isFull()
    {
        for (int row=0; row < Height; ++row)
        {
            for (int col=0; col < Width; ++col)
            {
                if (arr[row, col] == Empty)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public override string ToString()
    {
        string str = "|-|-|-|-|-|-|-|\n";
        for (int row = Height - 1; row >= 0; --row)
        {
            for (int col=0; col < Width; ++col)
            {
                // double space for Unity Console
                str += arr[row, col] == Empty ? "|  " : "|" + arr[row, col];
            }
            str += "|\n";
        }
        str += "|-|-|-|-|-|-|-|\n";
        return str;
    }

}
