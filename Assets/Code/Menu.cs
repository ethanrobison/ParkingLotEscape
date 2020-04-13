using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private Button _start, _quit;
    void Start () {
        _start = transform.Find("Buttons/Start").GetComponent<Button>();
        _start.onClick.AddListener(StartGame);
        _quit = transform.Find("Buttons/Quit").GetComponent<Button>();
        _quit.onClick.AddListener(QuitGame);
    }
    private void StartGame () {
        SceneManager.LoadScene(1);
    }
    private void QuitGame () {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
