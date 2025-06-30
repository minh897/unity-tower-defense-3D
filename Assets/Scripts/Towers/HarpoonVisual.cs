using System.Collections.Generic;
using UnityEngine;

public class HarpoonVisual : MonoBehaviour
{
    [SerializeField] private int maxLinks = 100;
    [SerializeField] private float linkDistance = .2f;
    [SerializeField] private GameObject linkPrefab;
    [SerializeField] private Transform linkParent;
    [Space]

    [SerializeField] private Transform startPoint; // gun point
    [SerializeField] private Transform endPoint; // harpoon back point
    [Space]

    [SerializeField] private GameObject onElectrifyVFX;
    private GameObject currentVFX;

    private ObjectPoolManager objectPool;
    private List<ProjectileHarpoonLink> links = new();

    void Start()
    {
        InitializeLinks();
        objectPool = ObjectPoolManager.instance;
    }

    void Update()
    {
        if (endPoint == null)
            return;

        ActivateLinksIfNeeded();
    }

    private void InitializeLinks()
    {
        for (int i = 0; i < maxLinks; i++)
        {
            ProjectileHarpoonLink newLink =
                Instantiate(linkPrefab, startPoint.position, Quaternion.identity, linkParent).GetComponent<ProjectileHarpoonLink>();

            links.Add(newLink);
        }
    }

    private void ActivateLinksIfNeeded()
    {
        Vector3 direction = (endPoint.position - startPoint.position).normalized;
        float distance = Vector3.Distance(startPoint.position, endPoint.position);

        int activeLinkAmount = Mathf.Min(maxLinks, Mathf.CeilToInt(distance / linkDistance));

        for (int i = 0; i < links.Count; i++)
        {
            if (i < activeLinkAmount)
            {
                Vector3 newPosition = startPoint.position + direction * linkDistance * (i + 2);
                links[i].EnableLinK(true, newPosition);
            }
            else
                links[i].EnableLinK(false, Vector3.zero);


            if (i != links.Count - 1)
                links[i].UpdateLineRenderer(links[i], links[i + 1]);
        }
    }

    public void EnableChainVisual(bool enable, Transform newEndPoint = null)
    {
        if (enable)
            endPoint = newEndPoint;

        if (enable == false)
        {
            endPoint = startPoint;
            DestroyElectrifyVFX();
        }
    }

    public void CreateElectrifyVFX(Transform targetTransform)
    {
        currentVFX = objectPool.Get(onElectrifyVFX, targetTransform.position, Quaternion.identity, targetTransform);
    }

    private void DestroyElectrifyVFX()
    {
        if (currentVFX != null)
            objectPool.Remove(currentVFX);
    }
}
