using System.Collections;
using UnityEngine;

public class HeadHUD : MonoBehaviour
{
    public enum FocusTarget { None, Player1, Player2 }

    private FocusTarget currentFocus = FocusTarget.None;
    private Animator animator;
    private string currentState = "00";
    private bool isTransitioning = false;

    public static HeadHUD Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        PlayState("00");
    }

    public void OnFocusChanged(FocusTarget newFocus)
    {
        if (newFocus == currentFocus || isTransitioning) return;

        FocusTarget previousFocus = currentFocus;
        currentFocus = newFocus;

        HandleTransition(previousFocus, currentFocus);
    }

    private void HandleTransition(FocusTarget from, FocusTarget to)
    {
        if (isTransitioning) return;

        string fromState = currentState;
        string toState = GetStateNameFromFocus(to);

        if (fromState == "00")
        {
            PlayState(toState);
        }
        else if (fromState == "01" && toState == "00")
        {
            StartCoroutine(Transition("01_reverse", "00"));
        }
        else if (fromState == "02" && toState == "00")
        {
            StartCoroutine(Transition("02_reverse", "00"));
        }
        else if (fromState == "01" && toState == "02")
        {
            PlayState("01To02");
        }
        else if (fromState == "02" && toState == "01")
        {
            PlayState("02To01");
        }
    }

    private string GetStateNameFromFocus(FocusTarget focus)
    {
        return focus switch
        {
            FocusTarget.None => "00",
            FocusTarget.Player1 => "01",
            FocusTarget.Player2 => "02",
            _ => "00"
        };
    }

    private void PlayState(string stateName)
    {
        animator.Play(stateName, 0, 0f);

        // Estado actual solo cambia si no es reverso ni transición cruzada
        if (stateName == "00" || stateName == "01" || stateName == "02")
        {
            currentState = stateName;
        }
        else if (stateName == "01To02")
        {
            currentState = "02"; // Sabemos que la transición termina en 02
        }
        else if (stateName == "02To01")
        {
            currentState = "01"; // Sabemos que la transición termina en 01
        }
    }

    private IEnumerator Transition(string midState, string finalState)
    {
        isTransitioning = true;

        animator.Play(midState, 0, 0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        PlayState(finalState);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        isTransitioning = false;
    }
}