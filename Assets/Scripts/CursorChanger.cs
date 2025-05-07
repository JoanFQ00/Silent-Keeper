using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorTexture1; // Imagen por defecto
    public Texture2D cursorTexture2; // Imagen al presionar
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    private int buttonsPressed = 0;

    void Start()
    {
        Cursor.SetCursor(cursorTexture1, hotspot, cursorMode);
    }

    void Update()
    {
        // Botón izquierdo o derecho presionado
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            buttonsPressed++;
            Cursor.SetCursor(cursorTexture2, hotspot, cursorMode);
        }

        // Botón izquierdo o derecho soltado
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            buttonsPressed = Mathf.Max(0, buttonsPressed - 1);

            if (buttonsPressed == 0)
            {
                Cursor.SetCursor(cursorTexture1, hotspot, cursorMode);
            }
        }
    }
}