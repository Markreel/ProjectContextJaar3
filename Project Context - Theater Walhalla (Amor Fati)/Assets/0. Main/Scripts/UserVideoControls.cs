using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserVideoControls : MonoBehaviour
{
    [SerializeField] private Image pauseButtonImage;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;

    private bool isPaused = false;
    private float mouseMoveTime = 0;

    private void Update()
    {
        
    }

    public void Pause()
    {
        isPaused = OnlineVideoManager.Instance.PauseVideo();
        pauseButtonImage.sprite = isPaused ? playSprite : pauseSprite;
    }
}
