using System.Collections.Generic;
using UnityEngine;

public enum SoundTypes
{
    Walking,
    Roar,
    ClickerScream
}

public class SoundSourceEntity : IEntity
{
    readonly Dictionary<SoundTypes, AudioSource> _sounds;

    public SoundSourceEntity(Dictionary<SoundTypes, AudioSource> sounds)
    {
        _sounds = sounds;
    }
    public void PlaySound(SoundTypes type)
    {
        _sounds[type].Play();
    }

    public void StopSound(SoundTypes type)
    {
        _sounds[type].Stop();
    }
}