using UnityEngine;

public class KidParticles : MonoBehaviour
{
    private GameObject kid;

    void Start()
    {
        kid = GameObject.FindWithTag("Kid");        
    }

    void Update()
    {
        gameObject.transform.position = new Vector3(kid.transform.position.x, kid.transform.position.y-0.75f, kid.transform.position.z);         
    }
}
