
using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCStatUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FloatingTextManager floatingTextManager;
    [SerializeField] private GameObject statBarPrefab;
    [SerializeField] private Transform statBarContainer;

    [Header("Pool Settings")]
    [SerializeField] private int maxPoolSize = 20;
    [SerializeField] private float displayDuration = 3f;

    [SerializeField] private Vector3 statBarAnchorOffset = new Vector3(0, 1,0 );

    private class StatBarEntry
    {
        public NPCStatBar Bar;
        public Transform Anchor;
        public float RemainingTime;
    }

    private readonly Dictionary<Health, StatBarEntry> activeBars = new();
    private readonly Stack<NPCStatBar> pool = new();

    private void Start()
    {
        foreach (Health health in FindObjectsByType<Health>(FindObjectsSortMode.None))
        {
            if (health.GetComponent<EnemyAI>())
                RegisterHealth(health);
        }
    }

    private void LateUpdate()
    {
        List<Health> expired = null;

        foreach (var pair in activeBars)
        {
            Health health = pair.Key;
            StatBarEntry entry = pair.Value;

            if (health == null || entry.Anchor == null)
            {
                expired ??= new List<Health>();
                expired.Add(health);
                continue;
            }

            entry.RemainingTime -= Time.deltaTime;

            if (entry.RemainingTime <= 0f)
            {
                expired ??= new List<Health>();
                expired.Add(health);
                continue;
            }

            entry.Bar.transform.position = entry.Anchor.position+statBarAnchorOffset;

            entry.Bar.SetHealth(
                health.CurrentHealth,
                health.MaxHealth
            );
        }

        if (expired == null)
            return;

        foreach (Health health in expired)
        {
            if (health != null)
                HideStatBar(health);
            else
                RemoveInvalidEntries();
        }
    }

    public void RegisterHealth(Health health)
    {
        health.OnHealthChange += HandleHealthChange;
    }

    private void HandleHealthChange(HealthChangeInfo info)
    {
        if (info.health == null)
            return;

        floatingTextManager.InitializeText(
            info.health.transform.position,
            info.Amount
        );

        ShowStatBar(info.health);
    }

    private void ShowStatBar(Health health)
    {
        if (activeBars.TryGetValue(health, out StatBarEntry existing))
        {
            existing.RemainingTime = displayDuration;
            existing.Bar.SetHealth(
                health.CurrentHealth,
                health.MaxHealth
            );
            return;
        }

        NPCStatBar bar = GetStatBar();

        if (bar == null)
            return;

        bar.SetHealth(health.CurrentHealth, health.MaxHealth);
        bar.gameObject.SetActive(true);

        activeBars.Add(health, new StatBarEntry
        {
            Bar = bar,
            Anchor = health.transform,
            RemainingTime = displayDuration
        });
    }

    private void HideStatBar(Health health)
    {
        if (!activeBars.TryGetValue(health, out StatBarEntry entry))
            return;

        entry.Bar.gameObject.SetActive(false);
        pool.Push(entry.Bar);

        activeBars.Remove(health);
    }

    private NPCStatBar GetStatBar()
    {
        if (pool.Count > 0)
            return pool.Pop();

        if (activeBars.Count + pool.Count >= maxPoolSize)
            return null;

        GameObject obj = Instantiate(statBarPrefab, statBarContainer);
        NPCStatBar bar = obj.GetComponent<NPCStatBar>();

        obj.SetActive(false);

        return bar;
    }

    private void RemoveInvalidEntries()
    {
        List<Health> invalid = new();

        foreach (var pair in activeBars)
        {
            if (pair.Key == null || pair.Value.Anchor == null)
                invalid.Add(pair.Key);
        }

        foreach (Health health in invalid)
        {
            if (health != null)
                HideStatBar(health);
            else
            {
                RemoveDestroyedTarget(health);
            }
        }
    }

    private void RemoveDestroyedTarget(Health destroyedHealth)
    {
        foreach (var pair in activeBars)
        {
            if (pair.Key == destroyedHealth)
            {
                pair.Value.Bar.gameObject.SetActive(false);
                pool.Push(pair.Value.Bar);
                activeBars.Remove(pair.Key);
                return;
            }
        }
    }
}
