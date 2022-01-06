using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public static class Computer
{
    private const int Width = 7;
    private const int Height = 6;

    private const char Empty = ' ';

    private class Scores
    {
        public const int WinMove        = 1000000;
        public const int ForcedMove     =  -10000;
        public const int DoubleTrapMove =    1000;
        public const int TrapMove       =     100;
        public const int InvalidMove    = Int32.MinValue;

        int[] values = new int[Width];

        public int this[int col]
        {
            get { return values[col]; }
            set { values[col] = value; }
        }

        public Scores()
        {
            values = Enumerable.Repeat<int>(0, Width).ToArray();
        }

        public Scores(int val)
        {
            values = Enumerable.Repeat<int>(val, Width).ToArray();
        }

        public int Max()
        {
            return values.Max();
        }

        public int getBestCol()
        {
            int maxValue = Max();
            List<int> colList = new List<int>();
            for(int col=0; col<Width; ++col)
            {
                if(values[col] == maxValue)
                {
                    if (col == 3)
                    {
                        return 3;
                    }
                    colList.Add(col);
                }
            }

            if (colList.Count() == 1)
            {
                return colList[0];
            }

            return colList[UnityEngine.Random.Range(0, colList.Count())];
        }

        public override string ToString()
        {
            string str = "|";
            for(int col=0; col<Width; ++col)
            {
                str += values[col] + "|";
            }
            str += "\n";
            return str;
        }
    }

    public static (int, int) getCol(State state, int recursionLevel)
    {
        Stopwatch timer = new Stopwatch();
        timer.Start();

        State computerState = state.Clone() as State;
        Scores scores = getScores(computerState, computerState.PlayerTurn, recursionLevel);
        int col = scores.getBestCol();

        timer.Stop();

        return (col,timer.Elapsed.Seconds * 1000 + timer.Elapsed.Milliseconds);
    }

    private static Scores getScores(State state, char player, int recursionLevel)
    {
        if (recursionLevel == 0)
        {
            return new Scores(0);
        }

        Scores scores = new Scores(0);
        for (int col=0; col < Width; ++col)
        {
            scores[col] = getScoreColRec(state, col, player, recursionLevel);
        }

        return scores;
    }

    private static int getScoreColRec(State state, int col, char player, int recursionLevel)
    {
        if (!state.isColValid(col))
        {
            return Scores.InvalidMove;
        }

        State nextState = state.Clone() as State;
        nextState.addPosition(col);

        int scoreCol = getScoreCol(nextState, col);
        if (scoreCol == Scores.WinMove ||
            scoreCol == Scores.DoubleTrapMove ||
            scoreCol == Scores.ForcedMove)
        {
            return scoreCol;
        }

        Scores recScores = getScores(nextState, player, recursionLevel - 1);

        int bestRecScore = recScores.Max();
        if (player == nextState.getLastPayer())
        {
            bestRecScore = -bestRecScore;
        }

        int bestRecScoreWithFactor = Convert.ToInt32(Convert.ToDouble(bestRecScore) / 1.5); // recursion factor
        return bestRecScoreWithFactor;
    }

    private static int getScoreCol(State state, int col)
    {
        if (state.Done) // check if winning move
        {
            return Scores.WinMove;
        }

        // EVALUATION
        int score = 0;

        Board board = state.Board;
        char player = Player.getOpponent(state.PlayerTurn); // get last played
        Board opponentEvaluationBoard = getEvaluationBoard(board, Player.getOpponent(player));
        for (int evalCol=0; evalCol < Width; ++evalCol)
        {
            if (board.isColValid(evalCol))
            {
                // future win for opponent: don't play
                char opponentEvalCol = opponentEvaluationBoard[board.getTopRow(evalCol), evalCol];
                if (opponentEvalCol == 'F' || opponentEvalCol == 'D')
                {
                    return Scores.ForcedMove;
                }
            }
        }

        Board evaluationBoard = getEvaluationBoard(board, player);

        int forceMoveCount = 0;
        for (int evalCol=0; evalCol < Width; ++evalCol)
        {
            if (board.isColValid(evalCol))
            {
                char playerEvalCol = evaluationBoard[board.getTopRow(evalCol), evalCol];
                if (playerEvalCol == 'D')
                {
                    score = Math.Max(score, Scores.DoubleTrapMove);
                }
                else if (playerEvalCol == 'F')
                {
                    score = Math.Max(score, Scores.TrapMove);
                    ++forceMoveCount;
                }
            }
        }

        // seven // double lines // 3 in a row
        // --> 2 forced moves in same col
        // --> 2 forced moves in the same board
        if (forceMoveCount > 1)
        {
            score = Scores.DoubleTrapMove;
        }

        return score;
    }

    private static Board getEvaluationBoard(Board board, char player)
    {
        Board evaluationBoard = board.Clone() as Board;

        horizontalEvaluation(board, player, ref evaluationBoard);
        verticalEvaluation(board, player, ref evaluationBoard);
        diagonalEvaluation(board, player, ref evaluationBoard);
        doubleForceMoveEvaluation(board, player, ref evaluationBoard);

        return evaluationBoard;
    }

    private static void horizontalEvaluation(Board board, char player, ref Board evaluationBoard)
    {
        for (int col=0; col < Width - 3; ++col)
        {
            for (int row = Height - 1; row >= 0; --row)
            {
                if (board[row, col] == player)
                {
                    // XXX- XX-X
                    if (board[row, col + 1] == player)
                    {
                        // XXX-
                        if (board[row, col + 2] == player &&
                            board[row, col + 3] == Empty)
                        {
                            evaluationBoard[row, col + 3] = 'F';
                        }
                        // XX-X
                        else if (board[row, col + 2] == Empty &&
                                 board[row, col + 3] == player)
                        {
                            evaluationBoard[row, col + 2] = 'F';
                        }
                    }
                    // X-XX
                    else if (board[row, col + 1] == Empty  &&
                             board[row, col + 2] == player &&
                             board[row, col + 3] == player)
                    {
                        evaluationBoard[row, col + 1] = 'F';
                    }
                }
                // -XXX
                else if(board[row, col    ] == Empty &&
                        board[row, col + 1] == player &&
                        board[row, col + 2] == player &&
                        board[row, col + 3] == player)
                {
                    evaluationBoard[row, col] = 'F';
                }
            }
        }
    }

    private static void verticalEvaluation(Board board, char player, ref Board evaluationBoard)
    {
        for (int col=0; col < Width; ++col)
        {
            for (int row = Height - 1; row >= 3; --row)
            {
                // -
                // X
                // X
                // X
                if (board[row    , col] == Empty  &&
                    board[row - 1, col] == player &&
                    board[row - 2, col] == player &&
                    board[row - 3, col] == player)
                {
                    evaluationBoard[row, col] = 'F';
                }
            }
        }
    }

    private static void diagonalEvaluation(Board board, char player, ref Board evaluationBoard)
    {
        for (int col=Width-1; col >= 3; --col)
        {
            for (int row=Height-1; row >= 3; --row)
            {
                //    X    X    X
                //   X?   X?   -?
                //  X??  -??  X??
                // -??? X??? X???
                if (board[row, col] == player)
                {
                    //    X    X
                    //   X?   X?
                    //  X??  -??
                    // -??? X???
                    if (board[row - 1, col - 1] == player)
                    {
                        //    X
                        //   X?
                        //  X??
                        // -???
                        if (board[row - 2, col - 2] == player &&
                            board[row - 3, col - 3] == Empty)
                        {
                            evaluationBoard[row - 3, col - 3] = 'F';
                        }
                        //    X
                        //   X?
                        //  -??
                        // X???
                        else if (board[row - 2, col - 2] == Empty &&
                                 board[row - 3, col - 3] == player)
                        {
                            evaluationBoard[row - 2, col - 2] = 'F';
                        }
                    }
                    //    X
                    //   -?
                    //  X??
                    // X???
                    else if (board[row - 1, col - 1] == Empty &&
                             board[row - 2, col - 2] == player &&
                             board[row - 3, col - 3] == player)
                    {
                        evaluationBoard[row - 1, col - 1] = 'F';
                    }
                }
                //    -
                //   X?
                //  X??
                // X???
                else if (board[row    , col    ] == Empty &&
                         board[row - 1, col - 1] == player &&
                         board[row - 2, col - 2] == player &&
                         board[row - 3, col - 3] == player)
                {
                    evaluationBoard[row, col] = 'F';
                }
            }
        }

        for (int col=0; col < Width - 3; ++col)
        {
            for (int row=Height-1; row >= 3; --row)
            {
                // X    X    X
                // ?X   ?X   ?-
                // ??X  ??-  ??X
                // ???- ???X ???X
                if (board[row, col] == player)
                {
                    // X    X
                    // ?X   ?X
                    // ??X  ??-
                    // ???- ???X
                    if (board[row - 1, col + 1] == player)
                    {
                        // X
                        // ?X
                        // ??X
                        // ???-
                        if (board[row - 2, col + 2] == player &&
                            board[row - 3, col + 3] == Empty)
                        {
                            evaluationBoard[row - 3, col + 3] = 'F';
                        }
                        // X
                        // ?X
                        // ??-
                        // ???X
                        else if (board[row - 2, col + 2] == Empty &&
                                 board[row - 3, col + 3] == player)
                        {
                            evaluationBoard[row - 2, col + 2] = 'F';
                        }
                    }
                    // X
                    // ?-
                    // ??X
                    // ???X
                    else if (board[row - 1, col + 1] == Empty &&
                             board[row - 2, col + 2] == player &&
                             board[row - 3, col + 3] == player)
                    {
                        evaluationBoard[row - 1, col + 1] = 'F';
                    }
                }
                // -
                // ?X
                // ??X
                // ???X
                else if (board[row    , col    ] == Empty &&
                         board[row - 1, col + 1] == player &&
                         board[row - 2, col + 2] == player &&
                         board[row - 3, col + 3] == player)
                {
                    evaluationBoard[row, col] = 'F';
                }
            }
        }
    }

    private static void doubleForceMoveEvaluation(Board board, char player, ref Board evaluationBoard)
    {
        for (int col=0; col < Width; ++col)
        {
            for (int row = Height - 1; row >= 1; --row)
            {
                if (evaluationBoard[row    , col] == 'F' &&
                    evaluationBoard[row - 1, col] == 'F')
                {
                    evaluationBoard[row - 1, col] = 'D';
                }
            }
        }
    }
}
