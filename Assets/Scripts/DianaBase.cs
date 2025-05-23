using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DianaBase : MonoBehaviour
{
    private InteractableBase interactable;
    private Light2D light2D;
    //private Animation anim;

    private void Start()
    {
        light2D = GetComponentInChildren<Light2D>();
        interactable = GetComponent<InteractableBase>();
        //anim = GetComponent<Animation>();

        //anim.enabled = true;
        light2D.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Balloon"))
        {
            light2D.enabled = false;
            interactable.SetKeyActivated(true);
            //anim.enabled = false;
        }
    }

    //DESDE ANIMACIÓN (FINAL)
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}