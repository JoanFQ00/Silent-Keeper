using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BalloonBehaviour : MonoBehaviour
{
    private Animator animator;

    public static Action OnBallonImpacted, OnHasFinished;
    public static Action<Transform> OnBallonExploded;
    public GameObject BalloonParticles;

    [HideInInspector]
    public bool hasTimeFinished;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Start");
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTimeFinished)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")
                || collision.gameObject.layer == LayerMask.NameToLayer("Diana") || collision.gameObject.layer == LayerMask.NameToLayer("Interactable") ||collision.gameObject.tag == "LockerTag")
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    transform.parent.gameObject.GetComponentInParent<Ballooned>().BallonHasEnemyCollided(collision.gameObject);

                OnBallonExploded?.Invoke(transform);
                BalloonParticles.GetComponent<ParticleSystem>().Play();

                OnBallonImpacted?.Invoke();
                animator.SetTrigger("End");
            }
        }
    }

    public void OnEndAnim() 
    {
        transform.parent.gameObject.GetComponentInParent<Ballooned>().bloonTime = 0f;
        BalloonBox.AddBalloon();
        OnHasFinished?.Invoke();
    }
}