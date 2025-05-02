using UnityEngine;

public class Locker : MonoBehaviour, IInteractableWithButton
{
    private bool activated;
    private GameObject _other;
    private Color originalColor;


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
                _other.transform.position = this.transform.position;
                originalColor = _other.GetComponent<SpriteRenderer>().color;
                _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);

                if (_other.gameObject.tag == "Kid")
                    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.HiddenInLocker);
                else if (_other.gameObject.tag == "Player")
                    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.HiddenInLocker);

                _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            }
            else
            {

                Camera cam = GameObject.FindAnyObjectByType<Camera>();

                if (cam.GetComponent<CameraFollow>().GetCameraTarget().tag == "Kid" && _other.gameObject.tag == "Kid")
                {
                    _other.transform.position = this.transform.position + this.transform.right * 1f;
                    _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                    _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);

                    _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    //_other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                    _other.GetComponent<SpriteRenderer>().color = originalColor;
                    _other = null;
                }
                else if (cam.GetComponent<CameraFollow>().GetCameraTarget().tag == "Player" && _other.gameObject.tag == "Player")
                {
                    _other.transform.position = this.transform.position + this.transform.right * 1f;
                    _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                    _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Stoped);

                    _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    //_other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                    _other.GetComponent<SpriteRenderer>().color = originalColor;
                    _other = null;
                }


                // _other.transform.position = this.transform.position + this.transform.right * 1f;
                // _other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);

                // if (_other.gameObject.tag == "Kid")
                //     _other.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);
                // else if (_other.gameObject.tag == "Player")
                //     _other.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Stoped);

                // _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                // _other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                // //_other.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
                // _other.GetComponent<SpriteRenderer>().color = originalColor;
                // _other = null;
            }
        }
    }

    public GameObject GetObjectContained()
    {
        return _other;
    }

    public void Interacted() { }
}