using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    public static DialogueBox main;

    public Text text;

    //public Button leftButton;
    //private Text leftText;
    //public Button rightButton;
    //private Text rightText;
    public Button midButton;
    private Text midText;

    //public RawImage portraitDisplay;
    //public Texture portrait;

    public string boxContent = "";
    private string boxDisplay = "";
    public float displaySpeed = 0.1f;
    public int beforeButtonCount = 10;
    private bool buttonDisplayed = false;

    private void DisplayCharacter()
    {
        int wID = boxDisplay.Length;

        if (wID < boxContent.Length)  boxDisplay += boxContent[wID];

        if (boxDisplay.Length >= boxContent.Length)
        {
            ShowButtons();
            buttonDisplayed = true;
        }
        else
        {
            if (boxDisplay.Length >= boxContent.Length - beforeButtonCount && !buttonDisplayed)
            {
                ShowButtons();
                buttonDisplayed = true;
            }
            Invoke("DisplayCharacter", displaySpeed);
        }
    }

    private void Erase()
    {
        boxContent = "";
        boxDisplay = "";
        //leftButton.gameObject.SetActive(false);
        //rightButton.gameObject.SetActive(false);
        midButton.gameObject.SetActive(false);
        buttonDisplayed = false;
    }

    public delegate void OnButtonPressed();

    //public OnButtonPressed onLeftButtonPressed;
    public OnButtonPressed onMidButtonPressed;
    //public OnButtonPressed onRightButtonPressed;

    public static void DoNothing()
    {

    }

    public void LeftButtonPressed()
    {
        //onLeftButtonPressed();
    }

    public void MidButtonPressed()
    {
        onMidButtonPressed();
    }

    public void RightButtonPressed()
    {
        //onRightButtonPressed();
    }

    public static void SetPortrait(Texture image)
    {
        //main.portraitDisplay.texture = image;
        //main.portrait = image;
    }

    public static void Display(string text, string midOption, OnButtonPressed midPressed)
    {
        main.gameObject.SetActive(true);

        main.Erase();
        main.boxContent = text;
        main.DisplayCharacter();

        //main.leftText.text = leftOption;
        main.midText.text = midOption;
        //main.rightText.text = rightOption;

        //main.onLeftButtonPressed = leftPressed;
        main.onMidButtonPressed = midPressed;
        //main.onRightButtonPressed = rightPressed;
    }

    private void ShowButtons()
    {
        //leftButton.gameObject.SetActive(leftText.text.Length > 0);
        //rightButton.gameObject.SetActive(rightText.text.Length > 0);
        midButton.gameObject.SetActive(midText.text.Length > 0);
    }

    public static void Hide()
    {
        main.gameObject.SetActive(false);
    }

    private void Awake()
    {
        main = this;

        //leftText = leftButton.GetComponentInChildren<Text>();
        //rightText = rightButton.GetComponentInChildren<Text>();
        midText = midButton.GetComponentInChildren<Text>();

        midButton.onClick.AddListener(MidButtonPressed);

        Hide();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        text.text = boxDisplay;
    }

    public void PressedSkip()
    {
        Tutorial.skipAll = true;
        Tutorial.main.CloseTutorial();
    }
}
