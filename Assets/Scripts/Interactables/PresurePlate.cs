using System;
using System.Threading;
using UnityEngine;

public class PresurePlate : MonoBehaviour
{
    [SerializeField] int key;
    [SerializeField] bool activated;
    SpriteRenderer spriteRenderer;

    public static Action<bool, int> PresureActivated;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (activated)
        {
            spriteRenderer.color = Color.green;
        }
        else
        {
            spriteRenderer.color = spriteRenderer.color = new Color(0f, 166f, 255f, 1f);
        }
    }

    private int _activeCollisions = 0; // Contador de colisiones activas

    void OnTriggerEnter2D(Collider2D collision)
    {
        activated = true;
        PresureActivated?.Invoke(activated, key);
        spriteRenderer.color = Color.green;

        // Aumentamos el contador cuando entra un collider
        _activeCollisions++;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Reducimos el contador cuando sale un collider
        _activeCollisions--;

        // Solo ejecutamos el código si no quedan colisiones activas
        if (_activeCollisions == 0)
        {
            activated = false;
            PresureActivated?.Invoke(activated, key);
            spriteRenderer.color = new Color(0f, 166f, 255f, 1f);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        // Solo cambiamos el color a verde si hay al menos una colisión
        if (_activeCollisions > 0)
        {
            spriteRenderer.color = Color.green;
        }
    }
}
