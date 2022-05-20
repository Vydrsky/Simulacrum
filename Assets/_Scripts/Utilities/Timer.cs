using UnityEngine;
using System;

public class Timer {

    /************************ FIELDS ************************/
    
    public float StartTime { get; set; }
    public float Duration { get; set; }

    public bool isRunning { get; set; }

    public event Action OnTimerCountingEnd;

    /************************ METHODS ************************/

    public void StartTimer(float duration) {
        StartTime = Time.time;
        isRunning = true;
        Duration = duration;
    }

    public void Tick() {
        if (!isRunning) {
            return;
        }
        if (Time.time > StartTime + Duration) {
            OnTimerCountingEnd?.Invoke();
        }
    }

    public void StopTimer() {
        isRunning = false;
    }
}
