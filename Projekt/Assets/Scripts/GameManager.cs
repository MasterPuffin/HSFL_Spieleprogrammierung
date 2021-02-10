using System;
using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/*
 * Game Manager
 * Author: Johannes Bluhm
 * Responsible for keeping everything together (current state of the game)
 */

public class GameManager : MonoBehaviour {
    //Max hp for the player
    public float hp = 100.0f;

    //Maximum level in the game. Used to check if there is a next level after a level has been completed
    public readonly int maxLevel = 3;

    //The number of the current level. Used to calculate which the next level is (current level + 1)
    public int currentLevel;

    //The score of the player (time needed for the level)
    public float score;

    private GameObject hud;

    private TextMeshProUGUI valueScore;
    private TextMeshProUGUI valueHp;

    private GameObject freecamIndicator;

    private GameObject successScreen;
    private GameObject crashScreen;
    private GameObject pauseScreen;

    //Used to skip hud updates and score count until the player is ready to play
    private bool initialized = false;

    //Pause state of the game
    private bool paused = false;

    //If the scene is a game or static scene
    private bool gameScene = true;

    //Initialize the GM. Executed by the player controller
    public void Init() {
        //Special Handling for non game scenes
        switch (SceneManager.GetActiveScene().name) {
            case "End":
                //Set total score text
                GameObject.Find("/UI/ValueScore").GetComponent<TextMeshProUGUI>().text =
                    Math.Round(GameData.totalTime, 2).ToString(CultureInfo.InvariantCulture);
                gameScene = false;
                break;
            case "Start":
                gameScene = false;
                break;
            default:
                hud = GameObject.Find("/UI/HUD");
                valueScore = GameObject.Find("/UI/HUD/ValueScore").GetComponent<TextMeshProUGUI>();
                valueHp = GameObject.Find("/UI/HUD/ValueHP").GetComponent<TextMeshProUGUI>();
                freecamIndicator = GameObject.Find("/UI/HUD/FreecamIndicator").gameObject;

                successScreen = GameObject.Find("/UI/SuccessScreen");
                crashScreen = GameObject.Find("/UI/CrashScreen");
                pauseScreen = GameObject.Find("/UI/PauseScreen");

                successScreen.SetActive(false);
                crashScreen.SetActive(false);
                pauseScreen.SetActive(false);

                score = 0.0f;
                Unpause();
                initialized = true;
                break;
        }
    }

    private void Update() {
        if (initialized && gameScene) {
            //Update the hud
            score += 1 * Time.deltaTime;
            UpdateHud();
        }
    }

    public void SetFreecamIndicator(bool state) {
        //Ignore if not a game scene
        if (!gameScene) {
            return;
        }

        freecamIndicator.SetActive(state);
    }

    //Methode toggled by camera controller to enable / disable the freecam status icon
    public void ToggleFreecamIndicator() {
        freecamIndicator.SetActive(!freecamIndicator.activeSelf);
    }

    //Player has finished a level
    public void Finish() {
        //Set score text on the success screen
        successScreen.transform.Find("ValueScore").GetComponent<TextMeshProUGUI>().text =
            Math.Round(score, 2).ToString(CultureInfo.InvariantCulture);
        //Show the success screen
        successScreen.SetActive(true);

        hud.SetActive(false);

        //Save levels score
        GameData.totalTime += score;

        Pause();
    }

    //Player has died
    public void Crash() {
        crashScreen.SetActive(true);
        hud.SetActive(false);
        Pause();
    }

    //Update the hud
    public void UpdateHud() {
        valueScore.text = Math.Round(score, 2).ToString(CultureInfo.InvariantCulture);
        valueHp.text = CalculateDisplayScore(hp).ToString();
    }

    //Convert the games value to a UI friendly value
    public int CalculateDisplayScore(float value) {
        return (int) Mathf.Floor(value);
    }

    //Pause handler for ESC keyboard press
    private void OnPause() {
        if (paused) {
            Unpause();
            paused = false;
            pauseScreen.SetActive(false);
        } else {
            Pause();
            paused = true;
            pauseScreen.SetActive(true);
        }
    }

    //Unpause the game when the user clicks on the continue button in the pause menu
    public void UIUnpause() {
        Unpause();
        paused = false;
        pauseScreen.SetActive(false);
    }

    //Pauses the game by freezing the game
    private void Pause() {
        Time.timeScale = 0;
    }

    //Take a guess..
    private void Unpause() {
        Time.timeScale = 1;
    }
}