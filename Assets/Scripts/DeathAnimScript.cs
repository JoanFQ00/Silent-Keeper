using UnityEngine;

public class DeathAnimScript : MonoBehaviour
{
    public GameObject im1, im2;

    private GameObject monster, kid;
    private Animator animator;

    [HideInInspector]
    public bool dying = false;

    public static DeathAnimScript Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(dying);
    }
    private void Start()
    {
        monster = GameObject.FindGameObjectWithTag("Player");
        kid = GameObject.FindGameObjectWithTag("Kid");
        animator = GetComponent<Animator>();

        im1.SetActive(true);
        im2.SetActive(true);

        OnStartGame();
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
        im1.SetActive(true);
        im2.SetActive(true);

        dying = true;
        animator.enabled = true;
        animator.SetTrigger("01");

        AudioManager.Instance.PlaySFX("die");
    }

    public void OnAnimationFinished()
    {
        if (GameManager.lastCheckpoint)
        {
            SceneChanger.instance.RestartCheckPointZone();
            return;
        }

        GameManager.Instance.SpawnPlayers();
        kid.GetComponent<KidMachine>().ChangeState(KidMachine.States.Stoped);
        monster.GetComponent<MonsterMove>().ChangeState(KidMachine.States.Stoped);

        kid.GetComponent<SpriteRenderer>().color = Color.white;
        monster.GetComponent<SpriteRenderer>().color = Color.white;

        OnStartGame();
    }

    private void OnStartGame()
    {
        animator.SetTrigger("02");
    }

    public void OnSecondFinish()
    {
        im1.SetActive(false);
        im2.SetActive(false);

        dying = false;
        animator.enabled = false;
    }
}