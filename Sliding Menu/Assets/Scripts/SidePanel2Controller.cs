using System.Collections;
using UnityEngine;

public class SidePanel2Controller : MonoBehaviour
{
    [SerializeField]
    MainController mainController;
    [SerializeField]
    private float speed = 10f;

    private float canvasWidth;
    private RectTransform rect;
    private bool open = false;
    private bool isAnimating = false;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasWidth = ((RectTransform)rect.parent).sizeDelta.x;
        rect.offsetMin = new Vector2(-canvasWidth, 0f);
        rect.offsetMax = new Vector2(-canvasWidth, 0f);
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
        float targetX = _open ? 0f : -canvasWidth;
        float aux = rect.offsetMin.x;

        while (Mathf.Abs(aux - targetX) > 1f)
        {
            aux = Mathf.Lerp(aux, targetX, speed * Time.deltaTime);
            rect.offsetMin = new Vector2(aux, 0f);
            rect.offsetMax = new Vector2(aux, 0f);
            yield return null;
        }

        rect.offsetMin = new Vector2(targetX, 0f);
        rect.offsetMax = new Vector2(targetX, 0f);
        isAnimating = false;
    }
}