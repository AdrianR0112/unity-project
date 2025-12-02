using UnityEngine;

/// <summary>
/// Gestor de entrada unificado que soporta tanto entrada táctil como mouse/teclado
/// Útil para hacer el juego compatible con PC y móvil
/// </summary>
public static class InputManager
{
    /// <summary>
    /// Detecta si hay un toque o clic en una posición específica del mundo
    /// </summary>
    public static bool IsPointTouched(Vector2 worldPosition, float radius = 0.5f)
    {
        // Detectar toques táctiles
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].phase == TouchPhase.Began || Input.touches[i].phase == TouchPhase.Stationary)
            {
                Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                if (Vector2.Distance(touchWorldPos, worldPosition) <= radius)
                {
                    return true;
                }
            }
        }

        // Detectar clics del mouse
        if (Input.GetMouseButton(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mouseWorldPos, worldPosition) <= radius)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Detecta si hay un tap/clic que acaba de comenzar en una posición específica
    /// </summary>
    public static bool IsPointTapped(Vector2 worldPosition, float radius = 0.5f)
    {
        // Detectar toques táctiles
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].phase == TouchPhase.Began)
            {
                Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(Input.touches[i].position);
                if (Vector2.Distance(touchWorldPos, worldPosition) <= radius)
                {
                    return true;
                }
            }
        }

        // Detectar clics del mouse
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mouseWorldPos, worldPosition) <= radius)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Obtiene la posición actual del input (toque o mouse) en coordenadas del mundo
    /// </summary>
    public static Vector2? GetCurrentInputWorldPosition()
    {
        if (Input.touchCount > 0)
        {
            return Camera.main.ScreenToWorldPoint(Input.touches[0].position);
        }
        
        if (Input.GetMouseButton(0))
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        return null;
    }    /// <summary>
    /// Detecta entrada de navegación horizontal (flechas, A/D, o swipe)
    /// </summary>
    public static int GetHorizontalNavigation()
    {
        // Entrada de teclado
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            return -1;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            return 1;

        // Aquí se podría agregar detección de swipe para móvil si es necesario
        
        return 0;
    }

    /// <summary>
    /// Detecta entrada de navegación vertical (flechas, W/S)
    /// </summary>
    public static int GetVerticalNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            return 1;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            return -1;

        return 0;
    }
}
