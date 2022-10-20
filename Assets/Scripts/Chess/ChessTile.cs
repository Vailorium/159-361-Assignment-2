using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessTile : MonoBehaviour, Interactable
{
    private const float ELEVATION = 0.025f;

    private int _x = 0;
    private int _y = 0;
    public int x {
        get => _x;
        set
        {
            float xPos = -0.21f + (0.06f * value);
            transform.localPosition = new Vector3(xPos, ELEVATION, transform.localPosition.z);
            this._x = value;
        }
    }
    public int y {
        get => _y;
        set
        {
            float zPos = -0.21f + (0.06f * value);
            transform.localPosition = new Vector3(transform.localPosition.x, ELEVATION, zPos);
            this._y = value;
        }
    }

    public void interact(PlayerController pC, GameObject obj = null)
    {
        // move piece to this tile
        pC.selectedPiece.moveTo(this.x, this.y);

        // unhighlight all tiles
        foreach(ChessTile tile in pC.selectedPiece.lastValidTiles)
        {
            tile.gameObject.SetActive(false);
        }
        pC.selectedPiece = null;

        // verify this was the correct move
        ChessGameController.VerifyMove();
    }
}
