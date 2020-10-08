using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenisBehaviour : MonoBehaviour
{
    [SerializeField] private bool inverted = false;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [SerializeField] private float movementPower = 1;
    [SerializeField, Range(1, 5)] private float distanceAdjustment = 1;
    [SerializeField] private bool useDistanceAdjustment = true;

    [Space]

    [SerializeField] private bool useAftakking = false;
    [SerializeField] private GameObject aftakkingPrefab;

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0)) { SpawnAftakking(); }
    }

    private void FixedUpdate()
    {
        //HandleKeyboardMovement();
        HandleMouseMovement();

        distanceAdjustment = Mathf.Clamp(distanceAdjustment + Time.deltaTime / 20f, 1, 5);
    }

    private void HandleKeyboardMovement()
    {
        float _hor = Input.GetAxis("Horizontal");

        if (_hor != 0)
        {
            float _calcMovPower = (_hor < 0 ? (inverted? movementPower : -movementPower) : (inverted ? -movementPower : movementPower)) / 100f;
            transform.localPosition = new Vector3(Mathf.Clamp(Mathf.Lerp(transform.localPosition.x, transform.localPosition.x + _calcMovPower,Time.time), minX, maxX), 0, 0);
        }
    }

    private void HandleMouseMovement()
    {
        Vector3 _temp = Input.mousePosition;
        _temp.z = 10f;

        Vector3 _pos = Camera.main.ScreenToWorldPoint(_temp);

        if (inverted) { _pos.x = -_pos.x; }
        _pos.y = 0 - (transform.parent.position.y - _pos.y);

        if(useDistanceAdjustment)
        {
            _pos.x /= distanceAdjustment;
            _pos.y /= distanceAdjustment;
        }

        Vector3 _newPos = new Vector3(_pos.x, _pos.y, 0);
        transform.localPosition = _newPos;// Vector3.Lerp(transform.position, _newPos, Time.time);
    }
    
    private void SpawnAftakking()
    {
        if(aftakkingPrefab == null) { return; }
        Instantiate(aftakkingPrefab, transform.position - Vector3.back, aftakkingPrefab.transform.rotation, transform.parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inverted && useAftakking) { SpawnAftakking(); }
    }
}
