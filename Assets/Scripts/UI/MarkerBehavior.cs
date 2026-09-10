using UnityEngine;

public class MarkerBehavior : MonoBehaviour
{
    private Transform Target;

    private void Update()
    {
        if (Target != null)
            Look();
        else
            Destroy(gameObject);
        
    }
    public void SetTarget(Transform position)
    {
        Target = position;
    }
    private float CalculateLookDirection()
    {
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(Target.position);
        Vector2 behaviorPos = Camera.main.ScreenToViewportPoint(transform.position);
        float angle = AngleBetweenTwoPoints(positionOnScreen, behaviorPos);

        return angle;
    }
    void Look()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, CalculateLookDirection()));
    }
    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}