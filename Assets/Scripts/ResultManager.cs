using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public TMP_Text timeText; 
    public static float finalTime;

    void Start()
    {
        int minutes = Mathf.FloorToInt(finalTime / 60f);
        int seconds = Mathf.FloorToInt(finalTime % 60f);

        timeText.text = $"{minutes:00}:{seconds:00}";
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("PlayScene");
    }
}
