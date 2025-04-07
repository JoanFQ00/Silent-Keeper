using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locker : MonoBehaviour
{
    private bool activated;
    private GameObject _other;
    private Color otherColor;

    private void Start()
    {
        _other = null;
        activated = false;
    }

    public void Interacted(GameObject other, GameObject thisObject)
    {
        if (this.gameObject == thisObject)
        {
            activated = !activated;
            _other = other;
            _other.GetComponent<Collider2D>().isTrigger = activated;

            if (activated)
            {
                // Mover al jugador dentro de la taquilla
                _other.transform.position = this.transform.position;
                otherColor = _other.GetComponent<SpriteRenderer>().color;
                _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

                if (_other.gameObject.tag == "Kid")
                    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.Hidden);
                else if (_other.gameObject.tag == "Player")
                    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Hidden);

                // Congelar la posición
                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            }
            else
            {
                // Poner al jugador fuera de la taquilla en la dirección correcta
                _other.transform.position = this.transform.position + this.transform.right * 2f; // Usa 'up' para moverse en 2D
                _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                if (_other.gameObject.tag == "Kid")
                    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);
                else if (_other.gameObject.tag == "Player")
                    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Stoped);

                // Desbloquear las restricciones del Rigidbody2D
                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                _other.GetComponent<SpriteRenderer>().color = otherColor;
                _other = null;
            }
        }
    }

    public GameObject GetObjectContained()
    {
        return _other;
    }
}