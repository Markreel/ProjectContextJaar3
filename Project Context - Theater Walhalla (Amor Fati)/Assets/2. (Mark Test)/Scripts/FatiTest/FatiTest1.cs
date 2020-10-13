using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatiTest1 : MonoBehaviour
{
    [Header("General Settings: ")]
    [SerializeField] private Transform parent;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float cubeAmount;
    [SerializeField] private bool loop;

    [Header("Size: ")]
    [SerializeField] private Vector3 minSize;
    [SerializeField] private Vector3 maxSize;

    [Header("Offset: ")]
    [SerializeField] private Vector3 minOffset;
    [SerializeField] private Vector3 maxOffset;

    [Header("Rotation: ")]
    [SerializeField] private bool useRandomRotation = false;

    [Header("Animation: ")]
    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private AnimationCurve animationCurve;


    private Coroutine animateCubeTransitionsRoutine;
    private List<CubeSegment> cubeSegments = new List<CubeSegment>();

    private void PopulateCubeList()
    {
        for (int i = 0; i < cubeAmount; i++)
        {
            cubeSegments.Add(new CubeSegment(Instantiate(cubePrefab, parent)));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) { UpdateCubeBody(); }
    }

    private void UpdateCubeBody()
    {
        if(cubeSegments.Count != cubeAmount)
        {
            //Simply destroy all current cubes
            foreach (var _cubeSegment in cubeSegments)
            {
                Destroy(_cubeSegment.Cube);
            }
            cubeSegments.Clear();

            //Spawn new cubes
            PopulateCubeList();
        }

        foreach (var _cubeSegment in cubeSegments)
        {
            _cubeSegment.SetPreviousTransform(_cubeSegment.Cube.transform);

            _cubeSegment.NextPosition = Vector3Extension.Vector3Random(minOffset, maxOffset);
            _cubeSegment.NextScale = Vector3Extension.Vector3Random(minSize, maxSize);

            if (useRandomRotation) { _cubeSegment.NextEulerAngles = Vector3Extension.Vector3Random(Vector3.zero, Vector3.one * 360f); }
            else { _cubeSegment.NextEulerAngles = Vector3.zero; }
        }

        AnimateCubeTransitions();
    }

    private void AnimateCubeTransitions()
    {
        if(animateCubeTransitionsRoutine != null) { StopCoroutine(IEAnimateCubeTransitions()); }
        animateCubeTransitionsRoutine = StartCoroutine(IEAnimateCubeTransitions());
    }

    private IEnumerator IEAnimateCubeTransitions()
    {
        float _tick = 0f;

        while (_tick < 1f)
        {
            _tick += Time.deltaTime / animationDuration;
            float _evaluatedTick = animationCurve.Evaluate(_tick);

            Debug.Log(_tick);

            foreach (var _cubeSegment in cubeSegments)
            {
                _cubeSegment.Cube.transform.localPosition =
                    Vector3.Lerp(_cubeSegment.PrevPosition, _cubeSegment.NextPosition, _evaluatedTick);

                _cubeSegment.Cube.transform.localScale =
                    Vector3.Lerp(_cubeSegment.PrevScale, _cubeSegment.NextScale, _evaluatedTick);

                _cubeSegment.Cube.transform.localRotation =
                    Quaternion.Lerp(Quaternion.Euler(_cubeSegment.PrevEulerAngles), Quaternion.Euler(_cubeSegment.NextEulerAngles), _evaluatedTick);
            }

            yield return null;
        }

        if (loop) { UpdateCubeBody(); }
        yield return null;
    }
}

[System.Serializable]
public class CubeSegment
{
    public CubeSegment(GameObject _cube)
    {
        Cube = _cube;
    }

    public void SetPreviousTransform(Transform _transform)
    {
        PrevPosition = _transform.localPosition;
        PrevScale = _transform.localScale;
        PrevEulerAngles = _transform.localEulerAngles;
    }

    public void SetNextTransform(Transform _transform)
    {
        NextPosition = _transform.localPosition;
        NextScale = _transform.localScale;
        NextEulerAngles = _transform.localEulerAngles;
    }

    public GameObject Cube;

    public Vector3 PrevPosition;
    public Vector3 NextPosition;

    public Vector3 PrevScale;
    public Vector3 NextScale;

    public Vector3 PrevEulerAngles;
    public Vector3 NextEulerAngles;
}
