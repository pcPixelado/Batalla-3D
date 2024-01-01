using UnityEngine;
using UnityEngine.UI;

public class teclaselector : MonoBehaviour
{
    public GameObject paneltecla;

    void Start()
    {

        paneltecla.SetActive(false);
    }

    public void ToggleTeclaPanel()
    {

        paneltecla.SetActive(!paneltecla.activeSelf);
    }
}
