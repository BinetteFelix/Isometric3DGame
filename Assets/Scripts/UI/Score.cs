using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts = new TextMeshProUGUI[3];
    public void SetInformation(int playerScore, string playerName, string timePlayed)
    {
        scoreTexts[0].text = playerName;
        scoreTexts[1].text = playerScore.ToString();
        scoreTexts[2].text = timePlayed;
    }
}
