using UnityEngine;
using UnityEngine.InputSystem;

public class TestingDoor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] int thisKey;
    [SerializeField] bool thisActivated;

    private Collider2D col;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        if (thisActivated)
        {
            spriteRenderer.color = Color.green;
            col.isTrigger = true;
        }
        else
        {
            spriteRenderer.color = Color.red;
            col.isTrigger = false;
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
            thisActivated = !thisActivated;

            if (thisActivated)
            {
                spriteRenderer.color = Color.green;
                col.isTrigger = true;
            }
            else
            {
                spriteRenderer.color = Color.red;
                col.isTrigger = false;
            }
        }
    }
}
