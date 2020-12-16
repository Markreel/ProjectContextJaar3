using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGameMusic : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips;
    private AudioSource source;
    private int index; 

    [SerializeField] private AudioClip lossAudioClip; 


    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        index = 0;
    }

    public void ChangeAudio()
    {
        index += 1; 
        source.clip = audioClips[index];
        source.Play(); 


    }

    public void PlayLossMusic()
    {
        source.pitch = 0.3f;
        source.clip = lossAudioClip;
        source.Play(); 
    }







    // Update is called once per frame
    void Update()
    {
        
    }
}
