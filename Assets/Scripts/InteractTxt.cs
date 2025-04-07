using UnityEngine;
using UnityEngine.Rendering;

public class InteractTxt : MonoBehaviour
{
    [SerializeField] GameObject txt;

    private void Start()
    {
        txt.SetActive(false);
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
        if (parent == txt.transform.parent.gameObject)
        {
            if (hasToActivate)
                txt.SetActive(true);
            else
                txt.SetActive(false);
        }
        else
        {
            txt.SetActive(false);
        }
    }
}
