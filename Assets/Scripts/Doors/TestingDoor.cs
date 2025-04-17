using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TestingDoor : MonoBehaviour
{
    [SerializeField] int thisKey;
    [SerializeField] bool thisActivated;

    private SpriteRenderer spriteRenderer;
    private ShadowCaster2D shadowCaster2D;

    private Collider2D col;

    void Start()
    {
        shadowCaster2D = GetComponent<ShadowCaster2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (thisActivated)
        {
            spriteRenderer.color = Color.green;
            col.isTrigger = true;
            if(shadowCaster2D != null) shadowCaster2D.enabled = false;
        }
        else
        {
            spriteRenderer.color = Color.red;
            col.isTrigger = false;
            if (shadowCaster2D != null) shadowCaster2D.enabled = true;
        }
    }

    void OnEnable()
    {
        Lever.LeverActivated += OpenDoor;
        PresurePlate.PresureActivated += OpenDoor;
    }

    void OnDisable()
    {
        Lever.LeverActivated -= OpenDoor;
        PresurePlate.PresureActivated -= OpenDoor;
    }

    private void OpenDoor(bool activated, int key)
    {
        if (key == thisKey)
        {
            if (thisActivated != activated)
                thisActivated = activated;
            else
                thisActivated = !activated;

            if (thisActivated)
            {
                spriteRenderer.color = Color.green;
                col.isTrigger = true;
                if (shadowCaster2D != null) shadowCaster2D.enabled = false;
            }
            else
            {
                spriteRenderer.color = Color.red;
                col.isTrigger = false;
                if (shadowCaster2D != null) shadowCaster2D.enabled = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            if (collision.gameObject.GetComponent<PlayerInventory>().GetKeyValue() == thisKey)
            {
                thisActivated = true;
                spriteRenderer.color = Color.green;
                col.isTrigger = true;
                if (shadowCaster2D != null) shadowCaster2D.enabled = false;
            }
        }
    }
}