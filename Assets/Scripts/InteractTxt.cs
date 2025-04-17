using UnityEngine;

public class InteractTxt : MonoBehaviour
{
    [SerializeField] GameObject txtMonster, txtKid;

    private void Start()
    {
        txtMonster.SetActive(false);
        txtKid.SetActive(false);
    }

    void OnEnable()
    {
        Interact.ShowInteractionMonster += VisualMonsterText;
        Interact.ShowInteractionKid += VisualKidText;
    }

    void OnDisable()
    {
        Interact.ShowInteractionMonster -= VisualMonsterText;
        Interact.ShowInteractionKid -= VisualKidText;
    }

    private void VisualMonsterText(GameObject parent)
    {
        if (parent == txtMonster.transform.parent.gameObject)
            txtMonster.SetActive(true);
        else
            txtMonster.SetActive(false);
    }
    private void VisualKidText(GameObject parent)
    {
        if (parent == txtKid.transform.parent.gameObject)
            txtKid.SetActive(true);
        else
            txtKid.SetActive(false);
    }
}