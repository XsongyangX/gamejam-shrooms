using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    public Text turnLabel;
    public Text powerupLabel;

    private void Awake()
    {
        main = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateTurnLabel();
        ShowPowerup("", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateTurnLabel()
    {
        if (TurnManager.currentTurn == TurnManager.TurnType.MUSHROOM)
        {
            main.turnLabel.text = "You're turn !";
            main.turnLabel.color = Color.cyan;
        }
        else
        {
            main.turnLabel.text = "Cities' turn !";
            main.turnLabel.color = Color.red;
        }
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
}
