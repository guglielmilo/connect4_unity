using System;
using System.Collections;
using System.Collections.Generic;

public static class Computer
{
    public static int getCol(State state, int recursionLevel)
    {
        int col;
        do // simulate computer
        {
            col = UnityEngine.Random.Range(0, 6);
        }
        while(!state.isColValid(col));
        return col;
    }

    private const int Width = 7;
    private const int Height = 6;

    private const char Empty = ' ';

    private class Scores
    {
        private const int WinMove        = 1000000;
        private const int ForcedMove     =  -10000;
        private const int DoubleTrapMove =    1000;
        private const int TrapMove       =     100;
        private const int InvalidMove    = Int32.MinValue;

        int[] values = new int[Width];
    }
}
