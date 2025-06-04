using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private Sprite activatedTrap, disabledTrap, almostOpening;
    [SerializeField] private bool thisActive, isAuto;
    [SerializeField] float alternationTime;
    [SerializeField] LayerMask playerLayer;

    private Collider2D col;
    private SpriteRenderer sr;
    private float counter;
    private bool isNearToBeActivated;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (isAuto)
        {
            col = GetComponent<Collider2D>();

            if (thisActive)
            {
                col.enabled = false;
                sr.sprite = disabledTrap;
            }
            else
            {
                col.enabled = true;
                sr.sprite = activatedTrap;
            }
        }

        isNearToBeActivated = false;
        counter = 0.0f;
    }

    private void Update()
    {
        if (isAuto)
        {
            counter += Time.deltaTime;

            if (counter >= alternationTime / 2.0f && !isNearToBeActivated && thisActive)
            {
                sr.sprite = almostOpening;
                isNearToBeActivated = true;
            }
            else if (counter >= alternationTime)
            {
                thisActive = !thisActive;
                col.enabled = !thisActive;

                if (thisActive)
                    sr.sprite = disabledTrap;
                else
                    sr.sprite = activatedTrap;

                counter -= alternationTime;
                isNearToBeActivated = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            if (!GameManager.lastCheckpoint)
                GameManager.Instance.SpawnPlayers();
            else
                SceneChanger.instance.RestartCheckPointZone();
        }
    }
}