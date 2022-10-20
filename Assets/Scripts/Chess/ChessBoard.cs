using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoard
{
    public List<List<ChessPiece>> board;

    public ChessBoard(ChessPiece[] pieces)
    {
        // create board and place pieces on board
        board = new List<List<ChessPiece>>(8);

        for(int i = 0; i < 8; i++)
        {
            board.Add(new List<ChessPiece>(8) { null, null, null, null, null, null, null, null});
        }

        foreach(ChessPiece piece in pieces)
        {
            board[piece.xLoc][piece.yLoc] = piece;
        }
    }

    /// <summary>
    /// Check if tile is empty or off board
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>True if tile is empty, false if tile is not or tile is off-board</returns>
    public bool IsTileFree(int x, int y)
    {
        if(x < 0 || y < 0 || x > 7 || y > 7)
        {
            return false;
        }
        return board[x][y] == null;
    }

    /// <summary>
    /// Check if tile is free OR has an enemy on it
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="team">Team of piece</param>
    /// <returns>True if tile has enemy or is free, false if tile is off board or is an ally</returns>
    public bool IsTileFree(int x, int y, ChessPieceTeam team)
    {
        if (x < 0 || y < 0 || x > 7 || y > 7)
        {
            return false;
        }
        return board[x][y] == null || board[x][y].team != team;
    }
}
