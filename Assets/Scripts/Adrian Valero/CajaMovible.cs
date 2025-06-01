using UnityEngine;

public class CajaMovible : MonoBehaviour
{
    public float velocidad = 3f;
    private Vector2 direccionMovimiento;
    private bool moviendo = false;
    private Vector2 ultimaDireccion;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (moviendo)
        {
            Vector2 movimiento = direccionMovimiento * velocidad * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movimiento);
        }
    }

   public void MoverEnDireccion(Vector2 direccion)
{
    if (!moviendo)
    {
        direccionMovimiento = direccion.normalized;
        ultimaDireccion = direccion.normalized; //  Importante para el posicionamiento
        moviendo = true;
    }
}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerCaja = LayerMask.NameToLayer("Caja");
        int layerPared = LayerMask.NameToLayer("Way");

        if (moviendo && 
           (collision.gameObject.layer == layerCaja || collision.gameObject.layer == layerPared))
        {
            DetenerMovimiento();
        }
    }

 private void DetenerMovimiento()
{
   
    moviendo = false;

    // Obtenemos la posición actual
    Vector2 posicionActual = transform.position;

    // Ajustamos según la última dirección (como ya hacías)
    if (ultimaDireccion == Vector2.right)
        posicionActual.x -= 0.5f;
    else if (ultimaDireccion == Vector2.left)
        posicionActual.x += 0.5f;
    else if (ultimaDireccion == Vector2.up)
        posicionActual.y -= 0.5f;
    else if (ultimaDireccion == Vector2.down)
        posicionActual.y += 0.5f;

    // 🔧 REDONDEAMOS el otro eje para que termine en .5
    posicionActual.x = Mathf.Floor(posicionActual.x) + 0.5f;
    posicionActual.y = Mathf.Floor(posicionActual.y) + 0.5f;

    transform.position = posicionActual;

    rb.linearVelocity = Vector2.zero;

}
}
