using UnityEngine;

public enum Orientation
{
    Up, Down, Left, Right
}
public class Car : MonoBehaviour
{
    // TODO don't exit into the void
    // TODO don't move cars into one another
    // TODO don't exit into other cars?
    private const int ROWS = Board.ROWS, COLS = Board.COLS;

    public Board board;
    public Player player;

    private bool _playerIn = false;
    private int _row, _col;
    private Orientation _orientation = Orientation.Up;

    private void Start () {
        MovePos();
    }

    private void Update () {
        if (!_playerIn) { return; }
        switch (_orientation) {
            case Orientation.Up:
            case Orientation.Down:
                if (Input.GetKeyDown(KeyCode.W)) {
                    TryApplyDeltaAndMovePos(1, 0);
                }
                if (Input.GetKeyDown(KeyCode.S)) {
                    TryApplyDeltaAndMovePos(-1, 0);
                }
                if (Input.GetKeyDown(KeyCode.D)) {
                    TryExitPlayer(0, 1);
                }
                if (Input.GetKeyDown(KeyCode.A)) {
                    TryExitPlayer(0, -1);
                }
                break;
            case Orientation.Left:
            case Orientation.Right:
                if (Input.GetKeyDown(KeyCode.A)) {
                    TryApplyDeltaAndMovePos(0, -1);
                }
                if (Input.GetKeyDown(KeyCode.D)) {
                    TryApplyDeltaAndMovePos(0, 1);
                }
                if (Input.GetKeyDown(KeyCode.W)) {
                    TryExitPlayer(1, 0);
                }
                if (Input.GetKeyDown(KeyCode.S)) {
                    TryExitPlayer(-1, 0);
                }
                break;
        }
    }

    private void TryApplyDeltaAndMovePos (int rowd, int cold) {
        int newrow = _row + rowd;
        int newcol = _col + cold;
        if (newrow < 0) {
            if (_orientation == Orientation.Up) {
                DespawnCar();
                return;
            } else {
                newrow = 0;
            }
        } else if (newrow > ROWS - 2) {
            if (_orientation == Orientation.Down) {
                DespawnCar();
                return;
            } else {
                newrow = ROWS - 2;
            }
        } else if (newcol < 0) {
            if (_orientation == Orientation.Right) {
                DespawnCar();
                return;
            } else {
                newcol = 0;
            }
        } else if (newcol > COLS - 2) {
            if (_orientation == Orientation.Left) {
                DespawnCar();
                return;
            } else {
                newcol = COLS - 2;
            }
        }

        Vector2 offset = new Vector2(cold, rowd);
        switch (_orientation) {
            case Orientation.Down:
            case Orientation.Up:
                if (rowd < 0) { offset = new Vector2(0, 0); }
                break;
            case Orientation.Left:
            case Orientation.Right:
                if (cold < 0) { offset = new Vector2(0, 0); }
                break;
        }

        var bcresult = Physics2D.OverlapBox(new Vector2(newcol, newrow) + offset, Vector2.one, 0f);
        if (bcresult == null) {
            _row = newrow;
            _col = newcol;
            MovePos();
        }
    }
    private void MovePos () {
        Vector2 offset;
        switch (_orientation) {
            case Orientation.Up:
                offset = new Vector2(0f, 0.5f);
                break;
            case Orientation.Down:
                offset = new Vector2(0f, 0.5f);
                break;
            case Orientation.Left:
                offset = new Vector2(0.5f, 0f);
                break;
            case Orientation.Right:
                offset = new Vector2(0.5f, 0f);
                break;
            default:
                offset = Vector2.zero;
                break;
        }
        var targetPos = board.GetTilePos(_row, _col);
        transform.position = targetPos + offset;
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        var other = collision.GetComponent<Player>();
        if (other == null) { return; }
        other.enabled = false;
        other.transform.SetParent(transform);
        EnterPlayer();
    }
    public void EnterPlayer () {
        _playerIn = true;
        player.ForcePos(_row, _col);
    }

    public void TryExitPlayer (int rowd, int cold) {
        int newrow = _row + rowd;
        int newcol = _col + cold;

        if (newrow < 0 || newrow >= ROWS || newcol < 0 || newcol >= COLS) {
            // TODO play SFX?
            return;
        }
        _playerIn = false;
        player.enabled = true;
        player.transform.SetParent(null);
        player.ForcePos(newrow, newcol);
    }

    private void DespawnCar () {
        // TODO VFX/SFX
        TryExitPlayer(0, 0);
        Destroy(gameObject);
    }

    public void SetTransform (int row, int col, Orientation orientation) {
        _row = row;
        _col = col;
        _orientation = orientation;
        float rot;
        switch (orientation) {
            case Orientation.Up:
                rot = 90f;
                break;
            case Orientation.Down:
                rot = -90f;
                break;
            case Orientation.Left:
                rot = 180f;
                break;
            case Orientation.Right:
            default:
                rot = 0f;
                break;
        }
        transform.rotation = Quaternion.Euler(0f, 0f, rot);
    }
}
