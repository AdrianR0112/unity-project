using UnityEngine;

/// <summary>
/// Script temporal de debug para diagnosticar problemas de entrada del mouse
/// Agregar este script a un GameObject en la escena para ver información de debug
/// </summary>
public class MouseDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] bool showDebugInfo = true;
    [SerializeField] bool drawMousePosition = true;
    
    void Update()
    {
        if (!showDebugInfo) return;

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = -Camera.main.transform.position.z;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            
            Debug.Log($"Mouse Click - Screen: {Input.mousePosition} | World: {mouseWorldPos}");
            
            if (drawMousePosition)
            {
                // Dibujar una línea desde la cámara hacia la posición del mouse
                Debug.DrawLine(Camera.main.transform.position, mouseWorldPos, Color.red, 0.1f);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("=== MOUSE DEBUG INFO ===");
            Debug.Log($"Camera Position: {Camera.main.transform.position}");
            Debug.Log($"Camera Z: {Camera.main.transform.position.z}");
            Debug.Log($"Mouse Position: {Input.mousePosition}");
            
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = -Camera.main.transform.position.z;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            Debug.Log($"Mouse World Position: {mouseWorldPos}");
            Debug.Log("========================");
        }
    }

    void OnGUI()
    {
        if (!showDebugInfo) return;

        GUILayout.Label("=== MOUSE DEBUG ===");
        GUILayout.Label($"Mouse Screen: {Input.mousePosition}");
        
        if (Camera.main != null)
        {
            Vector3 mouseScreenPos = Input.mousePosition;
            mouseScreenPos.z = -Camera.main.transform.position.z;
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            GUILayout.Label($"Mouse World: {mouseWorldPos}");
            GUILayout.Label($"Camera Z: {Camera.main.transform.position.z}");
        }
        
        GUILayout.Label("Press M for detailed debug info");
        GUILayout.Label("Left Click to see mouse position");
        
        if (Input.GetMouseButton(0))
        {
            GUILayout.Label("MOUSE CLICKING!");
        }
    }
}
