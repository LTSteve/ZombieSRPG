using UnityEngine;

public class LerpedVector : LerpedVariable<Vector3>
{
    public LerpedVector(float transitionTime = 1f, Vector3 initialValue = default, Vector3 initialTargetValue = default) : base(transitionTime, initialValue, initialTargetValue)
    { }

    public override Vector3 GetValue(float deltaT = 0f)
    {
        if (time == 0)
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

    private bool _isSmaller(Vector3 d1, Vector3 d2)
    {
        return d1.magnitude < d2.magnitude;
    }
}