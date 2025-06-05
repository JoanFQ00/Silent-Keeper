using UnityEngine;

public class DeathAnimScript : MonoBehaviour
{
    private GameObject monster, kid;

    private void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Player");
        kid = GameObject.FindGameObjectWithTag("Kid");
    }

    private void OnEnable()
    {
        EnemyDetector.OnDugbullState += PlayAnimation;
    }

    private void OnDisable()
    {
        EnemyDetector.OnDugbullState -= PlayAnimation;
    }

    private void PlayAnimation()
    {

    }

    private void OnAnimationFinished()
    {
        GameManager.Instance.SpawnPlayers();
        _kid.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);
    }
}