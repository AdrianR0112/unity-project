using UnityEngine;

/// <summary>
/// Script de prueba simple para verificar entrada de mouse
/// Agregar a cualquier GameObject para probar
/// </summary>
public class SimpleMouseTest : MonoBehaviour
{
    [Header("Test Settings")]
    [SerializeField] bool testMouseClicks = true;
    [SerializeField] bool testKeyboard = true;
    [SerializeField] bool showOnScreen = true;
    
    bool mousePressed = false;
    bool keyPressed = false;
    
    void Update()
    {
        if (testMouseClicks)
        {
            mousePressed = Input.GetMouseButton(0);
            if (Input.GetMouseButtonDown(0))
                Debug.Log($"Mouse clicked at: {Input.mousePosition}");
        }
          if (testKeyboard)
        {
            keyPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || 
                        Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
                        
            if (Input.GetKeyDown(KeyCode.W))
                Debug.Log("W key pressed - GAS");
            if (Input.GetKeyDown(KeyCode.S))
                Debug.Log("S key pressed - BRAKE");
        }
        
        // Test de conversión de coordenadas
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestCoordinateConversion();
        }
    }
    
    void TestCoordinateConversion()
    {
        Vector2 mouseScreen = Input.mousePosition;
        Camera cam = Camera.main ?? FindObjectOfType<Camera>();
        
        if (cam != null)
        {
            Vector3 mouseWorld1 = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, -cam.transform.position.z));
            Vector3 mouseWorld2 = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, cam.nearClipPlane + 1f));
            Vector3 mouseWorld3 = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 10f));
            
            Debug.Log("=== COORDINATE TEST ===");
            Debug.Log($"Mouse Screen: {mouseScreen}");
            Debug.Log($"Camera Position: {cam.transform.position}");
            Debug.Log($"Method 1 (camera Z): {mouseWorld1}");
            Debug.Log($"Method 2 (near clip + 1): {mouseWorld2}");
            Debug.Log($"Method 3 (fixed 10): {mouseWorld3}");
            Debug.Log("=======================");
        }
    }
    
    void OnGUI()
    {
        if (!showOnScreen) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.Label("=== MOUSE TEST ===");
        GUILayout.Label($"Mouse Position: {Input.mousePosition}");
        GUILayout.Label($"Mouse Pressed: {mousePressed}");
        GUILayout.Label($"Keys Pressed: {keyPressed}");
        GUILayout.Label("Press T to test coordinates");
        
        if (Camera.main != null)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GUILayout.Label($"World Position: {worldPos}");
        }
        
        GUILayout.EndArea();
    }
}
