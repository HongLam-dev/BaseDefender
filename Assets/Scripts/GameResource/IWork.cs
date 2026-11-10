using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public interface IWork
{
    public void Work(float deltaTime, IWorker worker);
    public WorkInfo GetWorkInfo();
}

public enum WorkType
{
    Lumbering,
    Mining,
    Building
}