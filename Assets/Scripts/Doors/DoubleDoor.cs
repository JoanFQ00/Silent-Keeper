using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] int thisKeyOne;
    [SerializeField] int thisKeyTwo;
    [SerializeField] bool thisActivated;

    private bool OneActivated;
    private bool TwoActivated;
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

        OneActivated = false;
        TwoActivated = false;
    }

    void OnEnable()
    {
        PresurePlate.PresureActivated += OpenDoor;
    }

    void OnDisable()
    {
        PresurePlate.PresureActivated -= OpenDoor;
    }

    void Update()
    {
        if (OneActivated && TwoActivated)   
        {
            thisActivated = true;
        }
        //else 
        //{
        //    OneActivated = false;
        //    TwoActivated = false;
        //}

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

    private void OpenDoor(bool activated, int key) 
    {
        if (key == thisKeyOne)
        {
            OneActivated = activated;
        }
        else if (key == thisKeyTwo) 
        {
            TwoActivated = activated;
        }
    }
}
