using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeMenuScrollRect : ScrollRect
{
    [SerializeField] int numberOfSteps;
    [SerializeField] float clampDuration;
    [SerializeField] Image[] pageMarkers;
    [SerializeField] Sprite[] pageMarkerSprites;
    int currentStep;
    float swapNormalizedDistance;
    float previousScrollSensitivity;

    protected override void Awake()
    {
        base.Awake();

        float contentWidth = content.GetComponent<RectTransform>().sizeDelta.x;
        float swapDistance = contentWidth / (numberOfSteps * 4f);
        
        swapNormalizedDistance = Mathf.InverseLerp(0f, contentWidth, swapDistance);
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        horizontalNormalizedPosition = 0f;
        currentStep = 1;

        pageMarkers[0].sprite = pageMarkerSprites[0];

        for (int i = 1; i < pageMarkers.GetLength(0); i++)
            pageMarkers[i].sprite = pageMarkerSprites[1];
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        bool shouldChangeStep = false;
        float currentNormalizedPosition = Mathf.InverseLerp(1, numberOfSteps, currentStep);

        if (currentStep < numberOfSteps && horizontalNormalizedPosition > currentNormalizedPosition + swapNormalizedDistance)
        {
            currentStep++;
            shouldChangeStep = true;
        }

        if (currentStep > 1 && horizontalNormalizedPosition < currentNormalizedPosition - swapNormalizedDistance)
        {
            currentStep--;
            shouldChangeStep = true;
        }

        previousScrollSensitivity = scrollSensitivity;
        scrollSensitivity = 0f;

        StartCoroutine(ClampToStep(shouldChangeStep));
    }

    IEnumerator ClampToStep(bool changeStep)
    {
        float elapsedTime = 0f;
        float startValue = horizontalNormalizedPosition;
        float endValue = Mathf.InverseLerp(1, numberOfSteps, currentStep);

        while (elapsedTime <= clampDuration)
        {
            elapsedTime += Time.deltaTime;
            horizontalNormalizedPosition = Mathf.Lerp(startValue, endValue, elapsedTime / clampDuration);

            yield return null;
        }

        horizontalNormalizedPosition = endValue;
        scrollSensitivity = previousScrollSensitivity;

        if (changeStep)
            ChangePageMarker();
    }

    void ChangePageMarker()
    {
        for (int i = 0; i < pageMarkers.GetLength(0); i++)
        {
            if (i + 1 == currentStep)
                pageMarkers[i].sprite = pageMarkerSprites[0];
            else
                pageMarkers[i].sprite = pageMarkerSprites[1];
        }
    }
}