using UnityEngine;

public struct WorkInfo
{
    public int remainingWorkload { get; private set; }
    public WorkType type { get; private set; }
    public Vector3 position { get; private set; }
    public Sprite icon { get; private set; }

    public WorkInfo(int remainingWorkload, WorkType type, Vector3 position, Sprite icon)
    {
        this.remainingWorkload = remainingWorkload;
        this.type = type;
        this.position = position;
        this.icon = icon;
    }
}