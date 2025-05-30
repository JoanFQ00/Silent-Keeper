using UnityEngine;

public class SortOrderInLayerByPosition : MonoBehaviour
{
    public bool m_IsStatic;
    private void Start()
    {
        SortOrderInLayer();
    }
    private void Update()
    {
        if(!m_IsStatic)
            SortOrderInLayer();
    }
    void SortOrderInLayer()
    {
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.position.y*100);
    }
}
