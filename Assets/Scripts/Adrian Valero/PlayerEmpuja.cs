//using UnityEngine;

//public class PlayerEmpuja : MonoBehaviour
//{
//    public Vector2 direccionEmpuje = Vector2.zero;
//    public float distancia = 1f;

//    private Transform cajaDetector;
//    private MonsterMove monsterMove;

//    private void Start()
//    {
//        monsterMove = GetComponentInParent<MonsterMove>();
//        cajaDetector = transform;
//    }

//    void Update()
//    {
//        Cambiar dirección y mover el detector según WASD
//        if (Input.GetKey(KeyCode.W))
//            direccionEmpuje = Vector2.up;
//        else if (Input.GetKey(KeyCode.S))
//            direccionEmpuje = Vector2.down;
//        else if (Input.GetKey(KeyCode.A))
//            direccionEmpuje = Vector2.left;
//        else if (Input.GetKey(KeyCode.D))
//            direccionEmpuje = Vector2.right;
//        else
//            direccionEmpuje = Vector2.zero;

//        direccionEmpuje = monsterMove.GetMoveDir();

//        if (direccionEmpuje.x < 0.0f)
//            cajaDetector.localPosition = direccionEmpuje * Vector2.left * distancia;
//        else if (direccionEmpuje.x > 0.0f)
//            cajaDetector.localPosition = direccionEmpuje * Vector2.right * distancia;
//        else if (direccionEmpuje.y < 0.0f)
//            cajaDetector.localPosition = direccionEmpuje * Vector2.up * distancia;
//        else if (direccionEmpuje.y > 0.0f)
//            cajaDetector.localPosition = direccionEmpuje * Vector2.up * distancia;
//        else
//            cajaDetector.localPosition = transform.position;

//        if (Input.GetKeyDown(KeyCode.T))
//        {
//            Collider2D[] colisiones = Physics2D.OverlapBoxAll(
//                cajaDetector.position, new Vector2(0.9f, 0.9f), 0f);

//            foreach (var col in colisiones)
//            {
//                CajaMovible caja = col.GetComponent<CajaMovible>();
//                if (caja != null)
//                {
//                    caja.MoverEnDireccion(direccionEmpuje);
//                    break; //Only one box
//                }
//            }
//        }
//    }
//}