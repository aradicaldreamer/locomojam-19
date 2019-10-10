using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SceneSwapper : MonoBehaviour{

    [SerializeField] private Canvas howToPlay;
    [SerializeField] private Canvas credits;
    [SerializeField] private Canvas options;
    [SerializeField] private Canvas pause;
    [SerializeField] private Canvas gameEnd;
    [SerializeField] private Canvas gameUI;
    [SerializeField] private Canvas carChoice;
    [SerializeField] private GameObject setUpTimer;
    public GameObject menuInter;


    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameScene() {
        SceneManager.LoadScene("GameScene");
    }

    public void Credits(bool open) { // Open up the credits gui
        //CloseCanvasMenu();
        //credits.gameObject.SetActive(open)
        SceneManager.LoadScene("Credits");
    }

    public void ReturnToMainMenu()
    {
        if (SceneManager.GetActiveScene().name.Equals("Credits"))
        {
            
        }
    }

    public void HowToPlay(bool open) { //Open the how to play GUI
//        CloseCanvasMenu();
//        howToPlay.gameObject.SetActive(open);
        SceneManager.LoadScene("HowToPlay");
    }

    public void Options(bool open) { //Open the how to play GUI
        CloseCanvasGame();
        options.gameObject.SetActive(open);
    }

    public void Pause(bool open) { //Pause the gameplay and open pause menu
        //Pause Gameplay
        CloseCanvasGame();
        pause.gameObject.SetActive(open);
    }

    public void Unpause(bool open) { //Pause the gameplay and open pause menu
        //UNPAUSE Gameplay
        CloseCanvasGame();
        //pause.gameObject.SetActive(open);
    }

    public void Restart() {
        CloseCanvasGame();
        //RESTART 
    }

    public void GameEnd(bool open) { //Screen for when game is over
        CloseCanvasGame();
        gameEnd.gameObject.SetActive(open);
    }

    public void SetUpTimer(bool open) { //Set up wait timer for beginning of game
        CloseCanvasGame();
        setUpTimer.gameObject.SetActive(open);
    }

    private void CloseCanvasMenu() {
        credits.gameObject.SetActive(false);
        howToPlay.gameObject.SetActive(false);
        //options.gameObject.SetActive(false);
        menuInter.GetComponent<MenuInteract>().buttonSelected = false;
    }

    private void CloseCanvasGame() {
        options.gameObject.SetActive(false);
        pause.gameObject.SetActive(false);
        gameEnd.gameObject.SetActive(false);
        setUpTimer.gameObject.SetActive(false);
    }

    public void Quit() {
        Application.Quit();
    }

}
