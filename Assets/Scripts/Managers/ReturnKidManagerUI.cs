using UnityEngine;

public class ReturnKidManagerUI : MonoBehaviour
{
    public GameObject spaceText;
    public float maxDistance = 5.0f;

    private GameObject monster, kid;

    void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Player");
        kid = GameObject.FindGameObjectWithTag("Kid");
    }

    void Update()
    {
        if (monster == null || kid == null || spaceText == null) return;

        float distance = Vector2.Distance(monster.transform.position, kid.transform.position);

        if (distance > maxDistance)
            spaceText.SetActive(true);
        else
            spaceText.SetActive(false);
    }
}