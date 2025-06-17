using System.Collections.Generic;
using UnityEngine;

public class EnemyStealth : Enemy
{
    [Header("Stealth Enemy Details")]
    [SerializeField] private float hideDuration = .5f;
    [SerializeField] private ParticleSystem smokeScreenFX;
    [SerializeField] private List<Enemy> enemiesToHide;

    protected override void Awake()
    {
        base.Awake();

        InvokeRepeating(nameof(HideItSelf), .1f, hideDuration);
        InvokeRepeating(nameof(HideEnemies), .1f, hideDuration);
    }

    void Update()
    {
        
    }

    public void EnableSmokeScreen(bool isEnable)
    {
        if (isEnable)
        {
            if (smokeScreenFX.isPlaying == false)
                smokeScreenFX.Play();
            else
                smokeScreenFX.Stop();
        }
    }

    private void HideItSelf()
    {
        HideEnemy(hideDuration);
    }

    private void HideEnemies()
    {
        foreach (Enemy enemy in enemiesToHide)
        {
            enemy.HideEnemy(hideDuration);
        }
    }

    public List<Enemy> GetEnemiesToHide() => enemiesToHide;
}
