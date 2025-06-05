using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorTexture1;
    public Texture2D cursorTexture2;
    public Texture2D cursorTexture3;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;

    public LayerMask interactableLayer;

    private int buttonsPressed = 0;
    private bool isHoveringInteractable = false;
    private GameObject kid;

    void Start()
    {
        Cursor.SetCursor(cursorTexture1, hotspot, cursorMode);
        kid = GameObject.FindGameObjectWithTag("Kid");
    }

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos, interactableLayer);

        if (hit != null)
        {
            if (!isHoveringInteractable && !GameManager.Instance.isPaused)
            {
                bool kidStateOK = true;
                bool otherInLocker = false;
                bool leverOK = true;
                bool boxOK = true;
                bool ballonBoxOK = true;
                bool doorOK = true;

                if (kid.TryGetComponent(out KidMachine kidMachine))
                {
                    kidStateOK = kidMachine.GetCurrentState() != KidMachine.States.HiddenInLocker;

                    if (!kidStateOK)
                        if (hit.TryGetComponent(out Locker locker))
                            kidStateOK = locker.GetObjectContained() == null || locker.GetObjectContained() == kid;
                }

                if (hit.TryGetComponent(out Locker locker2))
                {
                    otherInLocker = locker2.GetObjectContained() != null && locker2.GetObjectContained() != kid;
                }

                if (hit.TryGetComponent(out Lever lever))
                {
                    leverOK = lever.OnlyKid();
                }

                if (hit.TryGetComponent(out CajaMovible box))
                {
                    boxOK = false;
                }

                if (hit.TryGetComponent(out BalloonBox balBox))
                {
                    boxOK = false;
                }

                if (hit.TryGetComponent(out DoorBase door))
                {
                    doorOK = false;
                }

                if (kidStateOK && !otherInLocker && leverOK && boxOK && doorOK && ballonBoxOK)
                {
                    Cursor.SetCursor(cursorTexture3, hotspot, cursorMode);
                    isHoveringInteractable = true;
                }
            }
        }
        else
        {
            if (isHoveringInteractable)
            {
                Cursor.SetCursor(buttonsPressed > 0 ? cursorTexture2 : cursorTexture1, hotspot, cursorMode);
                isHoveringInteractable = false;
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            buttonsPressed++;
            if (!isHoveringInteractable)
                Cursor.SetCursor(cursorTexture2, hotspot, cursorMode);
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            buttonsPressed = Mathf.Max(0, buttonsPressed - 1);

            if (buttonsPressed == 0 && !isHoveringInteractable)
            {
                Cursor.SetCursor(cursorTexture1, hotspot, cursorMode);
            }
        }
    }
}