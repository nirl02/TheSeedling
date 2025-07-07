using UnityEngine;
using UnityEngine.UI;

public class PortalUI : MonoBehaviour
{

    [SerializeField] GameObject PortalCanvas;
    public static PortalUI Instance;

        private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        PortalCanvas.SetActive(false);
    }


    public void ShowCanvas()
    {
    
            PortalCanvas.SetActive(true);

        
    }

    public void HideCanvas()
    {
      
            PortalCanvas.SetActive(false);

    }
}
