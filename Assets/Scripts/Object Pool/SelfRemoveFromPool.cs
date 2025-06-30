using System.Collections;
using UnityEngine;

public class SelfRemoveFromPool : MonoBehaviour
{
    [SerializeField] private float removeDelay = 1;

    private ObjectPoolManager objPool;
    private ParticleSystem particle;

    void Awake()
    {
        objPool = ObjectPoolManager.instance;
        particle = GetComponentInChildren<ParticleSystem>();
    }

    void OnEnable()
    {
        if (particle != null)
        {
            particle.Clear();
            particle.Play();
        }

        StartCoroutine(RemoveWithDelayCo());
    }

    private IEnumerator RemoveWithDelayCo()
    {
        yield return new WaitForSeconds(removeDelay);
        objPool.Remove(gameObject);
    }
}

