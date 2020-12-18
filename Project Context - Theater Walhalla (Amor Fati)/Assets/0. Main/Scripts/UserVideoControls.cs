using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserVideoControls : MonoBehaviour
{
    [SerializeField] private float focusTime = 0.5f;

    [SerializeField] private Image pauseButtonImage;
    [SerializeField] private Sprite pauseSprite;
    [SerializeField] private Sprite playSprite;

    [SerializeField] private Slider volumeSlider;

    private bool isPaused = false;
    private float mouseMoveTime = 1;
    private Vector3 lastMousePos = Vector3.zero;

    private void Start()
    {
        //volumeSlider.value = SaveFileManager.SaveFileData.Volume;
        //OnlineVideoManager.Instance.VideoPlayer.GetTargetAudioSource(0).volume = volumeSlider.value;
    }

    private void Update()
    {
        //SaveFileManager.SaveFileData.Volume = volumeSlider.value;
        //OnlineVideoManager.Instance.VideoPlayer.GetTargetAudioSource(0).volume = volumeSlider.value;

        //Show pause icon depending on if the mouse is moving or not
        Vector3 _mouseDelta = Input.mousePosition - lastMousePos;
        if (_mouseDelta != Vector3.zero || isPaused)
        {
            mouseMoveTime = focusTime;
            pauseButtonImage.gameObject.SetActive(true);
        }
        else
        {
            if (mouseMoveTime > 0) { mouseMoveTime -= Time.deltaTime; }
            else { pauseButtonImage.gameObject.SetActive(false); }
        }
        lastMousePos = Input.mousePosition;

    }

    public void TogglePause()
    {
        isPaused = OnlineVideoManager.Instance.PauseVideo();
        pauseButtonImage.sprite = isPaused ? playSprite : pauseSprite;
    }
}
