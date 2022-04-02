using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StripPanelController : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    [Range(0f, 1f)]
    private float upPositionPercentage = 0.5f;
    private float upPosition = 640;
    [SerializeField]
    private RectTransform rectPanel;

    private bool open = false;
    private bool isAnimating = false;
    private Image image;
    private RectTransform childPanel;
    private bool dragBegin = false;
    private float deltaDragY = 0.0f;
    private float sizeDrag;

    void Start()
    {
        rectPanel.anchoredPosition = new Vector2(0, -rectPanel.sizeDelta.y / 2);
        image = GetComponent<Image>();
        image.enabled = false;
        childPanel = transform.GetChild(0).GetComponent<RectTransform>();
        sizeDrag = 0.2f * Screen.width;
        upPosition = GetComponent<RectTransform>().rect.height * upPositionPercentage;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(childPanel, Input.mousePosition))
            {
                ClosePanel();
            }
            else
            {
                dragBegin = true;
                deltaDragY = Input.mousePosition.y;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (dragBegin && deltaDragY > Input.mousePosition.y)
            {
                deltaDragY = Mathf.Abs(deltaDragY - Input.mousePosition.y);
                if (deltaDragY >= sizeDrag)
                    ClosePanel();
            }
            dragBegin = false;
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(childPanel, touch.position))
                {
                    CloseConfPanel();
                }
                else
                {
                    dragBegin = true;
                    deltaDragY = touch.position.y;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (dragBegin && deltaDragY > touch.position.y)
                {
                    deltaDragY = Mathf.Abs(deltaDragY - touch.position.y);
                    if (deltaDragY >= sizeDrag)
                        CloseConfPanel();
                }
                dragBegin = false;
            }
        }
#endif
    }

    public void OpenPanel()
    {
        if (!isAnimating && !open)
        {
            StartCoroutine(OpenCloseAnimation(true));
        }
    }

    public void ClosePanel()
    {
        if (!isAnimating && open)
        {
            StartCoroutine(OpenCloseAnimation(false));
        }
    }

    IEnumerator OpenCloseAnimation(bool _open)
    {
        open = _open;
        isAnimating = true;
        float targetY = _open ? upPosition : -rectPanel.sizeDelta.y / 2;
        float aux = rectPanel.anchoredPosition.y;
        if (_open)
            image.enabled = true;

        while (Mathf.Abs(aux - targetY) > 5f)
        {
            aux = Mathf.Lerp(aux, targetY, speed * Time.deltaTime);
            rectPanel.anchoredPosition = new Vector2(0, aux);

            yield return null;
        }

        rectPanel.anchoredPosition = new Vector2(0, targetY);
        isAnimating = false;
        if (!_open)
            image.enabled = false;
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }
}
