using UnityEngine;

public struct WorkInfo
{
    public int originalWorload;
    public int remainingWorkload { get; private set; }
    public WorkType type { get; private set; }
    public Vector3 position { get; private set; }
    public Sprite icon { get; private set; }

    public WorkInfo(int originalWorload,int remainingWorkload, WorkType type, Vector3 position, Sprite icon)
    {
        this.originalWorload = originalWorload;
        this.remainingWorkload = remainingWorkload;
        this.type = type;
        this.position = position;
        this.icon = icon;
    }
}