using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public enum SoundType
    {
        Jump,
        BigShoot,
        Shoot,
        Music_Menu,
        Music_Battle1, //calm
        Music_Battle2, //mild
        Music_Battle3, //fun
        Music_Battle4, //energetic
        Music_BossBattle, //boss
        Dash,

        EnemyShoot,
        BombExplode,

        Damage,

        SwingSword,
        Reflect,
        ShieldHit,
        Music_Overworld,
        Button_press, //
        Boulder_moving,
        Player_glide,
        enemy_chase,
        enemy_out_of_breathe,
        enemy_scared
        // Add more sound types as needed
    }

    [System.Serializable]
    public class Sound
    {
        public SoundType Type;
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [HideInInspector]
        public AudioSource Source;
    }

    //Singleton
    public static AudioManager Instance;

    //All sounds and their associated type - Set these in the inspector
    public Sound[] AllSounds;

    //Runtime collection
    private Dictionary<SoundType, Sound> _soundDictionary = new Dictionary<SoundType, Sound>();
    private AudioSource _musicSource;

    private void Awake()
    {
        //Assign singleton
        Instance = this;

        //Set up sounds
        foreach(var s in AllSounds)
        {
            _soundDictionary[s.Type] = s;
        }
    }



    //Call this method to play a sound
    public void Play(SoundType type)
    {
        //Make sure there's a sound assigned to your specified type
        if (!_soundDictionary.TryGetValue(type, out Sound s))
        {
            Debug.LogWarning($"Sound type {type} not found!");
            return;
        }

        //Creates a new sound object
        var soundObj = new GameObject($"Sound_{type}");
        var audioSrc = soundObj.AddComponent<AudioSource>();

        //Assigns your sound properties
        audioSrc.clip = s.Clip;
        audioSrc.volume = s.Volume;

        //Play the sound
        audioSrc.Play();

        //Destroy the object
        Destroy(soundObj, s.Clip.length);
    }

    //Call this method to change music tracks
    public void ChangeMusic(SoundType type)
    {
        if (!_soundDictionary.TryGetValue(type, out Sound track))
        {
            Debug.LogWarning($"Music track {type} not found!");
            return;
        }

        if (_musicSource == null)
        {
            var container = new GameObject("SoundTrackObj");
            _musicSource = container.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }

        _musicSource.clip = track.Clip;
        _musicSource.volume = track.Volume; //<-oops, forgetting to add this line prevents the audio from being changed by sliding volume
        _musicSource.Play();
    }

    //Custom: Call this to pick a random battle music from the 4
    public void PlayRandomBattleMusic()
    {
        //array of battle music
        SoundType[] battleTracks = new SoundType[]
        {
            SoundType.Music_Battle1,
            SoundType.Music_Battle2,
            SoundType.Music_Battle3,
            SoundType.Music_Battle4
        };

        //generate a random index from 0 to array length
        int randomIndex = Random.Range(0, battleTracks.Length);

        //play the randomly selected track
        ChangeMusic(battleTracks[randomIndex]);
    }
}