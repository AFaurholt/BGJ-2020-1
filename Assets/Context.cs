using UnityEngine;

public class Context<T> : MonoBehaviour where T : Context<T>
{
    public static T Current = null;

    private void OnEnable()
    {
        if (Current != null)
            Debug.LogWarning($"{name} will override {Current.name}", this);
        Current = (T)this;
    }

    private void OnDisable()
    {
        if (Current == this)
            Current = null;
    }
}