using System.Linq;
using UnityEngine;

public class DestroyMyself : MonoBehaviour
{
    float LifeTime = 1.5f;

    GameObject[] bulletsInScene;

    // Update is called once per frame
    void Update()
    {
        LifeTime -= Time.deltaTime;

        if (LifeTime < 0)
            Destroy(gameObject);

        bulletsInScene = GameObject.FindGameObjectsWithTag("Bullet");
        if (bulletsInScene.Length > 5)
        {
            foreach (GameObject bullet in bulletsInScene)
            {
                DestroyWhenTooMany(bullet);
            }
        }
    }
    void DestroyWhenTooMany(GameObject bullet)
    {
        Destroy(bullet);
    }
}