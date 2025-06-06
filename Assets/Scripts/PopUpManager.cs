using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public GameObject[] PopUps;
    public GameObject TicketPopUp;
    private HashSet<GameObject> activatedPopUps = new HashSet<GameObject>();

    private bool isActivated;
    private float currentActivatedTime;
    public float maxTime = 2.5f;

    private int ticketCount;

    public static PopUpManager Instance;

    void Start()
    {
        foreach (GameObject go in PopUps) 
        {
            go.SetActive(false);
        }
    }

    private void Update()
    {
        CheckTicketPopUp();
        if (isActivated)
        {
            currentActivatedTime += Time.deltaTime;

            if (currentActivatedTime > maxTime)
            {
                foreach (GameObject go in PopUps)
                {
                    go.SetActive(false);
                }
                isActivated = false;
            }
        }
    }

    public void ActivateThisPopUp(GameObject popUp)
    {
        if (activatedPopUps.Contains(popUp))
        {
            return;
        }

        foreach (GameObject go in PopUps)
        {
            go.SetActive(false); 
        }
        popUp.SetActive(true);
        isActivated = true;
        currentActivatedTime = 0f;
    }

    private void CheckTicketPopUp() 
    {
        if (ticketCount == 3) 
        {
            activatedPopUps.Add(TicketPopUp);
        }
    }

    public void TicketObtainted() 
    {
        ticketCount++;
    }
}
