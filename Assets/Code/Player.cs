using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public Board board;

    private int _row = 0, _col = 0;
    private void Start () {
        MovePos();
    }

    private void Update () {
        if (Input.GetKeyDown(KeyCode.A)) {
            _col = Mathf.Clamp(_col-1, 0, 9);
            MovePos();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            _col = Mathf.Clamp(_col+1, 0, 9);
            MovePos();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            _row = Mathf.Clamp(_row+1, 0, 9);
            MovePos();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            _row = Mathf.Clamp(_row-1, 0, 9);
            MovePos();
        }
    }

    private void MovePos () {
        var targetPos = board.GetTilePos(_row, _col);
        transform.position = targetPos;
    }
    public void ForcePos(int row, int col) {
        _row = row;
        _col = col;
        MovePos();
    }
}
