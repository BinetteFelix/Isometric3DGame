using UnityEngine;

public class DrawSpawnAreas : MonoBehaviour
{
    [SerializeField] private Transform[] SpawnAreas;
    [SerializeField] private Vector3[] AreaSizes;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.orangeRed;
        for (int i = 0; i < 4; i++)
        {
            Gizmos.DrawCube(SpawnAreas[i].position, AreaSizes[i]);
        }
       
    }
}
