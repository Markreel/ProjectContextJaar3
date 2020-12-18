using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatiMovement : MonoBehaviour
{
    [SerializeField] AudioVisualizerScript audiovisual;
    [SerializeField] float target;
    [SerializeField] int offset;
    [SerializeField] float multiplyer = 30;
    [SerializeField] float minimumSize = 0.25f;
    [SerializeField] float maxSize = .5f;
    [SerializeField] float speed = 20;

    [SerializeField] float _maxDistance;
    [SerializeField] Vector2 _movementSpeedMinMax;
    Vector3 _movementSpeed;
    [SerializeField] GameObject _center;

    [SerializeField] float audioThreshold;

    float rotationTarget;

    private void Start()
    {
        //audiovisual = GameObject.Find("AudioVisualizer").GetComponent<AudioVisualizerScript>();

        _movementSpeed = new Vector3(Random.Range(.02f, .04f), Random.Range(.02f, .04f), Random.Range(.02f, .04f));
        StartCoroutine(MoveCube());

    }
    private void Update()
    {
        target = Mathf.Lerp(target, Mathf.Clamp((audiovisual.spectrum[offset] * multiplyer + minimumSize), 0, maxSize), speed * Time.deltaTime);

        if (transform.position.x > _center.transform.position.x + _maxDistance)
        {
            _movementSpeed.x = -Random.Range(_movementSpeedMinMax.x, _movementSpeedMinMax.y);
        }
        else if (transform.position.x < _center.transform.position.x - _maxDistance)
        {
            _movementSpeed.x = Random.Range(_movementSpeedMinMax.x, _movementSpeedMinMax.y);
        }


        if (transform.position.y > _center.transform.position.y + _maxDistance)
        {
            _movementSpeed.y = -Random.Range(_movementSpeedMinMax.x, _movementSpeedMinMax.y);
        }
        else if (transform.position.y < _center.transform.position.y - _maxDistance)
        {
            _movementSpeed.y = Random.Range(_movementSpeedMinMax.x, _movementSpeedMinMax.y);
        }


        if (transform.position.z > _center.transform.position.z + _maxDistance)
        {
            _movementSpeed.z = -Random.Range(_movementSpeedMinMax.x, _movementSpeedMinMax.y);
        }
        else if (transform.position.z < _center.transform.position.z - _maxDistance)
        {
            _movementSpeed.z = Random.Range(_movementSpeedMinMax.x, _movementSpeedMinMax.y);
        }

        //transform.localEulerAngles = Quaternion.Euler(transform.localEulerAngles.x, rotationTarget, transform.localEulerAngles.z);

    }

    IEnumerator MoveCube()
    {
        if (target > audioThreshold)
        {
            transform.position += _movementSpeed * target * Time.deltaTime;
        }
        yield return null;

        //yield return new WaitForSeconds(.15f);
        StartCoroutine(MoveCube());
    }

}
