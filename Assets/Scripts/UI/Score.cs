using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public string PlayerName;
    public int score;
    public string TimePlayed;

    public List<TextMeshProUGUI> scoreTexts;
    private void Start()
    {
        scoreTexts[0].text = PlayerName;
        scoreTexts[1].text = score.ToString();
        scoreTexts[2].text = TimePlayed;
    }
}
