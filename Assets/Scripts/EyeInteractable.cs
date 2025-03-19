using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public class EyeInteractable : MonoBehaviour
{
    public bool IsHovered { get; set; }

    [SerializeField] private UnityEvent<GameObject> OnObjectHover; 
    [SerializeField] private Material OnHoverActiveMaterial; 
    [SerializeField] private Material OnHoverInactiveMaterial;
    
    private MeshRenderer meshRenderer; 
    private float hoverTime = 0f; // Time the object has been hovered over
    private float hoverDuration = 0.5f; // Duration to look at the object before it is destroyed
    
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    
    void Update() 
    {
        if (IsHovered)
        {
            // Increment the hoverTime by the time elapsed since last frame
            hoverTime += Time.deltaTime;

            // Check if the hoverTime exceeds the threshold
            if (hoverTime >= hoverDuration)
            {
                PlayerHealthManager.Instance.AddScore(1);
                Destroy(gameObject);
            }

            meshRenderer.material = OnHoverActiveMaterial;
            OnObjectHover?.Invoke(gameObject);
        }
        else
        {
            // Reset hover time since the object is no longer being hovered over
            hoverTime = 0f;
            meshRenderer.material = OnHoverInactiveMaterial;
        }
    }
}
