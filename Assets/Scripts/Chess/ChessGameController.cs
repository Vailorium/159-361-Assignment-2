using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGameController : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    [SerializeField] ChessPiece[] pieces;
    [SerializeField] GameObject hiddenPanel;

    // Player is white pieces (moves first)
    public static ChessBoard board;
    private static GameObject _hiddenPanel;

    private static List<ChessPiece> allPieces;

    private static List<ChessMove> solution;

    private static List<ChessMove> blackMoves;

    // current index in the solution, reset to 0 if fail puzzle
    private static int solutionPosition;

    public static List<List<ChessTile>> tiles;

    // Start is called before the first frame update
    void Start()
    {
        hiddenPanel.SetActive(false);
        _hiddenPanel = hiddenPanel;
        solutionPosition = 0;
        allPieces = new List<ChessPiece>(this.pieces);
        board = new ChessBoard(this.pieces);

        tiles = new List<List<ChessTile>>(8);
        for(int x = 0; x < 8; x++)
        {
            tiles.Add(new List<ChessTile>(8));
            for(int y = 0; y < 8; y++)
            {
                GameObject tile = Instantiate(tilePrefab, this.transform);
                ChessTile tileScript = tile.GetComponent<ChessTile>();
                tiles[x].Add(tileScript);
                tileScript.x = y;
                tileScript.y = x;
                tile.gameObject.SetActive(false);
            }
        }

        solution = new List<ChessMove>()
        {
            new ChessMove("White Rook (1)", 7, 4),
            new ChessMove("White Pawn (1)", 2, 2),
            new ChessMove("White Rook (1)", 0, 4)
        };

        blackMoves = new List<ChessMove>()
        {
            new ChessMove("Black King (1)", 0, 3),
            new ChessMove("Black King (1)", 0, 2)
        };
    }

    /// <summary>
    /// Reset board to initial state
    /// </summary>
    private static void ResetBoard()
    {
        foreach(ChessPiece piece in allPieces)
        {
            piece.resetPosition();
        }
        solutionPosition = 0;
    }

    /// <summary>
    /// Get the script component of a piece
    /// </summary>
    /// <param name="pieceName">Object name of piece</param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException">Returns exception if cannot find piece</exception>
    private static ChessPiece GetPieceScript(string pieceName)
    {
        GameObject piece = GameObject.Find(pieceName);
        if(piece != null)
        {
            ChessPiece pieceScript = piece.GetComponent<ChessPiece>();
            if(piece != null)
            {
                return pieceScript;
            }
        }
        throw new KeyNotFoundException();
    }

    /// <summary>
    /// Verify move was correct, if move was incorrect, reset
    /// </summary>
    public static void VerifyMove()
    {
        if(solutionPosition > 2)
        {
            // bug somehow
            ResetBoard();
            return;
        }
        ChessMove solutionMove = solution[solutionPosition];

        ChessPiece pieceScript = GetPieceScript(solutionMove.chessPieceName);
        if(pieceScript.xLoc == solutionMove.moveToY && pieceScript.yLoc == solutionMove.moveToX)
        {
            // success!
            if(solutionPosition == 2)
            {
                // finish puzzle
                foreach(ChessPiece piece in allPieces)
                {
                    // make all pieces uninteractable
                    int ignoreRaycastLayer = LayerMask.NameToLayer("Ignore Raycast");
                    piece.gameObject.layer = ignoreRaycastLayer;
                }
                // show hidden panel
                _hiddenPanel.SetActive(true);
            } else
            {
                // do black move
                ChessMove blackMove = blackMoves[solutionPosition];
                ChessPiece blackPieceScript = GetPieceScript(blackMove.chessPieceName);
                blackPieceScript.moveTo(blackMove.moveToX, blackMove.moveToY);
                solutionPosition++;
            }
        } else
        {
            // failed the puzzle
            ResetBoard();
        }
    }
}
