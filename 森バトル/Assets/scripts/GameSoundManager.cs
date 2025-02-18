using UnityEngine;

public class GameSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource UIsounds;
    public AudioClip[] music;
    public AudioClip[] soundEffects;
    public float musicVolume = 1f;
    public float seVolume = 1f;
    private int previouslyPlayedTrack = -1;
    private bool musicPlaying;
    private float musicStartTime;
    private float musicEndTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("AttemptToPlayNewTrack", 0f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(musicPlaying && musicEndTime <= Time.time)
        {
            musicPlaying = false;
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            UIsounds.pitch = Random.Range(0.9f, 1f);
            UIsounds.clip = soundEffects[Random.Range(11, 13)];
            UIsounds.Play();
        }
    }
    private int PickTrack()
    {
        int newTrack = previouslyPlayedTrack;
        while(newTrack == previouslyPlayedTrack)
        {
            newTrack = Random.Range(0, music.Length - 1);
        }
        return newTrack;
    }
    private void AttemptToPlayNewTrack()
    {
        if(!musicPlaying)
        {
            int rand = Random.Range(1, 8);
            if(rand == 1)
            {
                audioSource.clip = music[PickTrack()];
                audioSource.Play();
                musicPlaying = true;
                musicStartTime = Time.time;
                musicEndTime = musicStartTime + music[PickTrack()].length;
            }
        }
    }
    public void ChangeVolume(float input)
    {
        musicVolume = input;
        audioSource.volume = musicVolume;
    }
    public void ChangeSEVolume(float input)
    {
        seVolume = input;
    }
}
