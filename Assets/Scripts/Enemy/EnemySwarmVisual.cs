using UnityEngine;

public class EnemySwarmVisual : EnemyVisual
{
    [Header("Visual variants")]
    [SerializeField] private GameObject[] variants;

    [Header("Bounce Settings")]
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float minHeight = .1f;
    [SerializeField] private float maxHeight = .3f;
    [SerializeField] private AnimationCurve bounceCurve;
    private float bounceTimer;


    protected override void Start()
    {
        base.Start();
        ChooseVisualVariant();
    }

    protected override void Update()
    {
        base.Update();
        BounceEffect();
    }

    private void BounceEffect()
    {
        bounceTimer += bounceSpeed * Time.deltaTime;

        float bounceValue = bounceCurve.Evaluate(bounceTimer % 1);
        float bounceHeight = Mathf.Lerp(minHeight, maxHeight, bounceValue);

        visuals.localPosition = new Vector3(visuals.localPosition.x, bounceHeight, visuals.localPosition.z);
    }

    private void ChooseVisualVariant()
    {
        foreach (var option in variants)
        {
            option.SetActive(false);
        }

        int randomIndex = Random.Range(0, variants.Length);
        GameObject newVisuals = variants[randomIndex];

        newVisuals.SetActive(true);
        visuals = newVisuals.transform;
    }
}
