using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Interacts : MonoBehaviour
{
    [SerializeField] LayerMask interactablesLayer;
    [SerializeField] float interactsRadius;
    [SerializeField] bool drawGizmoz;

    // Helper function for checking interactables nearby
    public Interactable CheckInteractables(out Interactable interactable)
    {
        var collider = Physics.OverlapSphere(transform.position, interactsRadius, interactablesLayer);

        interactable = null;
        if (collider.Length == 0) return null;
        if (!collider[0].TryGetComponent<Interactable>(out interactable)) return null;

        return interactable;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (drawGizmoz) Gizmos.DrawWireSphere(transform.position, interactsRadius);
    }
#endif
}
