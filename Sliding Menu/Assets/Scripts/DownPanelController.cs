using System.Collections;
using UnityEngine;

public class DownPanelController : MonoBehaviour
{
    [SerializeField]
    MainController mainController;
    [SerializeField]
    private float speed = 10f;

    private float canvasHeight;
    private RectTransform rect;
    private bool open = false;
    private bool isAnimating = false;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasHeight = ((RectTransform)rect.parent).sizeDelta.y;
        rect.offsetMin = new Vector2(0f, -canvasHeight);
        rect.offsetMax = new Vector2(0f, -canvasHeight);
    }

    public void OpenPanel()
    {
        if (!isAnimating && !open)
        {
            mainController.ActivateMainPanel(false);
            StartCoroutine(OpenCloseAnimation(true));
        }
    }

    public void ClosePanel()
    {
        if (!isAnimating && open)
        {
            mainController.ActivateMainPanel(true);
            StartCoroutine(OpenCloseAnimation(false));
        }
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }

    private IEnumerator OpenCloseAnimation(bool _open)
    {
        open = _open;
        isAnimating = true;
        float targetY = _open ? 0f : -canvasHeight;
        float aux = rect.offsetMin.y;

        while (Mathf.Abs(aux - targetY) > 1f)
        {
            aux = Mathf.Lerp(aux, targetY, speed * Time.deltaTime);
            rect.offsetMin = new Vector2(0f, aux);
            rect.offsetMax = new Vector2(0f, aux);
            yield return null;
        }

        rect.offsetMin = new Vector2(0f, targetY);
        rect.offsetMax = new Vector2(0f, targetY);
        isAnimating = false;
    }
}