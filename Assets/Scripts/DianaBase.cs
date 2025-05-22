using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DianaBase : MonoBehaviour
{
    private InteractableBase interactable;
    private float timer, timeToDisable;
    private bool hasImpacted;
    private Light2D light2D;

    private void Start()
    {
        hasImpacted = false;
        timeToDisable = 3.0f;
        timer = 0.0f;
        light2D = GetComponentInChildren<Light2D>();
        interactable = GetComponent<InteractableBase>();

        light2D.enabled = true;
    }

    private void Update()
    {
        if (hasImpacted)
        {
            timer += Time.deltaTime;

            if (timer >= timeToDisable)
                gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Balloon"))
        {
            //CAMBIAR ANIMACIÓN A BAJAR
            hasImpacted = true;
            light2D.enabled = false;
            interactable.SetKeyActivated(true);
        }
    }
}