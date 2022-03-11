using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    //Get the Player's choice and set it as his choice when player clicks on one of the player input
    public void GetPlayerChoice()
    {
        string playerChoice = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;

        print("Player Selected: " + playerChoice);
        GameplayController.Instance.SetPlayerChoice(playerChoice);
    }

    //Start default campaign when player clicks on play in home screen
    public void StartCampaignMode()
    {
        ScreenManager.Instance.GoToGameScreenFromHomeScreen();
        GameplayController.Instance.StartCampaign();
    }

    //Start campaign against Sheldon when player clicks on play against Sheldon in home screen
    public void StartCampaignModeAgainstSheldon()
    {
        ScreenManager.Instance.GoToGameScreenFromHomeScreen();
        GameplayController.Instance.StartCampaign(opponent:"Sheldon");
    }

    //Start default campaign when player clicks on play in home screen
    public void PlayAgain()
    {
        ScreenManager.Instance.GoToGameScreenFromResultsScreen();
        GameplayController.Instance.StartCampaign();
    }

    public void GoToHomeScreen()
    {
        ScreenManager.Instance.GoToHomeScreenFromResultsScreen();
    }

}
