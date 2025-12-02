using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Alternativa al TouchDetector que funciona mejor con UI
/// Usar este script si los botones están en un Canvas
/// </summary>
public class UITouchDetector : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool isTouched = false;
    bool isPointerDown = false;
    bool isPointerInside = false;

    [Header("Debug")]
    [SerializeField] bool showDebug = false;

    void Update()
    {
        // Actualizar el estado de isTouched basado en la interacción del puntero
        isTouched = isPointerDown && isPointerInside;
        
        if (showDebug && isTouched)
            Debug.Log($"{gameObject.name} is being touched/clicked!");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        if (showDebug)
            Debug.Log($"{gameObject.name} - Pointer Down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (showDebug)
            Debug.Log($"{gameObject.name} - Pointer Up");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerInside = true;
        if (showDebug)
            Debug.Log($"{gameObject.name} - Pointer Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerInside = false;
        if (showDebug)
            Debug.Log($"{gameObject.name} - Pointer Exit");
    }

    public bool IsTouched()
    {
        return isTouched;
    }
}
