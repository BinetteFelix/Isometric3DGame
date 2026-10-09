using TMPro;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    private float timePassed;
    private TextMeshProUGUI timerText;
    private bool isTimerRunning;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            timePassed += Time.deltaTime;
            UpdateTimerText();
        }
    }
    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(timePassed / 60f);
        int seconds = Mathf.FloorToInt(timePassed % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    private void OnEnable()
    {
        isTimerRunning = true;
    }
}
