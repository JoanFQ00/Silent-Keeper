//using UnityEngine;

//public class CajaController : MonoBehaviour
//{
//    private Vector2 direccionMirada = Vector2.zero;
//    private CajaMovible cajaActual;

//    private void Update()
//    {
//        if (Input.GetKey(KeyCode.W)) direccionMirada = Vector2.up;
//        else if (Input.GetKey(KeyCode.S)) direccionMirada = Vector2.down;
//        else if (Input.GetKey(KeyCode.A)) direccionMirada = Vector2.left;
//        else if (Input.GetKey(KeyCode.D)) direccionMirada = Vector2.right;
//        else direccionMirada = Vector2.zero;

//        if (Input.GetKeyDown(KeyCode.T) && cajaActual != null && direccionMirada != Vector2.zero)
//        {
//            cajaActual.MoverEnDireccion(direccionMirada);
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Caja"))
//        {
//            cajaActual = collision.GetComponent<CajaMovible>();
//        }
//    }

//    private void OnTriggerExit2D(Collider2D collision)
//    {
//        if (collision.CompareTag("Caja"))
//        {
//            if (collision.GetComponent<CajaMovible>() == cajaActual)
//            {
//                cajaActual = null;
//            }
//        }
//    }
//}