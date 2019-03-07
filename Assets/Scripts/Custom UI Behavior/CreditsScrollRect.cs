using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreditsScrollRect : ScrollRect
{
    [SerializeField] float rollDuration;
    float elapsedTime;
    bool isRolling;

    protected override void OnEnable()
    {
        base.OnEnable();

        verticalNormalizedPosition = 1f;
        elapsedTime = 0f;
        isRolling = true;
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        base.OnBeginDrag(data);

        isRolling = false;
    }

    public override void OnEndDrag(PointerEventData data)
    {
        base.OnEndDrag(data);

        float interpolationValue = Mathf.InverseLerp(1f, 0f, verticalNormalizedPosition);

        elapsedTime = rollDuration * interpolationValue;
        isRolling = true;
    }

    void Update()
    {
        if (verticalNormalizedPosition > 0f && isRolling)
        {
            elapsedTime += Time.deltaTime;

            verticalNormalizedPosition = Mathf.Lerp(1f, 0f, elapsedTime / rollDuration);
        }
    }
}