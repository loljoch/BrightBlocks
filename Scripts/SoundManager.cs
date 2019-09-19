using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    [SerializeField] private AudioClip moveShapeAudio;
    [SerializeField] private AudioClip setBlockAudio;
    [SerializeField] private AudioClip clearLineAudio;

    private List<AudioSource> audioSources = new List<AudioSource>();

    public SoundManager(AudioClip _moveShapeAudio, AudioClip _setBlockAudio, AudioClip _clearLineAudio)
    {
        moveShapeAudio = _moveShapeAudio;
        setBlockAudio = _setBlockAudio;
        clearLineAudio = _clearLineAudio;
    }
    
    public void PlayMoveShapeClip()
    {
        PlayAudioClip(moveShapeAudio);
    }

    public void PlaySetBlockClip()
    {
        PlayAudioClip(setBlockAudio);
    }

    public void PlayClearLineClip()
    {
        PlayAudioClip(clearLineAudio);
    }

    private void PlayAudioClip(AudioClip _clip, float _volume = 1)
    {
        GetAudioSource().PlayOneShot(_clip, _volume);
    }

    private AudioSource CreateAudiosource()
    {
        GameObject _gameObject = new GameObject();
        AudioSource _audiosource = _gameObject.AddComponent<AudioSource>();
        audioSources.Add(_audiosource);
        return _audiosource;
    }

    private AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }

        return CreateAudiosource();
    }
}
