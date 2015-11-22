using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(ScoreSystem), typeof(GameController))]
public class MenuManager : MonoBehaviour {

    GameController gameController;
    ScoreSystem score;
    
    [SerializeField]
    Transform[] InGameGUI;
    [SerializeField]
    Transform[] MenuGUI;
    [SerializeField]
    Transform[] EndGameMenuGUI;
    [SerializeField]
    Transform[] PauseGUI;

    [SerializeField]
    Text _scoreText;
    [SerializeField]
    Text _highScoreText;

    void Start()
    {
        gameController = this.GetComponent<GameController>();
        gameController.EndGame += EndGame;

        score = GetComponent<ScoreSystem>();
    }

    //TODO Reset button for tablet
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void LateUpdate()
    {
        _scoreText.text = score.CurrentScore.ToString();
    }

    public void StartGame()
    {
        if (InGameGUI.Length > 0)
        {
            foreach (Transform group in InGameGUI)
            {
                group.gameObject.SetActive(true);
            }
        }
        if (MenuGUI.Length > 0)
        {
            foreach (Transform group in MenuGUI)
            {
                group.gameObject.SetActive(false);
            }
        }
        gameController.StartGame();
    }
    public void PauseGame(bool paused)
    {
        if (InGameGUI.Length > 0)
        {
            foreach (Transform group in InGameGUI)
            {
                foreach (Button button in group.GetComponentsInChildren<Button>())
                {
                    button.interactable = !paused;
                }
            }
        }

        PauseGUI[0].gameObject.SetActive(!paused);
        PauseGUI[0].GetComponent<Button>().interactable = !paused;
        PauseGUI[1].gameObject.SetActive(paused);
        PauseGUI[1].GetComponent<Button>().interactable = paused;

        gameController.Paused = paused;
    }
    //TODO Shows highscore, but is hard to read on account of the grid in the background
    public void EndGame()
    {
        _highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        if (InGameGUI.Length > 0)
        {
            foreach (Transform group in InGameGUI)
            {
                group.gameObject.SetActive(false);
            }
        }
        if (PauseGUI.Length > 0)
        {
            foreach (Transform group in PauseGUI)
            {
                group.gameObject.SetActive(false);
            }
        }
        if (EndGameMenuGUI.Length > 0)
        {
            foreach (Transform group in EndGameMenuGUI)
            {
                group.gameObject.SetActive(true);
            }
        }
    }
}
