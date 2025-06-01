using UnityEngine;

public class PlayerEmpuja : MonoBehaviour
{
    public Transform cajaDetector; // Objeto hijo que moveremos
    public Vector2 direccionEmpuje = Vector2.zero;
    public float distancia = 1f; // Ajustable por si quieres más espacio

    void Update()
    {
        // Cambiar dirección y mover el detector según WASD
        if (Input.GetKey(KeyCode.W))
            direccionEmpuje = Vector2.up;
        else if (Input.GetKey(KeyCode.S))
            direccionEmpuje = Vector2.down;
        else if (Input.GetKey(KeyCode.A))
            direccionEmpuje = Vector2.left;
        else if (Input.GetKey(KeyCode.D))
            direccionEmpuje = Vector2.right;
        else
            direccionEmpuje = Vector2.zero;

        // Posicionar el detector según la dirección
        cajaDetector.localPosition = direccionEmpuje * distancia;

        // Si se pulsa T, activar la caja
        if (Input.GetKeyDown(KeyCode.T))
        {
            Collider2D[] colisiones = Physics2D.OverlapBoxAll(
                cajaDetector.position, new Vector2(0.9f, 0.9f), 0f);

            foreach (var col in colisiones)
            {
                CajaMovible caja = col.GetComponent<CajaMovible>();
                if (caja != null)
                {
                    caja.MoverEnDireccion(direccionEmpuje);

                    break; // Solo una caja
                }
            }
        }
    }
}
