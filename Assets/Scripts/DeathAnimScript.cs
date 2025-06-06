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

        ForceExitMonsterFromLocker();

        AudioManager.Instance.PlaySFX("die");
    }

    private void ForceExitMonsterFromLocker()
    {
        foreach (var locker in Locker.AllLockers)
        {
            if (locker.GetObjectContained() == monster)
            {
                locker.ForceExit();
                break;
            }
        }
    }

    public void OnAnimationFinished()
    {
        if (GameManager.lastCheckpoint)
        {
            SceneChanger.instance.RestartCheckPointZone();
            return;
        }

        GameManager.Instance.SpawnPlayers();

        ResetEntity(monster);
        ResetEntity(kid);

        OnStartGame();
    }

    private void ResetEntity(GameObject entity)
    {
        if (entity.CompareTag("Player"))
        {
            var monsterMove = entity.GetComponent<MonsterMove>();
            monsterMove.ChangeState(KidMachine.States.Stoped);

            var rb = entity.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            var collider = entity.GetComponent<Collider2D>();
            if (collider != null) collider.enabled = true;

            var sr = entity.GetComponent<SpriteRenderer>();
            sr.color = Color.white;
        }
        else if (entity.CompareTag("Kid"))
        {
            var kidMachine = entity.GetComponent<KidMachine>();
            kidMachine.ChangeState(KidMachine.States.Stoped);

            var sr = entity.GetComponent<SpriteRenderer>();
            sr.color = Color.white;
        }
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
