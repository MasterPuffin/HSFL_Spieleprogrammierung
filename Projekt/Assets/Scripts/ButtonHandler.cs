using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Button Handler
 * Author: Johannes Bluhm
 * Handles the UI button interactions
 */

public class ButtonHandler : MonoBehaviour {
    private GameManager gameManager;

    public void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    //Load the next level
    public void NextLevel() {
        if (gameManager.currentLevel == gameManager.maxLevel || gameManager.currentLevel == 0) {
            ChangeScene("End");
        } else {
            ChangeScene("Level" + (gameManager.currentLevel + 1));
        }
    }

    //Restart the current level
    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Loads a scene with a specific name
    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    //Unpause game
    public void UnPause() {
        gameManager.UIUnpause();
    }

    //Shows the controls menu inside the pause menu
    public void ShowControls() {
        GameObject.Find("/UI/PauseScreen/Pause").GetComponent<Canvas>().enabled = false;
        GameObject.Find("/UI/PauseScreen/Controls").GetComponent<Canvas>().enabled = true;
    }

    //Hides the controls menu inside the pause menu
    public void HideControls() {
        GameObject.Find("/UI/PauseScreen/Pause").GetComponent<Canvas>().enabled = true;
        GameObject.Find("/UI/PauseScreen/Controls").GetComponent<Canvas>().enabled = false;
    }
    
    //Loads the main menu
    public void Menu() {
        ChangeScene("Start");
    }

    //Guess what...
    public void Quit() {
        Application.Quit();
    }
}