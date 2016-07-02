using UnityEngine;
using System.Collections;

public class GizmoLogic : MonoBehaviour
{
    [SerializeField]
    private Color _gizmoColor = Color.white;

    void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
