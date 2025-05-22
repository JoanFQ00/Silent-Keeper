using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    public GameObject[] PopUps;

    private bool isActivated;
    private float currentActivatedTime;
    public float maxTime;

    void Start()
    {
        foreach (GameObject go in PopUps) 
        {
            go.SetActive(false);
        }
    }

    private void Update()
    {
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
        foreach (GameObject go in PopUps)
        {
            go.SetActive(false); 
        }

        popUp.SetActive(true);
        isActivated = true;
        currentActivatedTime = 0f;
    }
}
