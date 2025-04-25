using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using static SkillBase;

public class VendingMachineBase : MonoBehaviour
{
    [SerializeField] private GameObject panelMachine, finalPanel, ButtonSkill1, ButtonSkill2, ButtonSkill3, Slot1UI, Slot2UI, Slot3UI, SkillSelectedUI;

    private GameObject[] skillsObtained; 
    private GameObject skillSelected;

    public static VendingMachineBase Instance;

    private void OnEnable()
    {
        DoorBase.OnZoneEnded += OnStartCinematic;
    }

    private void OnDisable()
    {
        DoorBase.OnZoneEnded -= OnStartCinematic;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);
    }

    void Start()
    {
        skillsObtained = new GameObject[3];
        skillSelected = null;

        panelMachine.SetActive(false);
        finalPanel.SetActive(false);

        ButtonSkill1.GetComponent<Button>().onClick.AddListener(OnSkill1Selected);
        ButtonSkill2.GetComponent<Button>().onClick.AddListener(OnSkill2Selected);
        ButtonSkill3.GetComponent<Button>().onClick.AddListener(OnSkill3Selected);

        Slot1UI.GetComponent<Button>().onClick.AddListener(OnSlot1Selected);
        Slot2UI.GetComponent<Button>().onClick.AddListener(OnSlot2Selected);
        Slot3UI.GetComponent<Button>().onClick.AddListener(OnSlot3Selected);
    }

    private void PickSkills()
    {
        //MIRAR DE ESCOGER LAS QUE NO TIENES
        GameObject[] allSkills = SkillManager.Instance.allSkills;

        if (allSkills.Length < 3)
        {
            //No hay suficientes habilidades para elegir
            return;
        }

        GameObject[] shuffled = allSkills.OrderBy(x => Random.value).ToArray();

        for (int i = 0; i < 3; i++)
        {
            skillsObtained[i] = shuffled[i];
        }
    }

    private void OnStartCinematic()
    {
        //GameManager.Instance.Pause(); MIRAR DE HACER OTRA FORMA DE PONERLO EN PAUSA DESDE EL GAMEMANAGER

        ButtonSkill1.SetActive(false);
        ButtonSkill2.SetActive(false);
        ButtonSkill3.SetActive(false);

        panelMachine.SetActive(true);
        PickSkills();

        panelMachine.GetComponent<Animator>().SetTrigger("StartAnim");
    }

    public void OnEndCinematic()
    {
        ButtonSkill1.SetActive(true);
        ButtonSkill2.SetActive(true);
        ButtonSkill3.SetActive(true);

        ButtonSkill1.GetComponent<Image>().sprite = skillsObtained[0].GetComponent<ISkill>().GetSkill().bigImage;
        ButtonSkill2.GetComponent<Image>().sprite = skillsObtained[1].GetComponent<ISkill>().GetSkill().bigImage;
        ButtonSkill3.GetComponent<Image>().sprite = skillsObtained[2].GetComponent<ISkill>().GetSkill().bigImage;

        //CARGAR DESCRIPCIONES Y TODA LA INFO DE SKILLBASE ;)
    }

    public void OnSkill1Selected()
    {
        skillSelected = skillsObtained[0];
        InitialColorUI(skillSelected);
    }

    public void OnSkill2Selected()
    {
        skillSelected = skillsObtained[1];
        InitialColorUI(skillSelected);
    }

    public void OnSkill3Selected()
    {
        skillSelected = skillsObtained[2];
        InitialColorUI(skillSelected);
    }

    public void OnSlot1Selected()
    {
        SkillManager.Instance.AssignSkillToSlot(skillSelected, 1);
        finalPanel.GetComponent<Animator>().SetTrigger("EndAnim");
    }
    public void OnSlot2Selected()
    {
        SkillManager.Instance.AssignSkillToSlot(skillSelected, 2);
        finalPanel.GetComponent<Animator>().SetTrigger("EndAnim");
    }

    public void OnSlot3Selected()
    {
        SkillManager.Instance.AssignSkillToSlot(skillSelected, 3);
        finalPanel.GetComponent<Animator>().SetTrigger("EndAnim");
    }

    private void InitialColorUI(GameObject skillObject)
    {
        SkillType skill = skillObject.GetComponent<ISkill>().GetSkill().currentSkillType;

        finalPanel.SetActive(true);
        panelMachine.SetActive(false);

        Slot1UI.GetComponent<Image>().sprite = SkillManager.Instance.GetSlot(1).GetComponent<ISkill>().GetSkill().bigImage;
        Slot2UI.GetComponent<Image>().sprite = SkillManager.Instance.GetSlot(2).GetComponent<ISkill>().GetSkill().bigImage;
        Slot3UI.GetComponent<Image>().sprite = SkillManager.Instance.GetSlot(3).GetComponent<ISkill>().GetSkill().bigImage;
        SkillSelectedUI.GetComponent<Image>().sprite = skillObject.GetComponent<ISkill>().GetSkill().bigImage;

        if (skill == SkillType.ACTIVE)
        {
            Slot3UI.GetComponent<Button>().interactable = false;
            Slot3UI.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            Slot1UI.GetComponent<Button>().interactable = false;
            Slot2UI.GetComponent<Button>().interactable = false;

            Slot1UI.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            Slot2UI.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    public void ReanudeToGame()
    {
        Slot1UI.GetComponent<Button>().interactable = true;
        Slot2UI.GetComponent<Button>().interactable = true;
        Slot3UI.GetComponent<Button>().interactable = true;

        Slot1UI.GetComponent<Image>().color = Color.white;
        Slot2UI.GetComponent<Image>().color = Color.white;
        Slot3UI.GetComponent<Image>().color = Color.white;

        finalPanel.SetActive(false);

        GameManager.Instance.RestartTime();
    }
}