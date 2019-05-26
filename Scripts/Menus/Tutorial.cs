using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public static Tutorial main;

    public static bool active = false;
    public static bool skipAll = false;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        active = true;
        DialogueBox.Display("Hello, welcome to SHROOMS !", "Next", OnNextHello);
    }

    private void OnNextHello()
    {
        if (skipAll) {
            CloseTutorial();
            return;
        }
        CameraController.MoveTo(TileBehaviour.core.transform.position);
        DialogueBox.Display("Let's get started right away! Click on the big Shroom !", "Close", CloseTutorial);

    }

    private bool coreClicked = false;
    public void OnClickCore()
    {
        if (skipAll)
        {
            CloseTutorial();
            return;
        }
        if (coreClicked) return;
        active = true;
        coreClicked = true;
        CameraController.TakeControl();
        DialogueBox.Display("This is your Core. \nProtect your Core! If the mean humans build their city on it, all is lost!", "Next", OnNextCore);

    }

    private void OnNextCore()
    {
        if (skipAll)
        {
            CloseTutorial();
            return;
        }
        DialogueBox.Display("Help the humans connect with Mother Nature again! Spread your Shrooms as far as you can!\n\nSee these highlighted tiles next to your Core?" +
            "\nWhen they glow blue, that means you can spread on it. Try it out!", "Close", CloseTutorial);
    }

    private bool firstSpread = false;
    public void OnFirstSpread()
    {
        if (skipAll)
        {
            CloseTutorial();
            return;
        }
        if (firstSpread) return;
        active = true;
        firstSpread = true;
        CameraController.TakeControl();
        DialogueBox.Display("Congratulations!\nYou just spread to another tile. Did you see that number change at the top of the screen?" +
            "It's the number of actions you can perform per turn. Some actions take more points than others.", "Next", OnNextSpread);

    }

    private void OnNextSpread()
    {
        if (skipAll)
        {
            CloseTutorial();
            return;
        }
        DialogueBox.Display("Spend all your points and spread as much happiness as you can!\n\nOr don't and press the skip button up above...", "Close", CloseTutorial);
    }

    private bool firstCityTurn = false;
    public void OnFirstCityTurn()
    {
        if (skipAll)
        {
            CloseTutorial();
            return;
        }
        if (firstCityTurn) return;
        active = true;
        firstCityTurn = true;
        CameraController.TakeControl();
        DialogueBox.Display("Oh no! It's the humans' turn now.\nThey will try to break your connections with the world...", "Next", OnNextCityTurn);

    }

    private void OnNextCityTurn()
    {
        if (skipAll)
        {
            CloseTutorial();
            return;
        }
        DialogueBox.Display("Be careful on your next turn! Don't let them get too close to your Core!", "Close", CloseTutorial);
    }

    private bool firstPowerUp = false;
    public void OnFirstPowerUp()
    {
        if (skipAll)
        {
            CloseTutorial();
            return;
        }
        if (firstPowerUp) return;
        active = true;
        firstPowerUp = true;
        CameraController.TakeControl();
        DialogueBox.Display("Woaw ! Did you see that? \nThat was a Power Up !", "Next", OnNextPowerUp);

    }

    private void OnNextPowerUp()
    {
        if (skipAll)
        {
            CloseTutorial();
            return;
        }
        DialogueBox.Display("Power Ups give you more Action Points to spend ! Don't let the mean humans get more than you !", "Close", CloseTutorial);
    }

    private void CloseTutorial()
    {
        CameraController.ReleaseControl();
        DialogueBox.Hide();
        active = false;
    }
}
