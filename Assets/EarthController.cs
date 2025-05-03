using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class EarthController : MonoBehaviour
{
    [System.Serializable]
    public class ElementSettings
    {
        public Transform elementObject;
        public Vector3 moveDirection = new Vector3(1, 0, 0);
        public float moveDistance = 1.5f;
        public float scaleMultiplier = 1.2f;
        public float animationDuration = 0.5f;

        public float rotationSpeedX = 0.2f;
        public float rotationSpeedY = 0.2f;
        public float rotationSpeedZ = 0.2f;

        public Vector3 initialLocalPosition;
        public Quaternion initialLocalRotation;
        [HideInInspector] public Vector3 initialLocalScale;

        public Vector3 customSeparationRotationEuler = Vector3.zero;
    }

    public ElementSettings[] elementSettings;

    [Header("Click Settings")]
    [Range(0.2f, 2.0f)]
    public float clickCooldown = 0.5f;
    [Range(0.1f, 1.0f)]
    public float clickDistanceThreshold = 0.3f;
    [Range(0.1f, 2.0f)]
    public float clickTimeThreshold = 0.5f;

    [Header("Trigger Object")]
    public Transform mainClickableObject;

    [Header("Toggle Settings")]
    public Toggle toggle;

    private bool isSeparated = false;
    private bool isDragging = false;
    private Transform currentDraggedObject = null;
    private float lastClickTime;

    private Vector2 lastTouchPosition;
    private Vector2 clickStartPosition;
    private float clickStartTime;
    private bool isClickHandled = false;

    void Start()
    {
        foreach (var element in elementSettings)
        {
            element.initialLocalScale = element.elementObject.localScale;
            if (element.initialLocalPosition == Vector3.zero)
                element.initialLocalPosition = element.elementObject.localPosition;
            if (element.initialLocalRotation == Quaternion.identity)
                element.initialLocalRotation = element.elementObject.localRotation;
        }

        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    public void OnInteractionStart()
    {
        if (Time.time - lastClickTime < clickCooldown)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            isClickHandled = true;
            return;
        }

        clickStartTime = Time.time;
        clickStartPosition = Input.mousePosition;
        lastTouchPosition = Input.mousePosition;
        isClickHandled = false;
        lastClickTime = Time.time;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            if (isSeparated)
            {
                foreach (var element in elementSettings)
                {
                    if (hit.transform == element.elementObject || hit.transform.IsChildOf(element.elementObject))
                    {
                        currentDraggedObject = element.elementObject;
                        isDragging = true;
                        return;
                    }
                }
            }
            else
            {
                if (hit.transform != mainClickableObject && !hit.transform.IsChildOf(mainClickableObject))
                {
                    isClickHandled = true;
                }
            }
        }
        else
        {
            isClickHandled = true;
        }
    }

    public void OnInteractionEnd()
    {
        if (!isClickHandled)
        {
            float clickDuration = Time.time - clickStartTime;
            float clickDistance = Vector2.Distance(clickStartPosition, Input.mousePosition);

            if (clickDuration < clickTimeThreshold && clickDistance < clickDistanceThreshold * Screen.dpi / 160f)
            {
                if (isSeparated) Merge();
                else Separate();

                isClickHandled = true;
            }
        }

        isDragging = false;
        currentDraggedObject = null;
    }

    void OnMouseDown() => OnInteractionStart();
    void OnMouseUp() => OnInteractionEnd();

    void Separate()
    {
        foreach (var element in elementSettings)
        {
            StartCoroutine(MoveAndAnimate(
                element.elementObject,
                element.moveDirection,
                element.moveDistance,
                element.scaleMultiplier,
                element.customSeparationRotationEuler,
                element.animationDuration
            ));
        }

        isSeparated = true;
    }

    public void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            Separate();
        }
        else
        {
            Merge();
        }
    }

    void Merge()
    {
        foreach (var element in elementSettings)
        {
            StartCoroutine(MoveToDefault(
                element.elementObject,
                element.initialLocalPosition,
                element.initialLocalRotation,
                element.initialLocalScale,
                element.animationDuration
            ));
        }

        isSeparated = false;
        isDragging = false;
        currentDraggedObject = null;
    }

    IEnumerator MoveAndAnimate(Transform obj, Vector3 moveDirection, float moveDistance, float scaleMultiplier, Vector3 eulerRotation, float duration)
    {
        Vector3 startPosition = obj.localPosition;
        Vector3 startScale = obj.localScale;
        Quaternion startRotation = obj.localRotation;

        Vector3 targetPosition = startPosition + moveDirection * moveDistance;
        Quaternion targetRotation = startRotation * Quaternion.Euler(eulerRotation);

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            obj.localPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            obj.localScale = Vector3.Lerp(startScale, startScale * scaleMultiplier, timeElapsed / duration);
            obj.localRotation = Quaternion.Lerp(startRotation, targetRotation, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        obj.localPosition = targetPosition;
        obj.localScale = startScale * scaleMultiplier;
        obj.localRotation = targetRotation;
    }

    IEnumerator MoveToDefault(Transform obj, Vector3 originalPosition, Quaternion originalRotation, Vector3 originalScale, float duration)
    {
        Vector3 startPosition = obj.localPosition;
        Quaternion startRotation = obj.localRotation;
        Vector3 startScale = obj.localScale;

        float timeElapsed = 0f;
        while (timeElapsed < duration)
        {
            obj.localPosition = Vector3.Lerp(startPosition, originalPosition, timeElapsed / duration);
            obj.localRotation = Quaternion.Lerp(startRotation, originalRotation, timeElapsed / duration);
            obj.localScale = Vector3.Lerp(startScale, originalScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        obj.localPosition = originalPosition;
        obj.localRotation = originalRotation;
        obj.localScale = originalScale;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                OnInteractionStart();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.position - lastTouchPosition;
                if (delta.magnitude > 0.1f)
                {
                    RotateWithInputDelta(delta);
                    lastTouchPosition = touch.position;

                    if (delta.magnitude > clickDistanceThreshold * Screen.dpi / 160f)
                        isClickHandled = true;
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                OnInteractionEnd();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastTouchPosition = Input.mousePosition;
                OnInteractionStart();
            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Vector2 delta = currentMousePosition - lastTouchPosition;

                if (delta.magnitude > 0.1f)
                {
                    RotateWithInputDelta(delta);
                    lastTouchPosition = currentMousePosition;

                    if (delta.magnitude > clickDistanceThreshold * Screen.dpi / 160f)
                        isClickHandled = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                OnInteractionEnd();
            }
        }
    }

    private void RotateWithInputDelta(Vector2 delta)
    {
        if (delta.magnitude < 0.1f) return;

        float rotationX = delta.y * 0.5f; // Dibalik
        float rotationY = delta.x * 0.5f; // Dibalik

        if (!isSeparated)
        {
            foreach (var element in elementSettings)
            {
                element.elementObject.Rotate(Vector3.right, rotationX * element.rotationSpeedX, Space.World);
                element.elementObject.Rotate(Vector3.up, rotationY * element.rotationSpeedY, Space.World);
            }
        }
        else
        {
            if (isDragging && currentDraggedObject != null)
            {
                float speedX = 0.2f;
                float speedY = 0.2f;

                foreach (var element in elementSettings)
                {
                    if (currentDraggedObject == element.elementObject)
                    {
                        speedX = element.rotationSpeedX;
                        speedY = element.rotationSpeedY;
                        break;
                    }
                }

                currentDraggedObject.Rotate(Vector3.right, rotationX * speedX, Space.World);
                currentDraggedObject.Rotate(Vector3.up, rotationY * speedY, Space.World);
            }
            else
            {
                foreach (var element in elementSettings)
                {
                    element.elementObject.Rotate(Vector3.right, rotationX * element.rotationSpeedX * 0.5f, Space.World);
                    element.elementObject.Rotate(Vector3.up, rotationY * element.rotationSpeedY * 0.5f, Space.World);
                }
            }
        }
    }
}
