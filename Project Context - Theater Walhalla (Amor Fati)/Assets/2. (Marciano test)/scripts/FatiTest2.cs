using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatiTest2 : MonoBehaviour
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

    //marciano toevoeging audio script
    [SerializeField] private AudioVisualizerScript audioVisualizer;
    [SerializeField] private float localSpectrum;
    [SerializeField] private int spectrumoffset;
    [SerializeField] private float spectrumMultiplyer;
    [SerializeField] private float spectrumMinimumSize;
    [SerializeField] private float sectrumMax;
    [SerializeField] private float spectrumSpeed;
    [SerializeField] public float spectrumTarget;


    private void Start()
    {
        audioVisualizer = GameObject.Find("AudioVisualizer").GetComponent<AudioVisualizerScript>();
    }

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

        localSpectrum = audioVisualizer.spectrum[spectrumoffset];
        spectrumTarget = Mathf.Lerp(spectrumTarget, Mathf.Clamp((localSpectrum * spectrumMultiplyer + spectrumMinimumSize),0,sectrumMax), spectrumSpeed*Time.deltaTime);
        
        animationDuration = 1 - spectrumTarget;
        
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

                _cubeSegment.Cube.transform.localEulerAngles =
                    Vector3.Lerp(_cubeSegment.PrevEulerAngles, _cubeSegment.NextEulerAngles, _evaluatedTick);
            }

            yield return null;
        }

        if (loop) { UpdateCubeBody(); }
        yield return null;
    }
}

