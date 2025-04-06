using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locker : MonoBehaviour
{
    private bool activated;
    private Color otherColor;

    private void Start()
    {
        activated = false;
    }

    public void Interacted(GameObject other, GameObject thisObject)
    {
        if (this.gameObject == thisObject)
        {
            activated = !activated;
            other.GetComponent<Collider2D>().isTrigger = activated;

            if (activated)
            {
                // Mover al jugador dentro de la taquilla
                other.transform.position = this.transform.position;
                otherColor = other.GetComponent<SpriteRenderer>().color;
                other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

                // Congelar la posición
                other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            }
            else
            {
                // Poner al jugador fuera de la taquilla en la dirección correcta
                other.transform.position = this.transform.position + this.transform.right * 2f; // Usa 'up' para moverse en 2D
                other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                // Desbloquear las restricciones del Rigidbody2D
                other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                other.GetComponent<SpriteRenderer>().color = otherColor;
            }
        }
    }
}
