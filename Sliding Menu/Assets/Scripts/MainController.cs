using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controlador de funcionalidades do painel de menu
/// </summary>
public class MainController : MonoBehaviour
{
    [SerializeField]
    private SidePanel1Controller sidePanel1;
    [SerializeField]
    private StripPanelController stripPanel;
    [SerializeField]
    private Transform logoGroup;
    [SerializeField]
    private Transform playGroup;

    /// <summary>
    /// Ativa painel alvo e desativa painel associado objeto dessa componente
    /// </summary>
    public void PlayGame()
    {
        print("PLAY GAME!");
    }

    public void OpenSidePanel1()
    {
        sidePanel1.OpenPanel();
    }

    public void OpenStripPanel()
    {
        stripPanel.OpenPanel();
    }

    public void ActivateMainPanel(bool activate)
    {
        foreach (Transform trans in logoGroup)
        {
            Image img = trans.GetComponent<Image>();
            if (img != null)
                img.raycastTarget = activate;
        }
        foreach (Transform trans in playGroup)
        {
            Image img = trans.GetComponent<Image>();
            if (img != null)
                img.raycastTarget = activate;
        }
    }

}