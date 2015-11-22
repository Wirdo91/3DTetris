using UnityEngine;
using System.Collections;

public class ScoreSystem : MonoBehaviour {

    int _currentScore;
    public int CurrentScore
    {
        get { return _currentScore; }
    }
    int _highScore;
    public int HighScore
    {
        get { return _highScore; }
    }

	// Use this for initialization
	void Start ()
    {
        _highScore = PlayerPrefs.GetInt("HighScore");
        _currentScore = 0;
	}

    public void AddScore(int value)
    {
        _currentScore += value;
    }

    public void EndGame()
    {
        if (CurrentScore > HighScore)
        {
            _highScore = CurrentScore;
        }
        PlayerPrefs.SetInt("HighScore", _highScore);
        _currentScore = 0;
    }
}
