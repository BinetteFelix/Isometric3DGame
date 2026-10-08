using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UploadScore : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private GameObject scorePrefab;
    [SerializeField] private Transform scoreBoard;
    [SerializeField] private TextMeshProUGUI timePlayed;
    bool HasSavedScore;

    private void Start()
    {
        HasSavedScore = false;
    }
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

            score.PlayerName = inputField.text;
            score.score = EnemyController.Instance.EnemiesKilled;
            score.TimePlayed = timePlayed.text;

            errorMessageText.gameObject.SetActive(false);
            HasSavedScore = true;
        }
    }
}
