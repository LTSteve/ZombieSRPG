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
        if (deltaT == 0 || time == 0) return value;

        var transitionPercent = deltaT / time;

        value = (1f - transitionPercent) * value + transitionPercent * targetValue;

        return value;
    }
}