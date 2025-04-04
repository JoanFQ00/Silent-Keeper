using UnityEngine;
using System.Linq;

public class VendingMachineBase : MonoBehaviour
{
    private enum StateVendingMachine
    {
        NONE_EXECUTING = 0,
        START_CINEMATIC,
        PLAYER_CONTROLLING
    }

    private StateVendingMachine currentState;
    private GameObject[] skillsObtained;

    private void OnEnable()
    {
        // SUSCRIBIRSE AL SCRIPT DE LA PUERTA ChangeToStartState
    }

    private void OnDisable()
    {
        // DESSUSCRIBIRSE AL SCRIPT DE LA PUERTA ChangeToStartState
    }

    void Start()
    {
        skillsObtained = new GameObject[3];
        currentState = StateVendingMachine.NONE_EXECUTING;
    }

    void Update()
    {
        switch (currentState)
        {
            case StateVendingMachine.START_CINEMATIC:
                // Aquí vendría tu lógica de cinemática
                Debug.Log("Cinemática de la máquina expendedora empieza.");
                currentState = StateVendingMachine.PLAYER_CONTROLLING;
                break;

            case StateVendingMachine.PLAYER_CONTROLLING:
                // Aquí vendría tu lógica de control del jugador
                Debug.Log("Ahora el jugador puede interactuar con la máquina.");
                currentState = StateVendingMachine.NONE_EXECUTING;
                break;

            default:
                break;
        }
    }

    private void ChangeToStartState()
    {
        GameObject[] allSkills = SkillManager.Instance.allSkills;

        if (allSkills.Length < 3)
        {
            Debug.LogError("No hay suficientes habilidades para elegir.");
            return;
        }

        // Elegimos 3 habilidades aleatorias únicas
        GameObject[] shuffled = allSkills.OrderBy(x => Random.value).ToArray();

        for (int i = 0; i < 3; i++)
        {
            skillsObtained[i] = shuffled[i];
        }

        // DEBUG
        Debug.Log("Habilidades seleccionadas:");
        foreach (GameObject skill in skillsObtained)
        {
            Debug.Log(skill.GetComponent<ISkill>().GetSkill().selectedSkillName);
        }

        currentState = StateVendingMachine.START_CINEMATIC;
    }
}