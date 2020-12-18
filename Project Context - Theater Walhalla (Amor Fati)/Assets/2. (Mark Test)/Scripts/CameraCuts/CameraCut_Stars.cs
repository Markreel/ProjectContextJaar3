using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCut_Stars : MonoBehaviour
{
    [SerializeField] private GameObject video;
    [Space]
    [SerializeField] private float initialDelay = 3f;
    [Space]
    [SerializeField] private LerpBaseClass[] phase1Lerps;
    [SerializeField] private float phase1LerpDuration = 20f;
    [Space]
    [SerializeField] private LerpBaseClass[] phase2Lerps;
    [SerializeField] private float phase2LerpDuration = 3f;
    [Space]
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 camPosA;
    [SerializeField] private Vector3 camPosB;
    [SerializeField] private float camIntroDuration = 5f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(IESequence());
        }
    }

    private IEnumerator IESequence()
    {
        yield return new WaitForSeconds(initialDelay);
        float _tick = 0;

        //Phase 1
        while (_tick < 1f)
        {
            _tick += Time.deltaTime / phase1LerpDuration;

            foreach (var _phase1Lerp in phase1Lerps)
            {
                _phase1Lerp.Lerp(_tick);
            }

            yield return null;
        }

        //Phase 2
        _tick = 0;
        while (_tick < 1f)
        {
            _tick += Time.deltaTime / phase2LerpDuration;

            foreach (var _phase2Lerp in phase2Lerps)
            {
                _phase2Lerp.Lerp(_tick);
            }

            yield return null;
        }

        yield return new WaitForSeconds(3f);

        //Phase 3
        _tick = 1f;
        while (_tick > 0f)
        {
            _tick -= Time.deltaTime / phase2LerpDuration;

            foreach (var _phase2Lerp in phase2Lerps)
            {
                _phase2Lerp.Lerp(_tick);
            }

            yield return null;
        }

        //_tick = 0;
        //while (_tick < 1f)
        //{
        //    _tick += Time.deltaTime / wallRemoveDuration;

        //    wall.LerpPos(_tick);
        //    cam.transform.position = Vector3.Lerp(camPosA, camPosB, _tick);
        //    yield return null;
        //}

        yield return null;
    }
}
