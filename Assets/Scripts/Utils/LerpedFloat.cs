
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
        if (deltaT == 0 || time == 0) return value;

        var transitionPercent = deltaT / time;

        value = (1f - transitionPercent) * value + transitionPercent * targetValue;

        return value;
    }
}