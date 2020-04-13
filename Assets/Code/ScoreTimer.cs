using TMPro;
using UnityEngine;

public class ScoreTimer : MonoBehaviour
{
    public TextMeshProUGUI current, best;
    private float _current;

    private void Awake () {
        UpdateTime();
    }
    private void Update () {
        _current += Time.deltaTime;
        UpdateTime();
    }

    private void UpdateTime () {
        current.text = $"{_current:000.00}";
    }

}
