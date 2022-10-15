using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour, Interactable
{

    [SerializeField] ChessPieceType pieceType;
    [SerializeField] public ChessPieceTeam team;

    private const float TILESIZE = 0.06f;

    public int xLoc;
    public int yLoc;

    // original coords to reset to if failed puzzle
    public int originalXLoc;
    public int originalYLoc;

    public List<ChessTile> lastValidTiles;

    public void Start()
    {
        lastValidTiles = new List<ChessTile>();
    }

    /// <summary>
    /// Moves chess piece transform, updates position
    /// </summary>
    /// <param name="x">X coordinate in chessboard array</param>
    /// <param name="y">Y coordinate in chessboard array</param>
    public void moveTo(int x, int y)
    {
        ChessGameController.board.board[this.xLoc][this.yLoc] = null;
        // y maps to z axis
        transform.localPosition = new Vector3(-0.21f + (x * TILESIZE), 0.016f, -0.21f + (y * TILESIZE));

        xLoc = y;
        yLoc = x;
        ChessGameController.board.board[this.xLoc][this.yLoc] = this;
    }

    /// <summary>
    /// Is the location on the board (i.e. are x and y > 0 and < 8
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns>True if on board, false if not</returns>
    private bool isLocationOnBoard(int x, int y)
    {
        return x > 0 && y > 0 && x < 8 && y < 8;
    }

    /// <summary>
    /// Basic valid tiles to move to for piece - Only which is needed for the puzzle
    /// </summary>
    /// <returns>List of tiles piece can legally move to</returns>
    public List<ChessTile> GetValidTiles()
    {
        List<ChessTile> tiles = new List<ChessTile>();
        int toMinX = this.xLoc + 1;
        int toMinY = this.yLoc + 1;
        int toMaxX = 8 - this.xLoc;
        int toMaxY = 8 - this.yLoc;
        switch (this.pieceType)
        {
            case ChessPieceType.Pawn:
                int yDirection = this.team == ChessPieceTeam.White ? -1 : 1;

                if(ChessGameController.board.IsTileFree(this.xLoc, this.yLoc + yDirection))
                {
                    tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc + yDirection]);
                }
                break;
            case ChessPieceType.Knight:
                if (ChessGameController.board.IsTileFree(this.xLoc + 1, this.yLoc + 2, this.team))
                    tiles.Add(ChessGameController.tiles[this.xLoc + 1][this.yLoc + 2]);
                if (ChessGameController.board.IsTileFree(this.xLoc + 2, this.yLoc + 1, this.team))
                    tiles.Add(ChessGameController.tiles[this.xLoc + 2][this.yLoc + 1]);
                if (ChessGameController.board.IsTileFree(this.xLoc + 1, this.yLoc - 2, this.team))
                    tiles.Add(ChessGameController.tiles[this.xLoc + 1][this.yLoc - 2]);
                if (ChessGameController.board.IsTileFree(this.xLoc + 2, this.yLoc - 1, this.team))
                    tiles.Add(ChessGameController.tiles[this.xLoc + 2][this.yLoc - 1]);
                if (ChessGameController.board.IsTileFree(this.xLoc - 1, this.yLoc + 2, this.team))
                    tiles.Add(ChessGameController.tiles[this.xLoc - 1][this.yLoc + 2]);
                if (ChessGameController.board.IsTileFree(this.xLoc - 2, this.yLoc + 1, this.team))
                    tiles.Add(ChessGameController.tiles[this.xLoc - 2][this.yLoc + 1]);
                if (ChessGameController.board.IsTileFree(this.xLoc - 1, this.yLoc - 2, this.team))
                    tiles.Add(ChessGameController.tiles[this.xLoc - 1][this.yLoc - 2]);
                if (ChessGameController.board.IsTileFree(this.xLoc - 2, this.yLoc - 1, this.team))
                    tiles.Add(ChessGameController.tiles[this.xLoc - 2][this.yLoc - 1]);
                break;
            case ChessPieceType.Rook:
                for(int d = 1; d < toMinX; d++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc - d, this.yLoc))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc - d][this.yLoc]);
                    } else if(ChessGameController.board.IsTileFree(this.xLoc - d, this.yLoc, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc - d][this.yLoc]);
                        break;
                    } else
                    {
                        // otherwise, tile has ally so break from loop
                        break;
                    }
                }
                for (int d = 1; d < toMinY; d++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc, this.yLoc - d))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc - d]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc, this.yLoc - d, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc - d]);
                        break;
                    } else
                    {
                        // otherwise, tile has ally so break from loop
                        break;
                    }
                }
                for (int d = 1; d < toMaxX; d++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc + d, this.yLoc))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc + d][this.yLoc]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc + d, this.yLoc, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc + d][this.yLoc]);
                        break;
                    } else
                    {
                        // otherwise, tile has ally so break from loop
                        break;
                    }
                }
                for (int d = 1; d < toMaxY; d++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc, this.yLoc + d))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc + d]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc, this.yLoc + d, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc + d]);
                        break;
                    } else
                    {
                        // otherwise, tile has ally so break from loop
                        break;
                    }
                }
                break;
            case ChessPieceType.Bishop:
                for (int dX = 1; dX < toMinX; dX++)
                {
                    if(ChessGameController.board.IsTileFree(this.xLoc - dX, this.yLoc - dX)) {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc - dX][this.yLoc - dX]);
                    } else if (ChessGameController.board.IsTileFree(this.xLoc - dX, this.yLoc - dX, this.team)) {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc - dX][this.yLoc - dX]);
                        break;
                    }
                }
                for (int dX = 1; dX < toMinX; dX++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc - dX, this.yLoc + dX))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc - dX][this.yLoc + dX]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc - dX, this.yLoc + dX, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc - dX][this.yLoc + dX]);
                        break;
                    }
                }
                for (int dX = 1; dX < toMaxX; dX++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc + dX, this.yLoc - dX))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc + dX][this.yLoc - dX]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc + dX, this.yLoc - dX, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc + dX][this.yLoc - dX]);
                        break;
                    }
                }
                for (int dX = 1; dX < toMaxX; dX++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc + dX, this.yLoc + dX))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc + dX][this.yLoc + dX]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc + dX, this.yLoc + dX, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc + dX][this.yLoc + dX]);
                        break;
                    }
                }
                break;
            case ChessPieceType.Queen:
                // no queen lol
                break;
            case ChessPieceType.King:
                int kingMinX = Mathf.Min(toMinX, 2);
                int kingMinY = Mathf.Min(toMinY, 2);
                int kingMaxX = Mathf.Min(toMaxX, 2);
                int kingMaxY = Mathf.Min(toMaxY, 2);
                for (int d = 1; d < kingMinX; d++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc - d, this.yLoc - d))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc - d][this.yLoc - d]);
                    }
                    if (ChessGameController.board.IsTileFree(this.xLoc - d, this.yLoc + d))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc - d][this.yLoc + d]);
                    }
                    if (ChessGameController.board.IsTileFree(this.xLoc - d, this.yLoc))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc - d][this.yLoc]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc - d, this.yLoc, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc - d][this.yLoc]);
                        break;
                    }
                    else
                    {
                        // otherwise, tile has ally so break from loop
                        break;
                    }
                }
                for (int d = 1; d < kingMinY; d++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc, this.yLoc - d))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc - d]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc, this.yLoc - d, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc - d]);
                        break;
                    }
                    else
                    {
                        // otherwise, tile has ally so break from loop
                        break;
                    }
                }
                for (int d = 1; d < kingMaxX; d++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc + d, this.yLoc - d))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc + d][this.yLoc - d]);
                    }
                    if (ChessGameController.board.IsTileFree(this.xLoc + d, this.yLoc + d))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc + d][this.yLoc + d]);
                    }
                    if (ChessGameController.board.IsTileFree(this.xLoc + d, this.yLoc))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc + d][this.yLoc]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc + d, this.yLoc, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc + d][this.yLoc]);
                        break;
                    }
                    else
                    {
                        // otherwise, tile has ally so break from loop
                        break;
                    }
                }
                for (int d = 1; d < kingMaxY; d++)
                {
                    if (ChessGameController.board.IsTileFree(this.xLoc, this.yLoc + d))
                    {
                        // if tile is empty
                        tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc + d]);
                    }
                    else if (ChessGameController.board.IsTileFree(this.xLoc, this.yLoc + d, this.team))
                    {
                        // otherwise, if tile has an opponent, break from loop
                        tiles.Add(ChessGameController.tiles[this.xLoc][this.yLoc + d]);
                        break;
                    }
                    else
                    {
                        // otherwise, tile has ally so break from loop
                        break;
                    }
                }
                break;
        }
        lastValidTiles = tiles;
        return tiles;
    }

    public void resetPosition()
    {
        this.moveTo(originalYLoc, originalXLoc);
    }

    public void interact(PlayerController pC = null, GameObject obj = null)
    {
        if(pC.selectedPiece != null)
        {
            // unhighlight all tiles
            foreach (ChessTile tile in pC.selectedPiece.lastValidTiles)
            {
                tile.gameObject.SetActive(false);
            }
        }
        pC.selectedPiece = this;

        List<ChessTile> tiles = this.GetValidTiles();
        foreach(ChessTile tile in tiles)
        {
            tile.gameObject.SetActive(true);
        }
    }
}
