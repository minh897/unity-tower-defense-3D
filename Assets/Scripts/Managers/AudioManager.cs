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

    public void PlaySFX(AudioSource audioToPlay, bool isPitchRandom = false)
    {
        if (audioToPlay == null)
        {
            Debug.Log("Could not play " + audioToPlay.gameObject.name + ". There is no audio clip assigned");
            return;
        }
        
        // Safety check to prevent the audio from playing on top of each other
        if (audioToPlay.isPlaying)
            audioToPlay.Stop();

        audioToPlay.pitch = isPitchRandom ? Random.Range(.9f, 1.1f) : 1;
        audioToPlay.Play();
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

}
