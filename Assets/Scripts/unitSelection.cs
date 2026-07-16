using UnityEngine;

public class unitSelection : MonoBehaviour
{
    private Unit unit;
    private battleManager bm;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        bm = FindFirstObjectByType<battleManager>();
    }
    private void OnMouseDown()
    {
        bm.Selectunit(unit);
    }
}
