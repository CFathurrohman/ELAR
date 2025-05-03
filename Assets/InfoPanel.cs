using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    public GameObject infoPanel;
    
    public void ClosePanel()
    {
        infoPanel.SetActive(false);
    }
}
