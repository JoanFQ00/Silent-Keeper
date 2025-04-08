using UnityEngine;

public class Ballooned : MonoBehaviour, ISkill
{
    [Header("Skill (ScriptableObject)")]
    public SkillBase thisSkill;

    [Header("Skill Variables")]
    public float bloonSpeed, bloonTime;
    public GameObject balloon;

    private Vector3 mousePos, ballonDirection;

    private float defaultBuffTime;
    //private GameObject player;
    private bool finishedSkill;

    public void Execute(GameObject monster)
    {
        defaultBuffTime = bloonTime;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        balloon.transform.position = monster.transform.position;
        ballonDirection = (mousePos - balloon.transform.position).normalized;

        balloon.SetActive(true);
        finishedSkill = false;
    }

    public void UpdateSkill()
    {
        if (bloonTime >= 0f)
        {
            balloon.transform.Translate(ballonDirection * bloonSpeed * Time.deltaTime);
            bloonTime -= Time.deltaTime;
        }
        else
        {
            bloonTime = defaultBuffTime;

            balloon.SetActive(false);
            finishedSkill = true;
        }
    }

    public SkillBase GetSkill()
    {
        return thisSkill;
    }

    public void SetInitialHasFinished()
    {
        finishedSkill = true;
    }

    public bool HasFinished()
    {
        return finishedSkill;
    }
}