using UnityEngine;
using Utility;

public class SaveManager : SingletonBehaviour<SaveManager>
{
    private int HighscoresSaved;
    private string highscoresAmountName = "AmountOfHighScores";
    [SerializeField] private GameObject highscorePrefab;
    [SerializeField] private Transform highscoreGrid;

    public override void Instantiate()
    {
        LoadHighscore();
    }
    public void SaveHighscore(int killScore, string playerName, string timePlayed)
    {
        HighscoresSaved++;
        PlayerPrefs.SetInt(highscoresAmountName, HighscoresSaved);
        PlayerPrefs.SetInt($"Score{HighscoresSaved}", killScore);
        PlayerPrefs.SetString($"Name{HighscoresSaved}", playerName);
        PlayerPrefs.SetString($"Time{HighscoresSaved}", timePlayed);
    }
    private void LoadHighscore()
    {
        HighscoresSaved = PlayerPrefs.GetInt(highscoresAmountName);
        if (HighscoresSaved > 0)
        {
            for (int i = 1; i < HighscoresSaved + 1; i++)
            {
                int playerScore = PlayerPrefs.GetInt($"Score{i}");
                string playerName = PlayerPrefs.GetString($"Name{i}", "NoName");
                string playerTime = PlayerPrefs.GetString($"Time{i}", "00:00");

                GameObject highscore = Instantiate(highscorePrefab, highscoreGrid);
                Score score = highscore.GetComponent<Score>();

                score.SetInformation(playerScore, playerName, playerTime);
            }
        }
    }
}
