using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image fruitImage; // Front
    public Image backImage;  // Back
    [HideInInspector] public int cardID; // used for matching

    private bool isFlipped = false;
    public bool IsFlipped => isFlipped;

    public void ShowFront()
    {
        fruitImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
        isFlipped = true;
    }

    public void ShowBack()
    {
        fruitImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
        isFlipped = false;
    }

    public void OnCardClick()
    {
        if (!isFlipped)
        {
            GameManager.instance.CardClicked(this);
        }
    }
}
