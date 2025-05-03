using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Button quizButton;  
    public Button infoButton;  
    public Button nextLayerButton;
    public Button prevLayerButton;

    public Button linkButton;  // <<--- Tombol Link di InfoPanel

    public string quizURL = "https://www.youtube.com/watch?v=KmOVNVZEP9o&list=RDKmOVNVZEP9o&start_radio=1";

    public GameObject earthFull;
    public GameObject earthSlice;
    public GameObject crustFull;
    public GameObject crustSlice;
    public GameObject mantleFull;
    public GameObject mantleSlice;
    public GameObject outerCoreFull;
    public GameObject outerCoreSlice;
    public GameObject innerCoreFull;
    public GameObject innerCoreSlice;

    public GameObject infoPanel;
    public Image infoImage;  
    public Image infoButtonImage;  

    private int currentLayerIndex = 0;  

    public Sprite[] infoPanelSprites;  
    public Sprite[] infoButtonSprites; 
    public string[] infoPanelLinks; // <<--- Array Link untuk InfoPanel

    void Start()
    {
        quizButton.onClick.AddListener(OnQuizButtonClicked);
        nextLayerButton.onClick.AddListener(OnNextLayerButtonClicked);
        prevLayerButton.onClick.AddListener(OnPrevLayerButtonClicked);
        infoButton.onClick.AddListener(OnInfoButtonClicked);
        linkButton.onClick.AddListener(OnLinkButtonClicked); // <<--- Tambahkan listener LinkButton

        infoPanel.SetActive(false);

        UpdateEarthLayers();
        UpdateInfoButtonSprite();
    }

    public void OnQuizButtonClicked()
    {
        Application.OpenURL(quizURL);
    }

    void UpdateEarthLayers()
    {
        SetLayerActive(false);

        if (currentLayerIndex == 0)
        {
            earthFull.SetActive(true);
            earthSlice.SetActive(true);
            crustFull.SetActive(true);
            crustSlice.SetActive(true);
            mantleFull.SetActive(true);
            mantleSlice.SetActive(true);
            outerCoreFull.SetActive(true);
            outerCoreSlice.SetActive(true);
            innerCoreFull.SetActive(true);
            innerCoreSlice.SetActive(true);
        }
        else if (currentLayerIndex == 1)
        {
            crustFull.SetActive(true);
            crustSlice.SetActive(true);
            mantleFull.SetActive(true);
            mantleSlice.SetActive(true);
            outerCoreFull.SetActive(true);
            outerCoreSlice.SetActive(true);
            innerCoreFull.SetActive(true);
            innerCoreSlice.SetActive(true);
        }
        else if (currentLayerIndex == 2)
        {
            mantleFull.SetActive(true);
            mantleSlice.SetActive(true);
            outerCoreFull.SetActive(true);
            outerCoreSlice.SetActive(true);
            innerCoreFull.SetActive(true);
            innerCoreSlice.SetActive(true);
        }
        else if (currentLayerIndex == 3)
        {
            outerCoreFull.SetActive(true);
            outerCoreSlice.SetActive(true);
            innerCoreFull.SetActive(true);
            innerCoreSlice.SetActive(true);
        }
        else if (currentLayerIndex == 4)
        {
            innerCoreFull.SetActive(true);
            innerCoreSlice.SetActive(true);
        }
    }

    void SetLayerActive(bool isActive)
    {
        earthFull.SetActive(isActive);
        earthSlice.SetActive(isActive);
        crustFull.SetActive(isActive);
        crustSlice.SetActive(isActive);
        mantleFull.SetActive(isActive);
        mantleSlice.SetActive(isActive);
        outerCoreFull.SetActive(isActive);
        outerCoreSlice.SetActive(isActive);
        innerCoreFull.SetActive(isActive);
        innerCoreSlice.SetActive(isActive);
    }

    void OnNextLayerButtonClicked()
    {
        if (currentLayerIndex < 4)
        {
            currentLayerIndex++;
            UpdateEarthLayers();
            UpdateInfoButtonSprite();
        }
    }

    void OnPrevLayerButtonClicked()
    {
        if (currentLayerIndex > 0)
        {
            currentLayerIndex--;
            UpdateEarthLayers();
            UpdateInfoButtonSprite();
        }
    }

    void UpdateInfoPanel()
    {
        if (infoPanel.activeSelf)
        {
            if (currentLayerIndex >= 0 && currentLayerIndex < infoPanelSprites.Length)
            {
                infoImage.sprite = infoPanelSprites[currentLayerIndex];
            }

            // Update LinkButton visibility
            if (currentLayerIndex >= 0 && currentLayerIndex < infoPanelLinks.Length)
            {
                bool hasLink = !string.IsNullOrEmpty(infoPanelLinks[currentLayerIndex]);
                linkButton.gameObject.SetActive(hasLink);
            }
            else
            {
                linkButton.gameObject.SetActive(false);
            }
        }
    }

    void UpdateInfoButtonSprite()
    {
        if (currentLayerIndex >= 0 && currentLayerIndex < infoButtonSprites.Length)
        {
            infoButtonImage.sprite = infoButtonSprites[currentLayerIndex];
        }
    }

    void OnInfoButtonClicked()
    {
        bool isActive = !infoPanel.activeSelf;
        infoPanel.SetActive(isActive);

        if (isActive)
        {
            UpdateInfoPanel();
        }
    }

    void OnLinkButtonClicked()
    {
        if (currentLayerIndex >= 0 && currentLayerIndex < infoPanelLinks.Length)
        {
            string url = infoPanelLinks[currentLayerIndex];
            if (!string.IsNullOrEmpty(url))
            {
                Application.OpenURL(url);
            }
        }
    }
}
