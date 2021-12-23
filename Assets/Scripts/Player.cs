using System.Collections;
using System.Collections.Generic;

public static class Player
{
    public const char P0 = '0';
    public const char P1 = '1';
    public const char P2 = '2';

    public static char getOpponent(char player)
    {
        return player == P1 ? P2 : P1;
    }

    public static string playerToString(char player)
    {
        if (player == P1 || player == P2)
        {
            return "P" + player;
        }
        return "NONE";
    }

}