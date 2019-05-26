using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    public GameObject turnManager;

    public Text turnLabel;
    public Text powerupLabel;
    public Text actionPointLabel;
    public Button skipButton;

    private void Awake()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTurnLabel();
        ShowPowerup("", 0);
        skipButton.onClick.AddListener(PressSkipTurn);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkipButton();
        UpdateActionPointLabel();
    }

    public static void UpdateTurnLabel()
    {
        if (TurnManager.currentTurn == TurnManager.TurnType.MUSHROOM)
        {
            main.turnLabel.text = "Your turn !";
            main.turnLabel.color = Color.cyan;
        }
        else
        {
            main.turnLabel.text = "Cities' turn !";
            main.turnLabel.color = Color.red;
        }
    }

    private void UpdateActionPointLabel()
    {
        actionPointLabel.text = "Actions remaining: " + TurnManager.GetCurrentTeamActionPoints();
    }

    public static void ShowPowerup(string effect, float duration)
    {
        main.powerupLabel.text = "Power Up: " + effect;
        main.Invoke("HidePowerupText", duration);
    }

    private void HidePowerupText()
    {
        powerupLabel.text = "";
    }

    private void UpdateSkipButton()
    {
        skipButton.interactable = TurnManager.currentTurn == TurnManager.TurnType.MUSHROOM;
    }

    public void PressSkipTurn()
    {
        if (TurnManager.currentTurn == TurnManager.TurnType.MUSHROOM)
        {
            turnManager.GetComponent<TurnManager>().StartNewTurn();
        }
    }

    public void PressRestart()
    {
        TurnManager.cityActionPointsAmount = 0;
        TurnManager.mushroomActionPointsAmount = 5;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
