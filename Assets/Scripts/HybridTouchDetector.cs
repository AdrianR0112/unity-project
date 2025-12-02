using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Detector de entrada híbrido que funciona tanto para UI como para GameObjects del mundo
/// Combina TouchDetector y UITouchDetector en uno solo
/// </summary>
public class HybridTouchDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool isTouched = false;
    bool isUITouched = false;
    bool isWorldTouched = false;
    
    [Header("Detection Settings")]
    [SerializeField] bool detectUI = true;
    [SerializeField] bool detectWorld = true;
    
    [Header("World Detection")]
    [SerializeField] BoxCollider2D worldCollider;
    
    [Header("Debug")]
    [SerializeField] bool showDebug = false;
    
    Camera targetCamera;
    
    void Start()
    {
        targetCamera = Camera.main ?? FindObjectOfType<Camera>();
        
        if (worldCollider == null)
            worldCollider = GetComponent<BoxCollider2D>();
            
        if (showDebug)
        {
            Debug.Log($"{gameObject.name} - Hybrid Touch Detector initialized");
            Debug.Log($"Detect UI: {detectUI}, Detect World: {detectWorld}");
            Debug.Log($"World Collider: {worldCollider != null}");
        }
    }
    
    void Update()
    {
        if (detectWorld)
            DetectWorldInput();
            
        // Combinar ambos tipos de entrada
        isTouched = isUITouched || isWorldTouched;
        
        if (showDebug && isTouched)
            Debug.Log($"{gameObject.name} - Touched! UI: {isUITouched}, World: {isWorldTouched}");
    }
    
    void DetectWorldInput()
    {
        isWorldTouched = false;
        
        if (worldCollider == null || targetCamera == null)
            return;
        
        // Detectar toques táctiles
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector2 touchPos = Input.touches[i].position;
            if (CheckWorldPosition(touchPos))
            {
                isWorldTouched = true;
                if (showDebug)
                    Debug.Log($"{gameObject.name} - World Touch detected");
                break;
            }
        }
        
        // Detectar mouse
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            if (CheckWorldPosition(mousePos))
            {
                isWorldTouched = true;
                if (showDebug)
                    Debug.Log($"{gameObject.name} - World Mouse detected");
            }
        }
    }
    
    bool CheckWorldPosition(Vector2 screenPosition)
    {
        // Método 1: ScreenToWorldPoint
        Vector3 worldPos = targetCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, -targetCamera.transform.position.z));
        if (worldCollider.OverlapPoint(worldPos))
            return true;
        
        // Método 2: Raycast 2D
        Vector3 worldPosRay = targetCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, targetCamera.nearClipPlane));
        Vector2 rayDirection = (worldPosRay - targetCamera.transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(targetCamera.transform.position, rayDirection);
        
        if (hit.collider == worldCollider)
            return true;
            
        return false;
    }
    
    // UI Event Handlers
    public void OnPointerDown(PointerEventData eventData)
    {
        if (detectUI)
        {
            isUITouched = true;
            if (showDebug)
                Debug.Log($"{gameObject.name} - UI Pointer Down");
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (detectUI)
        {
            isUITouched = false;
            if (showDebug)
                Debug.Log($"{gameObject.name} - UI Pointer Up");
        }
    }
    
    public bool IsTouched()
    {
        return isTouched;
    }
    
    // Método para forzar estado (útil para debugging)
    public void SetTouched(bool touched)
    {
        isTouched = touched;
        if (showDebug)
            Debug.Log($"{gameObject.name} - Forced touched state: {touched}");
    }
}
