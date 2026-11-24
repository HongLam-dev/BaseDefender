using System;
using System.Collections;
using UnityEngine;

public class ConstructionSite : MonoBehaviour,IWork
{
    [SerializeField] ConstructionData data;
    [SerializeField] int remainingWorkLoad = 100;
    [SerializeField] int baseProgressPerSecond=5;
    [SerializeField] GameObject constructionPrefab;
    [SerializeField] Transform constructionSiteModel;

    [SerializeField] ParticleSystem finishEffect;
    private float timer;
    public event Action OnWorkDone;
    public event Action<int> OnWorkProgress;

    private void Start()
    {
        remainingWorkLoad = data.workLoadToBuild;
    }
    public void Work(float deltaTime, IWorker worker)
    {
        timer += deltaTime;
        if(timer>=1&&remainingWorkLoad>0)
        {
            remainingWorkLoad -= baseProgressPerSecond + (int)worker.GetWorkPower(WorkType.Building);
            OnWorkProgress?.Invoke(remainingWorkLoad);
            timer = 0;
            if(remainingWorkLoad<=0)
            {
                OnWorkDone?.Invoke();
                StartCoroutine(FinishCoroutine());
            }
        }
    }
    public WorkInfo GetWorkInfo()
    {
        return new WorkInfo(data.workLoadToBuild,remainingWorkLoad, WorkType.Building, transform.position, data.icon);
    }
    IEnumerator FinishCoroutine()
    {
        if(finishEffect)
        {
            finishEffect.Clear();
            finishEffect.Play();
        }
        yield return new WaitForSeconds(2);
        Instantiate(constructionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public bool IsDone()
    {
        return remainingWorkLoad <= 0;
    }
}
