using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Scriptable Objects/Sound Data",fileName = "Sound Data")]
public class Sound : ScriptableObject
{
    public SoundType soundType;
    public AudioClip clip;
    
    [Range(-3f, 3f)]
    public float pitch = 1;

    [Range(0f,1f)]
    public float volume = 0.5f;
    public bool loop = false;
}
