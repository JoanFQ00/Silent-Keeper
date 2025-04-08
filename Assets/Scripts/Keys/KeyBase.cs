using System;
using UnityEngine;

public class KeyBase : MonoBehaviour
{
    [SerializeField] int key;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Players"))
        {
            collision.GetComponent<PlayerInventory>().OnKeyObtained(key);
            Destroy(gameObject);
        }
    }
}