using UnityEngine;

public class InteractTxt : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        Interact.ShowInteraction += ChangeActive;
    }

    void OnDisable()
    {
        Interact.ShowInteraction -= ChangeActive;
    }

    private void ChangeActive(GameObject parent, bool hasToActivate)
    {
        if (parent == gameObject.transform.parent.gameObject)
        {
            if (hasToActivate)
                gameObject.SetActive(true);
            else
                gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
