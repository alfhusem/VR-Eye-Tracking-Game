using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EyeTrackingRay : MonoBehaviour
{
    [SerializeField]
    private float rayDistance = 1.0f;

    [SerializeField]
    private float rayWidth = 0.01f;

    [SerializeField]
    private LayerMask layersToInclude;

    [SerializeField]
    private Color rayColorDefaultState = Color.white;

    [SerializeField]
    private Color rayColorHoverState = Color.red;

    private LineRenderer lineRenderer;

    private List<EyeInteractable> eyeInteractables = new List<EyeInteractable>();

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetupRay();
    }

    void SetupRay() 
    {
        lineRenderer.enabled = false;
    // lineRenderer.useWorldSpace = false; 
    // lineRenderer.positionCount = 2; 
    // lineRenderer.startWidth = rayWidth; 
    // lineRenderer.endWidth = rayWidth; 
    // lineRenderer.startColor = rayColorDefaultState; 
    // lineRenderer.endColor = rayColorDefaultState; 
    // lineRenderer.SetPosition(0, transform. position); 
    // lineRenderer.SetPosition(1, new Vector3(transform.position.x, transform.position.y, 
    //     transform.position.z + rayDistance)); 
    } 
    void FixedUpdate() 
    { 
        RaycastHit hit; 
        Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance; 
        if(Physics.Raycast(transform.position, rayCastDirection, out hit, Mathf.Infinity, layersToInclude)) 
        {
            UnSelect(); 
            lineRenderer.startColor = rayColorHoverState; 
            lineRenderer.endColor = rayColorHoverState; 
            var eyeInteractable = hit.transform.GetComponent<EyeInteractable>(); 
            eyeInteractables.Add(eyeInteractable); 
            eyeInteractable.IsHovered = true; 
        } 
        else 
        {
            lineRenderer.startColor = rayColorDefaultState; 
            lineRenderer.endColor = rayColorDefaultState; 
            UnSelect(true);
        }
    }
    
    void UnSelect(bool clear = false)
    {
        foreach (var interactable in eyeInteractables)
        {
            interactable.IsHovered = false;
        }
        if(clear)
        {
            eyeInteractables.Clear();
        }
    }
}