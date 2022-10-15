using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMove
{
    public string chessPieceName;
    public int moveToX;
    public int moveToY;

    public ChessMove(string pieceName, int x, int y) {
        chessPieceName = pieceName;
        moveToX = x;
        moveToY = y;
    }
}
