using UnityEngine;

public class GlobusPartciles : MonoBehaviour
{
    private void OnEnable()
    {
        BalloonBehaviour.OnBallonExploded += ActivateParticles;
    }
    private void OnDisable()
    {
        BalloonBehaviour.OnBallonExploded -= ActivateParticles;
    }

    private void ActivateParticles(Transform transformGlobus) 
    {
        transform.position = transformGlobus.position;
        GetComponent<ParticleSystem>().Play();
    }
}
