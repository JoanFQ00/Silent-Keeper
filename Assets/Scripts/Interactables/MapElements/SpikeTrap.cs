using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private Sprite trapImage;
    [SerializeField] private bool thisActive, isAuto;
    [SerializeField] float alternationTime;
    [SerializeField] LayerMask playerLayer;

    private Collider2D col;
    private SpriteRenderer sr;
    private float counter;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = trapImage;

        if (isAuto)
        {
            col = GetComponent<Collider2D>();

            if (thisActive)
            {
                col.enabled = false;
                sr.color = Color.green;
            }
            else
            {
                col.enabled = true;
                sr.color = Color.red;
            }
        }

        counter = 0.0f;
    }

    private void Update()
    {
        if (isAuto)
        {
            counter += Time.deltaTime;

            if (counter >= alternationTime)
            {
                thisActive = !thisActive;
                col.enabled = !thisActive;

                if (thisActive)
                    sr.color = Color.green;
                else 
                    sr.color = Color.red;

                counter -= alternationTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
            SceneChanger.instance.RestartZone();
    }
}