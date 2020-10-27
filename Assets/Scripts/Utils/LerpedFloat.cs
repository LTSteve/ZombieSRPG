
using UnityEngine;

public class LerpedFloat : LerpedVariable<float>
{
    public LerpedFloat(float transitionTime = 1f, float initialValue = 0f, float initialTargetValue = 0f) : base(transitionTime, initialValue, initialTargetValue)
    {}

    public override float GetValue(float deltaT = 0f)
    {
        if(time == 0)
        {
            value = targetValue;
        }
        if (deltaT == 0 || time == 0 || value == targetValue) return value;

        var transitionPercent = deltaT / time;

        var distancePerTick = (targetValue - previousValue) * transitionPercent;

        var remainingDistance = targetValue - value;

        if (_isSmaller(remainingDistance, distancePerTick))
        {
            value = targetValue;
        }
        else
        {
            value += distancePerTick;
        }

        return value;
    }

    private bool _isSmaller(float d1, float d2)
    {
        return Mathf.Abs(d1) < Mathf.Abs(d2);
    }
}