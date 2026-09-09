using UnityEngine;

public class DestroyMyself : MonoBehaviour
{
    public float LifeTime = 1.5f;
    GameObject[] bulletsInScene;

    [SerializeField] private float damage;

    // Update is called once per frame
    void Update()
    {
        LifeTime -= Time.deltaTime;

        if (LifeTime < 0)
            Destroy(gameObject);

        bulletsInScene = GameObject.FindGameObjectsWithTag("Bullet");
        if (bulletsInScene.Length > 50)
        {
            foreach (GameObject bullet in bulletsInScene)
            {
                if (bullet != gameObject)
                    DestroyWhenTooMany(bullet);
            }
        }
    }
    void DestroyWhenTooMany(GameObject bullet)
    {
        Destroy(bullet);
    }
    private void OnTriggerEnter(Collider other)
    {
        EnemyHealth enemy = other.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            Destroy(gameObject);
            enemy.TakeDamage(damage);
        }
    }
}