using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
/// Controls the visual representation and animation of the Frog's tongue.
/// </summary>
public class TongueController : MonoBehaviour
{
    public Transform tongueRoot;
    public LineRenderer lineRenderer;
    public float speed = 10f; // Speed of tongue movement
    public Material tongueMaterial;

    public event Action OnTongueRetracted;

    private void Awake()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.useWorldSpace = true;
        
        if (tongueMaterial != null)
            lineRenderer.material = tongueMaterial;
            
        // Initial state
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, tongueRoot.position);
    }

    public void StartExtendTongue(List<Vector2Int> pathGridPoints)
    {
        StartCoroutine(TongueLookingRoutine(pathGridPoints));
    }

    private IEnumerator TongueLookingRoutine(List<Vector2Int> pathGridPoints)
    {
        // 1. Convert Grid Points to World Points
        List<Vector3> worldPoints = new List<Vector3>();
        worldPoints.Add(tongueRoot.position);

        TileManager tileManager = SingletonManager.GetSingleton<TileManager>();
        foreach (var p in pathGridPoints)
        {
            Tile t = tileManager.GetTileAt(p.x, p.y);
            if (t != null)
            {
                // Maintain tongue height same as root
                Vector3 targetPos = t.transform.position;
                targetPos.y = tongueRoot.position.y;
                worldPoints.Add(targetPos);
            }
        }

        // 2. Animate Extension
        // We extend point by point.
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, tongueRoot.position);

        for (int i = 1; i < worldPoints.Count; i++)
        {
            Vector3 startPos = worldPoints[i - 1];
            Vector3 endPos = worldPoints[i];
            float dist = Vector3.Distance(startPos, endPos);
            float duration = dist / speed;

            lineRenderer.positionCount = i + 1;
            lineRenderer.SetPosition(i, startPos); // Start at previous point

            // Tween the last point position
            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
                lineRenderer.SetPosition(i, currentPos);
                yield return null;
            }
            lineRenderer.SetPosition(i, endPos);
        }
        
        // Slight pause at full extension?
        yield return new WaitForSeconds(0.1f);

        // 3. Animate Retraction (Bringing berries back)
        // For visual simplicity, we just retract the line point by point from end to start.
        // The Frog.cs handles the logic of "removing" items, but visually we might want to see them slide.
        // For this prototype, let's just retract the tongue. Items disappearing is handled by Frog logic 
        // immediately or we can trigger callbacks here.
        
        // Actually, if we want to "drag" items, we would need to pass that info. 
        // But Frog.cs calls HandleEating AFTER retraction in the previous code. 
        // Ideally, we move the item WITH the tongue tip.
        
        // Retraction:
        for (int i = worldPoints.Count - 1; i > 0; i--)
        {
            Vector3 startPos = worldPoints[i];
            Vector3 endPos = worldPoints[i - 1]; // Move back to previous knot
            float dist = Vector3.Distance(startPos, endPos);
            float duration = dist / speed;

            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
                lineRenderer.SetPosition(i, currentPos);
                yield return null;
            }
            
            // Pop the point
            lineRenderer.positionCount = i;
        }
        
        // Fully retracted
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, tongueRoot.position);

        OnTongueRetracted?.Invoke();
    }
}
