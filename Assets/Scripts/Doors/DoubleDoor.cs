using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] int thisKeyOne;
    [SerializeField] int thisKeyTwo;
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
        
    }

    void OnDisable()
    {
        
    }


}
