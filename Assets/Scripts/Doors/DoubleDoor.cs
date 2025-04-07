using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] int thisKeyOne;
    [SerializeField] int thisKeyTwo;
    [SerializeField] bool thisActivated;

    private bool OneActivated;
    private bool TwoActivated;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (thisActivated) 
        {
            spriteRenderer.color = Color.green;
        }
        else 
        {
            spriteRenderer.color = Color.red;
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

        else 
        {
            OneActivated = false;
            TwoActivated = false;
        }

        if (thisActivated) 
        {
            spriteRenderer.color = Color.green;
        }
        else 
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void OpenDoor(bool activated, int key) 
    {
        if (key == thisKeyOne)
        {
            OneActivated = true;
        }

        else if (key == thisKeyTwo) 
        {
            TwoActivated = true;
        }
    }
}
