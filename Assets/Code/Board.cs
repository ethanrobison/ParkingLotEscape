using UnityEngine;

public class Board : MonoBehaviour
{
    public const int ROWS = 10, COLS = 10;
    public GameObject TilePrefab;

    private GameObject[,] _tiles = new GameObject[10, 10];

    private void Awake () {
        DrawBoard();
    }

    private void DrawBoard () {
        for (int row = 0; row < ROWS; row++) {
            for (int col = 0; col < COLS; col++) {
                SpawnTile(row, col);
            }
        }
    }

    private void SpawnTile (int row, int col) {
        var pos = new Vector2(col, row);
        var tile = Instantiate(TilePrefab, pos, Quaternion.identity, transform);
        tile.transform.Find("Visuals").GetChild(ChildIndex(row, col)).gameObject.SetActive(true);
        _tiles[row, col] = tile;

    }

    public Vector2 GetTilePos (int row, int col) {
        return _tiles[row, col].transform.position;
    }

    /* 
     * 0000 inner
     * 1000 left
     * 0100 right
     * 0010 top
     * 0001 bottom
     * 1010 topleft
     * 0110 topright
     * ...
     */

    private int ChildIndex (int row, int col) {
        bool left = col == 0;
        bool right = col == COLS - 1;
        bool bot = row == 0;
        bool top = row == ROWS - 1;
        if (left) {
            if (top) {
                return 7;
            } else if (bot) {
                return 5;
            } else {
                return 1;
            }
        } else if (right) {
            if (top) {
                return 8;
            } else if (bot) {
                return 6;
            } else {
                return 2;
            }
        } else if (bot) {
            return 3;
        } else if (top) {
            return 4;
        } else { return 0; }
    }
}

