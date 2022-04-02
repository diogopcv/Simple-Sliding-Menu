using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SidePanel1Controller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    MainController mainController;
    [SerializeField]
    private DownPanelController downPanel;
    [SerializeField]
    private SidePanel2Controller sidePanel2;
    [SerializeField]
    private float speed = 10f;

    private float canvasWidth;
    private float sizeDrag;
    private RectTransform rect;
    private bool open = false;
    private bool isAnimating = false;
    private float offsetX = 0f;
    private bool clickBegin = false;
    private float deltaDragX = 0;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvasWidth = ((RectTransform)rect.parent).sizeDelta.x;
        offsetX = -0.2f * canvasWidth;
        rect.offsetMin = new Vector2(-canvasWidth, 0f);
        rect.offsetMax = new Vector2(-canvasWidth, 0f);
        sizeDrag = 0.2f * Screen.width;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) && !RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
        {
            ClosePanel();
        }
#else
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began && !RectTransformUtility.RectangleContainsScreenPoint(rect, touch.position))
                {
                    CloseConfPanel();
                }
            }
#endif
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

    IEnumerator OpenCloseAnimation(bool _open)
    {
        open = _open;
        isAnimating = true;
        float targetX = _open ? offsetX : -canvasWidth;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        deltaDragX = eventData.position.x;
        clickBegin = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (clickBegin && deltaDragX > eventData.position.x)
        {
            deltaDragX = Mathf.Abs(deltaDragX - eventData.position.x);
            if (deltaDragX >= sizeDrag)
                ClosePanel();
        }
        clickBegin = false;
    }

    public bool IsAnimating()
    {
        return isAnimating;
    }

    public void OpenDownPanel()
    {
        if (!isAnimating && open)
        {
            ClosePanel();
            downPanel.OpenPanel();
        }
    }

    public void OpenSidePanel2()
    {
        if (!isAnimating && open)
        {
            StartCoroutine(OpenSidePanel2IEnumerator());
        }
    }

    IEnumerator OpenSidePanel2IEnumerator()
    {
        ClosePanel();
        yield return new WaitForSeconds(0.3f);
        sidePanel2.OpenPanel();
    }
}