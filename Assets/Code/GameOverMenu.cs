using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    private Button _restart, _mainmenu;
    private void Start () {
        _restart = transform.Find("Buttons/Restart").GetComponent<Button>();
        _restart.onClick.AddListener(Restart);
        _mainmenu = transform.Find("Buttons/Main").GetComponent<Button>();
        _mainmenu.onClick.AddListener(GoToMainMenu);
    }
    private void Restart () {
        SceneManager.LoadScene(1);
    }
    private void GoToMainMenu () {
        SceneManager.LoadScene(0);
    }
}
