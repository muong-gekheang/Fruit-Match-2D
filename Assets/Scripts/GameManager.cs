using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    public string sceneName;

    private float elapsedTime = 0f; // time in seconds
    private bool timerRunning = false;
    public TMP_Text timerText;

    private int matchedCount = 0;

    public static GameManager instance;

    public GameObject cardPrefab;
    public Transform gridParent; // assign GridPanel
    public Button playButton;
    public Button pauseButton;
    public Button restartButton;

    private bool isPaused = false;

    public Sprite[] fruitSprites; // 4 unique sprites for 8 cards

    private List<Card> allCards = new List<Card>();
    private List<Card> flippedCards = new List<Card>(); //max 2 cards (to match)

    void Awake()
    {
        instance = this;
        playButton.onClick.AddListener(StartGame);
        pauseButton.onClick.AddListener(TogglePause);
        restartButton.onClick.AddListener(RestartGame);

        pauseButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timerRunning && !isPaused)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }


    public void StartGame()
    {
        matchedCount = 0;

        //on game start, show only pause and restart buttons
        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        foreach (Transform child in gridParent)
            Destroy(child.gameObject);

        flippedCards.Clear();
        allCards.Clear();

        int pairCount = 6; // for 4x2 grid

        // 1. Generate IDs
        List<int> ids = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            ids.Add(i);
            ids.Add(i);
            //result: ids = [0,0,1,1,2,2,3,3]
        }

        // 2. Shuffle IDs
        ids = ids.OrderBy(x => Random.value).ToList();

        // 3. Instantiate cards using SHUFFLED ids
        // 1 fruit displayed in two cards
        for (int i = 0; i < ids.Count; i++)
        {
            GameObject obj = Instantiate(cardPrefab, gridParent);
            Card card = obj.GetComponent<Card>();

            int id = ids[i];

            card.cardID = id;
            card.frontImage.sprite = fruitSprites[id]; // sprite matches ID
            card.ShowBack();

            obj.GetComponent<Button>().onClick.AddListener(() => card.OnCardClick());
            allCards.Add(card);
        }

        // make sure game runs if coming from paused
        Time.timeScale = 1f;
        elapsedTime = 0f;
        timerRunning = true;
        UpdateTimerUI();

    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            Time.timeScale = 0f; // freeze
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f; // unfreeze
            isPaused = false;

        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // ensure normal speed
        isPaused = false;

        StartGame();
    }

    public void Exit()
    {
        SceneManager.LoadScene("StartMenu");
    }


    public void CardClicked(Card card)
    {
        if (isPaused) return; // if game is paused, cards not flipped 
        if (flippedCards.Contains(card) || flippedCards.Count >= 2)
            return;

        card.ShowFront();
        flippedCards.Add(card);

        if (flippedCards.Count == 2)
        {
            Invoke(nameof(CheckMatch), 1f);
        }
    }

    void CheckMatch()
    {
        if (flippedCards[0].cardID == flippedCards[1].cardID)
        {
            foreach (Card c in flippedCards)
            {
                c.frontImage.gameObject.SetActive(false); // hide front
                c.backImage.gameObject.SetActive(false);  // hide back
                c.GetComponent<Button>().interactable = false;
            }
            matchedCount += 2;
        }
        else
        {
            foreach (Card c in flippedCards)
                c.ShowBack();
        }
        flippedCards.Clear();

        if (matchedCount == allCards.Count)
        {
            timerRunning = false;
            ResultManager.finalTime = elapsedTime;
            SceneManager.LoadScene("ResultScene");
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

}
