using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject cardPrefab;
    public Transform gridParent; // assign GridPanel
    public Button playButton;
    public Sprite[] fruitSprites; // 4 unique sprites for 8 cards

    private List<Card> allCards = new List<Card>();
    private List<Card> flippedCards = new List<Card>(); //max 2 cards (to match)

    void Awake()
    {
        instance = this;
        playButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
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
            card.fruitImage.sprite = fruitSprites[id]; // sprite matches ID
            card.ShowBack();

            obj.GetComponent<Button>().onClick.AddListener(() => card.OnCardClick());
            allCards.Add(card);
        }
    }


    public void CardClicked(Card card)
    {
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
                c.fruitImage.gameObject.SetActive(false); // hide front
                c.backImage.gameObject.SetActive(false);  // hide back
                c.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            foreach (Card c in flippedCards)
                c.ShowBack();
        }
        flippedCards.Clear();
    }
}
