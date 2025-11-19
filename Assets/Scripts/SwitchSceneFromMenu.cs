using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuSceneSwticher : MonoBehaviour
{
    public string sceneName = "SampleScene"; // assign in Inspector

    public void SwitchScene()
    {
        Debug.Log("Switching to scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}
