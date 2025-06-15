using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("BGM Details")]
    [SerializeField] private bool isBGMPlay;
    [SerializeField] private AudioSource[] bgms;
    private int currentBGMIndex;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 2);
    }

    private void PlayMusicIfNeeded()
    {
        if (bgms.Length <= 0)
        {
            Debug.LogWarning("No music was assigned. Check AudioManager");
            return;
        }

        if (isBGMPlay == false)
            return;

        if (bgms[currentBGMIndex].isPlaying == false)
            PlayRandomBGM();
    }

    [ContextMenu("Play Random Music")]
    public void PlayRandomBGM()
    {
        currentBGMIndex = Random.Range(0, bgms.Length);
        PlayBGM(currentBGMIndex);
    }

    public void PlayBGM(int bgmToPlay)
    {
        if (bgms.Length <= 0)
        {
            Debug.LogWarning("No music was assigned. Check AudioManager");
            return;
        }

        StopAllBGM();
        currentBGMIndex = bgmToPlay;
        bgms[bgmToPlay].Play();
    }

    [ContextMenu("Stop All Music")]
    public void StopAllBGM()
    {
        for (int i = 0; i < bgms.Length; i++)
        {
            bgms[i].Stop();
        }
    }

    public void PlaySFX()
    {
        Debug.Log("Sound was played");
    }
}
