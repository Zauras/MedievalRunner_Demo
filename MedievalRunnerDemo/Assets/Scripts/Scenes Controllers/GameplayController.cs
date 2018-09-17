using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{ 
    public static GameplayController instance;
    private Player player;

    [SerializeField]
    private Text  pausedText, gameOverText, gameSucceedText;

    [SerializeField]
    private Button restartGameButton, resumeButton;

    [SerializeField]
    private GameObject pausePanel, instructionsPanel;

    private bool paused;

    //[SerializeField]
    //private GameObject[] playerModel;

//  <<<<<<<<<<<<<<<<< UNITY >>>>>>>>>>>>>>>>>

    void Awake()
    {
        if (instance == null) { instance = this; }
        paused = false;
        Time.timeScale = 0f;
    }
    void Start()
    {
        player = Player.instance;
    }

//  <<<<<<<<<<<<<<<<< STATES >>>>>>>>>>>>>>>>>

    public void Ready()
    {
        //playerModel[GameController.instance.GetSelectedModel()].SetActive(true);
        instructionsPanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape) & paused)
        {
            if (player != null & player.isAlive)
            {
                paused = false;
                ResumeGame();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = true;
            Time.timeScale = 0f;
            pausePanel.gameObject.SetActive(true);
            pausedText.gameObject.SetActive(true);
            resumeButton.gameObject.SetActive(true);

            restartGameButton.onClick.RemoveAllListeners();
            restartGameButton.onClick.AddListener(() => ResumeGame()); //add listener to button
        }
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        pausedText.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PlayerDied()
    {
        pausePanel.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        restartGameButton.onClick.RemoveAllListeners();
        restartGameButton.onClick.AddListener(() => RestartGame());
    }

    public void LevelSucceed()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        gameSucceedText.gameObject.SetActive(true);
        restartGameButton.onClick.RemoveAllListeners();
        restartGameButton.onClick.AddListener(() => RestartGame());
    }

    //  <<<<<<<<<<<<<<<<< BUTTONS >>>>>>>>>>>>>>>>>

    public void RestartGame()
    {
        SceneFader.instance.LoadScene(SceneManager.GetActiveScene().name); // perkrauna esama scena
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneFader.instance.LoadScene("MainMenu"); 
    }



}
