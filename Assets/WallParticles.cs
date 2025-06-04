using UnityEngine;

public class WallPaticles : MonoBehaviour
{
    private void OnEnable()
    {
        DoorBase.OnWallActivated += ActivateParticles;
    }
    private void OnDisable()
    {
        DoorBase.OnWallActivated -= ActivateParticles;
    }

    private void ActivateParticles(Transform transformGlobus)
    {
        transform.position = transformGlobus.position;
        GetComponent<ParticleSystem>().Play();
    }
}
