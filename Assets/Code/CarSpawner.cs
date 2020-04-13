using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSpawner : MonoBehaviour
{
    private const int ROWS = Board.ROWS, COLS = Board.COLS;

    private const float BASE_INTERVAL = 3.5f, INTERVAL_MIN = 1.5f, EVOLUTION_RATE = 0.1f;

    public GameObject carPrefab;
    public Board board;
    public Player player;

    private float _timer = BASE_INTERVAL;
    private float _interval = BASE_INTERVAL;
    private void Start () {
        InitResults();
        MakeCar(4, 4, Orientation.Down);
        MakeCar(6, 0, Orientation.Up);
        MakeCar(8, 8, Orientation.Right);
        MakeCar(2, 2, Orientation.Left);
    }

    private void Update () {
        _timer -= Time.deltaTime;
        if (_timer < 0f) {
            bool success = CheckSpawnPointExists();
            if (!success) {
                SceneManager.LoadScene(2);
                return;
            }

            int range = _successess.Count - 1;
            var ind = Random.Range(0, range);
            var c = _successess[ind];
            var o = c.GetOrientation();

            MakeCar(c.row, c.col, o);
            _timer = _interval;
            _interval -= ((_interval - INTERVAL_MIN) * EVOLUTION_RATE);
        }

    }


    public GameObject MakeCar (int row, int col, Orientation orientation) {
        var targetpos = board.GetTilePos(row, col);
        var car = Instantiate(carPrefab, targetpos, Quaternion.identity, transform);

        var ccomp = car.GetComponent<Car>();
        ccomp.SetTransform(row, col, orientation);
        ccomp.board = board;
        ccomp.player = player;

        return car;
    }


    private List<CastResults> _successess = new List<CastResults>();

    private static CastResults[,] _casts = new CastResults[ROWS, COLS];
    private void InitResults () {
        for (int row = 0; row < ROWS; row++) {
            for (int col = 0; col < COLS; col++) {
                _casts[row, col] = new CastResults(row, col);
            }
        }
    }

    private void ClearResults () {
        for (int row = 0; row < ROWS; row++) {
            for (int col = 0; col < COLS; col++) {
                _casts[row, col].Clear();
            }
        }
        _successess.Clear();
    }

    private bool CheckSpawnPointExists () {
        ClearResults();
        bool result = false;

        for (Orientation o = Orientation.Up; o <= Orientation.Right; o++) {
            Vector2 size;
            Vector2 offset = Vector2.zero;
            switch (o) {
                case Orientation.Up:
                    size = new Vector2(1, 2);
                    break;
                case Orientation.Down:
                    size = new Vector2(1, 2);
                    break;
                case Orientation.Left:
                    size = new Vector2(2, 1);
                    break;
                case Orientation.Right:
                default:
                    size = new Vector2(2, 1);
                    break;
            }


            for (int row = 0; row < ROWS - 1; row++) {
                for (int col = 0; col < COLS - 1; col++) {
                    var bcresult = Physics2D.OverlapBox(new Vector2(col, row) + offset, size, 0f);
                    if (bcresult == null) {
                        result = true;
                        _casts[row, col].Set(o);
                    }
                }
            }
        }
        if (!result) { return false; }

        for (int row = 0; row < ROWS; row++) {
            for (int col = 0; col < COLS; col++) {
                if (_casts[row, col].AcceptsAny()) {
                    _successess.Add(_casts[row, col]);
                }
            }
        }

        return true;
    }

    private class CastResults
    {
        public int row, col;
        public bool[] orientations = new bool[4];

        public CastResults (int row, int col) {
            this.row = row;
            this.col = col;
        }

        public void Clear () {
            for (int i = 0; i < 4; i++) {
                orientations[i] = false;
            }
        }

        public bool AcceptsAny () {
            for (int i = 0; i < 4; i++) {
                if (orientations[i]) { return true; }
            }
            return false;
        }

        public Orientation GetOrientation () {
            int ind = Random.Range(0, 3);
            for (int i = 0; i < 4; i++) {
                var trueind = (ind + i) % 4;
                if (orientations[trueind]) { return (Orientation)trueind; }
            }

            return Orientation.Down; // Gonna puke anyway
        }

        public void Set (Orientation o, bool val = true) {
            orientations[(int)o] = val;
        }
    }
}

