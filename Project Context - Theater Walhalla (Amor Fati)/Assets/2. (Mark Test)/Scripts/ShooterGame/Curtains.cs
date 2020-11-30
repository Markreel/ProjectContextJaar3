using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Curtains : MonoBehaviour
{
    [SerializeField] private Image leftCurtain;
    [SerializeField] private float leftOpenedX;
    [SerializeField] private float leftClosedX;

    [SerializeField] private Image rightCurtain;
    [SerializeField] private float rightOpenedX;
    [SerializeField] private float rightClosedX;

    [SerializeField] private float defaultDuration;

    [SerializeField] private AudioClip audioOnOpen;
    [SerializeField] private AudioClip audioOnClose;

    private Coroutine animateCurtainsRoutine;

    public void Open(float _duration = -1)
    {
        if (animateCurtainsRoutine != null) { StopCoroutine(animateCurtainsRoutine); }
        animateCurtainsRoutine = StartCoroutine(IEAnimateCurtains(true));
    }

    public void Close(float _duration = -1)
    {
        if (animateCurtainsRoutine != null) { StopCoroutine(animateCurtainsRoutine); }
        animateCurtainsRoutine = StartCoroutine(IEAnimateCurtains(false));
    }

    private IEnumerator IEAnimateCurtains(bool _open, float _duration = -1)
    {
        if (_duration < 0) { _duration = defaultDuration; }

        //play audio on loop until animation is over

        float _leftStartPos = _open ? leftClosedX : leftOpenedX;
        float _leftEndPos = _open ? leftOpenedX : leftClosedX;

        float _rightStartPos = _open ? rightClosedX : rightOpenedX;
        float _rightEndPos = _open ? rightOpenedX : rightClosedX;

        float _key = 0;
        while (_key < 1f)
        {
            _key += Time.deltaTime / _duration;

            //leftCurtain.transform.position = Vector3.Lerp();
            //rightCurtain.transform.position = Vector3.Lerp();

            yield return null;
        }

        yield return null;
    }
}
