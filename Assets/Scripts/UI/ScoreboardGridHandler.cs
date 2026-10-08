using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreboardGridHandler : MonoBehaviour
{
    [SerializeField] InputAction testIncreaseScore;
    public List<Score> listOfScores;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ArrangeScores();
    }

    // Update is called once per frame
    void Update()
    {
        if (listOfScores.Count > 5)
        {
            listOfScores.RemoveAt(5);
        }
    }
    void ArrangeScores()
    {
        GetComponentsInChildren(listOfScores);
        if (listOfScores.Count > 5)
        {
            for (int i = 0; i < listOfScores.Count; i++)
            {
                Destroy(listOfScores[5 + i].gameObject);
            }
        }
        else
            return;

    }
}
