using TMPro;
using UnityEngine;

public class UploadScore : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private Transform scoreBoard;
    [SerializeField] private TextMeshProUGUI timePlayed;
    bool HasSavedScore;
    public void Upload()
    {
        if (inputField.text == string.Empty)
        {
            errorMessageText.gameObject.SetActive(true);
            return;
        }
        else if (!HasSavedScore)
        {
            GameObject newScore = Instantiate(scorePrefab, scoreBoard);
            Score score = newScore.GetComponent<Score>();

            SaveScore(score, EnemyController.Instance.EnemiesKilled, inputField.text, timePlayed.text);

            errorMessageText.gameObject.SetActive(false);
            HasSavedScore = true;
        }
    }
    public void SaveScore(Score newScore, int score, string playerName, string timePlayed)
    {
        newScore.SetInformation(score, playerName, timePlayed);
        SaveManager.Instance.SaveHighscore(score, playerName, timePlayed);
    }
    private void OnEnable()
    {
        HasSavedScore = false;
    }
}
