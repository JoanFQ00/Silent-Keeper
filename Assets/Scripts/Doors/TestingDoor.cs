using UnityEngine;
using UnityEngine.InputSystem;

public class TestingDoor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] int thisKey;
    [SerializeField] bool thisActivated;
    
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
    }

    void OnEnable() 
    {
        Lever.LeverActivated += OpenDoor;
    }

    void OnDisable()
    {
        Lever.LeverActivated -= OpenDoor;
    }

    
    private void OpenDoor(bool activated, int key ) 
    {
        if (key == thisKey) 
        {
            thisActivated = !thisActivated;

            if(thisActivated) 
            {
                spriteRenderer.color = Color.green;
            }
            else 
            {
                spriteRenderer.color = Color.red;
            }
        }
    }
}
