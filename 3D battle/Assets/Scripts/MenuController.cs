using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject panelMenu; 

    void Start()
    {
        
        panelMenu.SetActive(false);
    }

    public void ToggleMenuPanel()
    {
        
        panelMenu.SetActive(!panelMenu.activeSelf);
    }
}
