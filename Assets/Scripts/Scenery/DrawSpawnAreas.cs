using UnityEngine;

public class DrawSpawnAreas : MonoBehaviour
{
    [SerializeField] private Vector3 AreaSize;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.orangeRed;
        Gizmos.DrawCube(transform.position, AreaSize);
    }
}
