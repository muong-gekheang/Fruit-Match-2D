using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public void GoToPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
