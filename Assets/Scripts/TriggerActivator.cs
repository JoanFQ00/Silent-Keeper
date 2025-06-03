using UnityEngine;
using System.Collections.Generic;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField] private bool hasToAvivateObjects, onlyOneTime;
    [SerializeField] private List<GameObject> elements = new List<GameObject>();
    [SerializeField] private bool lastAnim = false;
    [SerializeField] private AnimationClip voidAnim;

    private void Start()
    {
        if (hasToAvivateObjects)
            foreach (GameObject element in elements)
                element.SetActive(false);
        else
            foreach (GameObject element in elements)
                element.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (lastAnim)
        {
            IntroRoomsManager.Instance.SetAnimToPlay(voidAnim);
            IntroRoomsManager.Instance.StartRoomAnim();

            if (onlyOneTime) Destroy(gameObject);
        }
        else if (hasToAvivateObjects)
        {
            if (collision.tag == "Player" || collision.tag == "Kid" || (gameObject.layer == LayerMask.NameToLayer("Diana") && collision.gameObject.layer == LayerMask.NameToLayer("Balloon") && GetComponent<TriggerActivator>().enabled))
            {
                foreach (GameObject element in elements)
                    element.SetActive(true);

                if (onlyOneTime) Destroy(gameObject);
            }

        }
        else
        {
            if (collision.tag == "Player" || collision.tag == "Kid" || (gameObject.layer == LayerMask.NameToLayer("Diana") && collision.gameObject.layer == LayerMask.NameToLayer("Balloon") && GetComponent<TriggerActivator>().enabled))
            {
                foreach (GameObject element in elements)
                    element.SetActive(false);

                if (onlyOneTime) Destroy(gameObject);
            }
        }
    }
}