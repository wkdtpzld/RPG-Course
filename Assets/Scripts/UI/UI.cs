using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    public UI_ItemTooltip itemTooltip;

    private void Start()
    {

    }


    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        Debug.Log(_menu);

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }
}
