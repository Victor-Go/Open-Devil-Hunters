using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Src;
using UnityEngine;
using UnityEngine.Audio;

namespace Code.Scripts.Src.Utils
{
  public class AudioWrapper
  {
    private static readonly ObjectPool objectPool = ObjectPool.Instance;

    private readonly Dictionary<string, AudioClip> clips = new();
    private readonly AudioSource audioSource;

    private static readonly AudioMixerGroup sfxMixerGroup =
      Resources.Load<AudioMixer>("Audio/AudioMixer").FindMatchingGroups("SFX").First();

    public AudioWrapper(string[] clipNames, Transform parent)
    {
      if (clipNames == null || clipNames.Length == 0) return;

      foreach (var clipName in clipNames)
      {
        var clip = Resources.Load<AudioClip>(clipName);

        if (clip != null)
        {
          clips[clipName] = clip;
        }
        else
        {
          Debug.LogErrorFormat("Unable to find clip {0}.", clipName);
        }
      }

      var gameObject = new GameObject("AudioWrapper");
      gameObject.transform.SetParent(parent);
      audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
      audioSource.outputAudioMixerGroup = sfxMixerGroup;
    }

    public void Play(string clipName, Vector2 position)
    {
      PlayClip(clips[clipName], position);
    }

    public void PlayRandomly()
    {
      if (clips == null || clips.Count == 0) return;

      var clip = clips.ElementAt(Random.Range(0, clips.Count)).Value;
      audioSource.clip = clip;
      audioSource.Play();
    }

    public static void PlayClip(AudioClip clip, Vector2 position)
    {
      var tempGameObject = objectPool.GetObject("Audio/TempAudioSource");
      tempGameObject.transform.position = position;
      var tempAudioSource = tempGameObject.GetComponent<AudioSource>();
      tempAudioSource.clip = clip;
      tempAudioSource.Play();
      objectPool.Recycle(tempGameObject, clip.length + 5);
    }
  }
}