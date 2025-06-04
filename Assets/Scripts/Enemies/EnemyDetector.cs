using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;



public class EnemyDetector : TaggedMonobehaviour
{
    private LayerMask wallLayer;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Transform _player;
    private Transform _kid;

    private Light2D _light2D;

    private float VisionRange;
    private float VisionAngle;
    private float stunTime;

    public enum EnemyState
    {
        NORMAL = 0,
        STUNED
    }

    public enum Characters
    {
        BOTH,
        ONE
    }

    public Characters enemyPlayerDetector;
    private EnemyState enemyState;

    void Start()
    {
        wallLayer = LayerMask.GetMask("Wall");

        startPosition = transform.position;
        startRotation = transform.rotation;

        stunTime = 0f;
        enemyState = EnemyState.NORMAL;
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _kid = GameObject.FindGameObjectWithTag("Kid")?.transform;

        _light2D = GetComponent<Light2D>();
        Color color = Color.white;

        //if (enemyPlayerDetector == Characters.ONE)
        //{
        //    if (ColorUtility.TryParseHtmlString("#274C4D", out color))
        //        _light2D.color = color;
        //}
        //else
        //    if (ColorUtility.TryParseHtmlString("#FFA132", out color))
        //    _light2D.color = color;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isPaused)
            return;

        if (enemyState == EnemyState.NORMAL)
        {
            if (_light2D != null)
            {
                VisionRange = _light2D.pointLightOuterRadius;
                VisionAngle = _light2D.pointLightOuterAngle;
            }

            if (_light2D.enabled && !TransitionCamAnimation.Instance.playersInTransition && !IntroRoomsManager.Instance.roomInAnimation)
            {
                if (enemyPlayerDetector == Characters.BOTH)
                {
                    if (CheckTargetInVision(_player) || CheckTargetInVision(_kid))
                        SendToSpawner();
                }
                else
                {
                    if (CheckTargetInVision(_kid))
                        SendToSpawner();
                }
            }
        }
        else
        {
            if (stunTime >= 0f)
            {
                stunTime -= Time.deltaTime;
            }
            else
            {
                stunTime = 0f;
                SetEnemyState(EnemyState.NORMAL);
            }
        }
    }

    private bool CheckTargetInVision(Transform target)
    {
        if (target == null) return false;

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget <= VisionRange)
        {
            float angleToTarget = Vector2.Angle(transform.up, directionToTarget);
            if (angleToTarget < VisionAngle / 2f)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, wallLayer);

                if (target.gameObject.tag == "Player")
                {
                    if (target.GetComponent<MonsterMove>().GetMonsterState() != KidMachine.States.HiddenInLocker && target.GetComponent<MonsterMove>().GetHiddenMonsterState() != KidMachine.States.HiddenInBush)
                    {
                        if (hit.collider == null)
                            return true;

                        if (hit.collider.isTrigger)
                            return true;

                        return hit.collider.transform == target;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (target.gameObject.tag == "Kid")
                {
                    if (target.GetComponent<KidMachine>().GetCurrentState() != KidMachine.States.HiddenInLocker && target.GetComponent<KidMachine>().GetHiddenKidState() != KidMachine.States.HiddenInBush)
                    {
                        if (hit.collider == null)
                            return true;

                        if (hit.collider.isTrigger)
                            return true;

                        return hit.collider.transform == target;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
    }

    public void SetStunTime(float time)
    {
        stunTime = time;
    }

    public void SetEnemyState(EnemyState state)
    {
        enemyState = state;

        if (enemyState == EnemyState.STUNED)
            _light2D.enabled = false;
        else
            _light2D.enabled = true;
    }

    public EnemyState GetEnemyState()
    {
        return enemyState;
    }

    private void SendToSpawner()
    {
        if (_light2D != null)
        {
            _light2D.color = Color.black;
        }

        //if (/*HABILITAT BOOL PER PREGUNTAR SI S'HA UTILITZAT*/)
        //    UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        //else
        //{
        //    ResetValues();
        //}

        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        SceneChanger.instance.RestartZone();
    }

    //private void ResetValues()
    //{
    //    startPosition = transform.position;
    //    startRotation = transform.rotation;

    //    stunTime = 0f;
    //    enemyState = EnemyState.NORMAL;
    //    _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    //    _kid = GameObject.FindGameObjectWithTag("Kid")?.transform;

    //    _light2D = GetComponent<Light2D>();
    //    Color color = Color.white;

    //    if (enemyPlayerDetector == Characters.ONE)
    //    {
    //        if (ColorUtility.TryParseHtmlString("#274C4D", out color))
    //            _light2D.color = color;
    //    }
    //    else
    //        if (ColorUtility.TryParseHtmlString("#FFA132", out color))
    //            _light2D.color = color;
    //}

    private void OnDrawGizmos()
    {
        Light2D light = GetComponent<Light2D>();
        if (light == null) return;

        float range = light.pointLightOuterRadius;
        float angle = light.pointLightOuterAngle;

        Vector3 origin = transform.position;
        Vector3 forward = transform.up;

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(origin, range);

        Quaternion leftRotation = Quaternion.Euler(0, 0, -angle / 2f);
        Quaternion rightRotation = Quaternion.Euler(0, 0, angle / 2f);

        Vector3 leftDir = leftRotation * forward;
        Vector3 rightDir = rightRotation * forward;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(origin, origin + leftDir * range);
        Gizmos.DrawLine(origin, origin + rightDir * range);
    }
}