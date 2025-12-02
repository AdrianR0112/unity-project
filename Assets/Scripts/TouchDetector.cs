using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TouchDetector : MonoBehaviour
{
    bool isTouched;
    BoxCollider2D boxCollider2D;
    
    [Header("Detection Method")]
    [SerializeField] bool useScreenSpaceDetection = false;
    [SerializeField] bool useWorldSpaceDetection = true;
    
    [Header("Debug")]
    [SerializeField] bool showDebug = false;
    
    Camera targetCamera;

	void Start()
	{
        boxCollider2D = GetComponent<BoxCollider2D>();
        targetCamera = Camera.main;
        
        if (targetCamera == null)
        {
            targetCamera = FindObjectOfType<Camera>();
            if (showDebug)
                Debug.LogWarning($"{gameObject.name}: No MainCamera found, using first available camera");
        }
    }

	void Update()
    {
        TouchDetection();
    }

    void TouchDetection()
	{
        isTouched = false;
        
        if (targetCamera == null)
        {
            if (showDebug)
                Debug.LogError($"{gameObject.name}: No camera available for touch detection");
            return;
        }
        
        // Detectar toques táctiles (móvil)
        for (int i = 0; i < Input.touchCount; i++)
        {
            Vector2 inputPos = Input.touches[i].position;
            if (CheckInputPosition(inputPos, "Touch"))
            {
                isTouched = true;
                break;
            }
        }
        
        // Detectar clics del mouse (para testing en editor)
        if (Input.GetMouseButton(0))
        {
            Vector2 inputPos = Input.mousePosition;
            if (CheckInputPosition(inputPos, "Mouse"))
            {
                isTouched = true;
            }
        }
    }
    
    bool CheckInputPosition(Vector2 screenPosition, string inputType)
    {
        if (useScreenSpaceDetection)
        {
            // Método 1: Detección en espacio de pantalla (para UI)
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPosition, targetCamera);
            }
        }
        
        if (useWorldSpaceDetection)
        {
            // Método 2: Detección en espacio del mundo (para GameObjects 2D)
            Vector3 worldPosition = targetCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, targetCamera.nearClipPlane + 1f));
            
            if (showDebug)
                Debug.Log($"{gameObject.name} - {inputType}: Screen {screenPosition} -> World {worldPosition}");
            
            if (boxCollider2D.OverlapPoint(worldPosition))
            {
                if (showDebug)
                    Debug.Log($"{gameObject.name} - {inputType} HIT!");
                return true;
            }
        }
        
        // Método 3: Raycast desde la cámara (más robusto)
        Ray ray = targetCamera.ScreenPointToRay(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        
        if (hit.collider == boxCollider2D)
        {
            if (showDebug)
                Debug.Log($"{gameObject.name} - {inputType} HIT via Raycast!");
            return true;
        }
        
        return false;
    }

    public bool IsTouched()
	{
        return isTouched;
	}
}
