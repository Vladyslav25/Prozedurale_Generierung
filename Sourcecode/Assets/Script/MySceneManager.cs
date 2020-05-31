using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MySceneManager : MonoBehaviour
{
    private Text txt;
    private bool hasWinText;

    public void StartClick()
    {
        ChangeScene("1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public static void ChangeScene(string _LevelName)
    {
        SceneManager.LoadScene(_LevelName);
    }
}
