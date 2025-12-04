using UnityEngine;

public class AquariumOpenButton : MonoBehaviour
{
    public GameObject aquariumPanel;

    public void OnClickOpen()
    {
        aquariumPanel.SetActive(true);
        
    }
    public void CloseAquarium()
    {
        aquariumPanel.SetActive(false);
        
    }
}
