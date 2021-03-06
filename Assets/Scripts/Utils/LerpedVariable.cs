﻿
public abstract class LerpedVariable<T>
{
    protected T value = default(T);
    protected T targetValue = default(T);
    protected T previousValue = default(T);
    protected float time = 1f;

    public LerpedVariable(float transitionTime = 1f, T initialValue = default, T initialTargetValue = default)
    {
        value = initialValue;
        time = transitionTime;
        targetValue = initialTargetValue;
    }

    public abstract T GetValue(float deltaT);
    public void SetValue(T newTargetValue)
    {
        previousValue = value;
        targetValue = newTargetValue;
    }
    public void SetTime(float newTime)
    {
        time = newTime;
    }
    public void HardSetValue(T newValue)
    {
        targetValue = newValue;
        value = newValue;
    }

    public override string ToString()
    {
        return value.ToString() + "(" + targetValue.ToString() + ")";
    }
}