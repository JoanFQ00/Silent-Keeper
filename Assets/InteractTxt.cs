using UnityEngine;

public class InteractTxt : MonoBehaviour
{

    void OnEnable()
    {
        Interact.ShowInteraction += ChangeActive;
    }

    void OnDisable()
    {
        Interact.ShowInteraction += ChangeActive;
    }

    private void ChangeActive(bool active) 
    {
        if (active) 
        {
            gameObject.SetActive(true);
        }
        else 
        {
            gameObject.SetActive(false);
        }
    }
}
