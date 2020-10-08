using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCutTest : MonoBehaviour
{
    [SerializeField] private GameObject bubble;
    [SerializeField] private GameObject video;

    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 camPosA;
    [SerializeField] private Vector3 camPosB;
    [SerializeField] private float camIntroDuration = 5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(IELerpCam());
        }
    }

    private IEnumerator IELerpCam()
    {

        float _tick = 0;
        while (_tick < 1f)
        {
            _tick += Time.deltaTime / camIntroDuration;

            cam.transform.position = Vector3.Lerp(camPosA, camPosB, _tick);
            yield return null;
        }

        bubble.SetActive(false);
        video.SetActive(true);

        yield return null;
    }
}
