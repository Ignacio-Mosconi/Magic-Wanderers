using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeMenuScrollRect : ScrollRect
{
    [SerializeField] int numberOfSteps;
    [SerializeField] float clampDuration;
    int currentStep;
    float previousScrollSensitivity;

    protected override void OnEnable()
    {
        base.OnEnable();

        horizontalNormalizedPosition = 0f;
        currentStep = 1;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        float currentNormalizedPosition = Mathf.InverseLerp(1, numberOfSteps, currentStep);

        if (currentStep < numberOfSteps && horizontalNormalizedPosition > currentNormalizedPosition + currentNormalizedPosition / 2f)
            currentStep++;

        if (currentStep > 1 && horizontalNormalizedPosition < currentNormalizedPosition - currentNormalizedPosition / 2f)
            currentStep--;

        previousScrollSensitivity = scrollSensitivity;
        scrollSensitivity = 0f;

        StartCoroutine(ClampToStep());
    }

    IEnumerator ClampToStep()
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
    }
}