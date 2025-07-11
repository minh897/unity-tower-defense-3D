using UnityEngine;

public class EnemySpiderVisual : EnemyVisual
{
    [Header("Leg Details")]
    public float legSpeed = 3;
    public float increaseLegSpeed = 10;
    private SpiderLeg[] legs;

    [Header("Body Animation")]
    [SerializeField] private float bodyAnimSpeed = 1;
    [SerializeField] private float maxHeight = .1f;
    [SerializeField] private Transform bodyTransform;

    [Header("Smoke VFX")]
    [SerializeField] private float smokeCooldown;
    [SerializeField] private ParticleSystem[] smokeVFXs;
    private float smokeTimer;

    private Vector3 startPosition;
    private float elaspedTime;

    protected override void Awake()
    {
        base.Awake();
        legs = GetComponentsInChildren<SpiderLeg>();
    }

    protected override void Start()
    {
        base.Start();

        startPosition = bodyTransform.localPosition;
    }

    protected override void Update()
    {
        base.Update();

        AnimateBody();
        ActivateSmokeVFX();
        UpdateSpiderLegs();
    }

    public void BrieflySpeedUpLegs()
    {
        foreach (var leg in legs)
        {
            leg.SpeedUpLeg();
        }
    }

    private void ActivateSmokeVFX()
    {
        smokeTimer -= Time.deltaTime;

        if (smokeTimer < 0)
        {
            smokeTimer = smokeCooldown;
            foreach (var smoke in smokeVFXs)
            {
                smoke.Play();
            }
        }
    }

    private void AnimateBody()
    {
        elaspedTime += Time.deltaTime * bodyAnimSpeed;

        // Plus 1 to prevent the body goes below the ground (from 0 to 2)
        // Divide by 2 so it goes from 0 and 1
        float sinValue = (Mathf.Sin(elaspedTime) + 1) / 2;
        float newY = Mathf.Lerp(0, maxHeight, sinValue);

        bodyTransform.localPosition = startPosition + new Vector3(0, newY, 0);
    }

    private void UpdateSpiderLegs()
    {
        foreach (var leg in legs)
        {
            leg.UpdateLeg();
        }
    }
}
