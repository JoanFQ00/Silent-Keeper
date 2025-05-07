using UnityEngine;
using System.Collections.Generic;

public class TriggerActivator : MonoBehaviour
{
    [SerializeField] private bool hasToAvivateObjects, onlyOneTime;
    [SerializeField] private List<GameObject> elements = new List<GameObject>();

    private void Start()
    {
        if (hasToAvivateObjects)
            foreach (GameObject element in elements)
                element.SetActive(false);
        else
            foreach (GameObject element in elements)
                element.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasToAvivateObjects)
        {
            if (collision.tag == "Player" || collision.tag == "Kid")
                foreach (GameObject element in elements)
                    element.SetActive(true);

            if (onlyOneTime) Destroy(gameObject);
        }
        else
        {
            if (collision.tag == "Player" || collision.tag == "Kid")
                foreach (GameObject element in elements)
                    element.SetActive(false);

            if (onlyOneTime) Destroy(gameObject);
        }
    }
}