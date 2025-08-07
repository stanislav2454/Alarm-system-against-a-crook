using UnityEngine;

public class Runner : MonoBehaviour
{
    public bool IsRunning { get; private set; }

    public void ToggleRunning() => 
        IsRunning = !IsRunning;

    public void SetRunning(bool run) => 
        IsRunning = run;
}