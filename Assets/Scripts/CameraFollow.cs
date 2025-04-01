using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] GameObject target;
    private Transform targetTransform;


    void Start()
    {
        targetTransform = target.transform;
    }

    void Update()
    {
        transform.position = targetTransform.position;
    }
}
