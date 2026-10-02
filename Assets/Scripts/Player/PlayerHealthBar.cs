using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image HealthBar;
    private Vector3 offset = new Vector3(0, 2.5f, 0);
    Player_Movement player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>();
    }
    private void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offset;
        }
    }
}
