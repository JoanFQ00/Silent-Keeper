using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] float interactRadius, detectionRadius;
    public LayerMask detectionLayer;

    private float scrollCooldown = 0.3f;
    private float lastScrollTime = 0f;

    public float GetRadius(bool getInteractRadius)
    {
        if (getInteractRadius)
            return interactRadius;
        else
            return detectionRadius;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && gameObject.tag == "Player")
        {
            if (!GameManager.Instance.isPaused)
                Interacting(gameObject);
        }

        if (Input.mouseScrollDelta.y < 0 && gameObject.tag == "Kid")
        {
            if (!GameManager.Instance.isPaused && Time.time - lastScrollTime > scrollCooldown)
            {
                lastScrollTime = Time.time;
                Interacting(gameObject);
            }
        }
    }

    public void Interacting(GameObject thisObject)
    {
        if (this.gameObject == thisObject)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius, detectionLayer);

            if (hit != null)
            {
                if (hit.GetComponent<Lever>() != null)
                {
                    var element = hit.GetComponent<Lever>();
                    element.Interacted();
                }
                else if (hit.GetComponent<DoorBase>() != null)
                {
                    var element = hit.GetComponent<DoorBase>();
                    element.Interacted();
                }
                else if (hit.GetComponent<Locker>() != null)
                {
                    if (this.gameObject == hit.GetComponent<Locker>().GetObjectContained() || hit.GetComponent<Locker>().GetObjectContained() == null)
                    {
                        var element = hit.GetComponent<Locker>();
                        element.Interacted(this.gameObject, hit.gameObject);
                    }
                }
            }
        }
    }

    //INPUT SYSTEM
    //private void OnInteractionExecutedMonster()
    //{
    //    //CUANDO TENGAMOS LAS 2 TECLAS DIFERENTES
    //    if (!GameManager.Instance.isPaused)
    //    {
    //        if (gameObject.tag == "Player")
    //            Interacting(this.gameObject);

    //        Interacting(gameObject);
    //    }
    //}

    //private void OnInteractionExecutedKid()
    //{
    //    if (!GameManager.Instance.isPaused)
    //    {
    //        if (gameObject.tag == "Kid")
    //            Interacting(this.gameObject);
    //    }
    //}

    //public float GetInteractRadius() { return interacRadius; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}