using UnityEngine;

public class CharacterSortingHandler : MonoBehaviour
{
    [SerializeField] private Transform kid;
    [SerializeField] private Transform monster;

    private SpriteRenderer kidRenderer;
    private SpriteRenderer monsterRenderer;

    void Start()
    {
        if (kid == null) kid = GameObject.FindGameObjectWithTag("Kid").transform;
        if (monster == null) monster = GameObject.FindGameObjectWithTag("Player").transform;

        kidRenderer = kid.GetComponent<SpriteRenderer>();
        monsterRenderer = monster.GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPaused)
            return;

        float kidY = kid.position.y;
        float monsterY = monster.position.y;
    }
}
