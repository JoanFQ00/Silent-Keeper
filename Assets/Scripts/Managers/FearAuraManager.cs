using UnityEngine;

public class FearAuraManager : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        if (player == null || AlphaThresholdController.Instance == null)
            return;

        FearAuraEmitter[] enemies = Object.FindObjectsByType<FearAuraEmitter>(FindObjectsSortMode.None);

        float closestDistance = float.MaxValue;
        Color selectedColor = Color.black;

        foreach (FearAuraEmitter enemy in enemies)
        {
            float dist = Vector3.Distance(enemy.transform.position, player.position);
            if (dist <= enemy.detectionRadius && dist < closestDistance)
            {
                closestDistance = dist;

                float t = Mathf.Clamp01(1 - (dist / enemy.detectionRadius));

                Color darkDangerColor = enemy.dangerColor * 0.3f;

                selectedColor = Color.Lerp(Color.black, darkDangerColor, t);
            }
        }

        AlphaThresholdController.Instance.auraColor = selectedColor;
    }
}