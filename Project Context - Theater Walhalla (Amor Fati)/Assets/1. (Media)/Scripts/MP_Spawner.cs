using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Spawner : MonoBehaviour
{
    [SerializeField] public static MP_Spawner Instance;

    [SerializeField] public float SpeedMultiplier = 1;

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float delayPerProjectile = 3;
    [SerializeField] private float travelDuration = 1f;
    [SerializeField] private float decisionDuration = 3f;


    [SerializeField] private AnimationCurve travelCurve;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform endPoint;

    [SerializeField] private TelevisionControl televisionControl;
    [Space]
    [SerializeField] private Material[] materialsBlm;
    [SerializeField] private Material[] materialsCorona;
    [SerializeField] private Material[] materialsPrivacy;
    [SerializeField] private Material[] materialsZwartePiet;

    private List<MP_Blueprint> blueprints = new List<MP_Blueprint>();
    private int projectileLimit;
    private int projectileCounter = 0;
    private int projectilesDestroyed = 0;

    private List<MediaProjectile> activeMediaProjectiles = new List<MediaProjectile>();

    private IFocusable previousFocusable;

    private void Awake()
    {
        Instance = this;

        PopulateBlueprintList();
    }

    private void Update()
    {
        RaycastHit _hit;
        Ray _ray = CameraController.Instance.ActiveCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out _hit))
        {
            Transform _objectHit = _hit.transform;
            MediaProjectile _mp = _objectHit.GetComponent<MediaProjectile>();
            IFocusable _focusable = _objectHit.GetComponent<IFocusable>();

            if(_focusable != null)
            {
                previousFocusable?.Unfocus(_hit.point);
                _focusable.Focus(_hit.point);
                previousFocusable = _focusable;
            }
            else
            {
                previousFocusable?.Unfocus(_hit.point);
            }

            if (_mp != null)
            {
                Debugger.Instance.DisplayText(_mp.gameObject.name);

                if (Input.GetMouseButtonDown(0))
                {
                    RemoveMediaProjectile(_mp, true);
                }
            }
            else
            {
                Debugger.Instance.DisplayText("");
            }
        }
        else
        {
            previousFocusable?.Unfocus(_hit.point);
        }
    }

    private void FixedUpdate()
    {
        SpeedMultiplier = Mathf.Clamp(SpeedMultiplier + 0.005f, 1, 5);
        //Debug.Log(SpeedMultiplier);
    }

    private void PopulateBlueprintList()
    {
        //Add BLM blueprints
        foreach (var _mat in materialsBlm)
        {
            blueprints.Add(new MP_Blueprint(_mat, MediaTopic.BLM));
        }

        //Add corona blueprints
        foreach (var _mat in materialsCorona)
        {
            blueprints.Add(new MP_Blueprint(_mat, MediaTopic.Corona));
        }

        //Add privacy blueprints
        foreach (var _mat in materialsPrivacy)
        {
            blueprints.Add(new MP_Blueprint(_mat, MediaTopic.Privacy));
        }

        //Add zwarte piet blueprints
        foreach (var _mat in materialsZwartePiet)
        {
            blueprints.Add(new MP_Blueprint(_mat, MediaTopic.ZwartePiet));
        }

        projectileLimit = blueprints.Count;
    }

    private IEnumerator Start()
    {
        if(blueprints.Count == 0) { yield break; }

        yield return new WaitForSeconds(delayPerProjectile / SpeedMultiplier);

        Transform _p1 = spawnPoints[Random.Range(0, spawnPoints.Length)];

        //Take a random blueprint from the list and remove it from the list afterwards
        int _randomBPIndex = Random.Range(0, blueprints.Count);
        MP_Blueprint _bp = blueprints[_randomBPIndex];
        blueprints.RemoveAt(_randomBPIndex);

        Vector3 _endPoint = endPoint.position + new Vector3(Random.Range(-3.5f, 3.5f), Random.Range(-1.5f, 1.5f), activeMediaProjectiles.Count * 0.01f + Random.Range(0, 0.005f));

        GameObject _obj = Instantiate(projectilePrefab, _p1.position, _p1.rotation, transform);
        activeMediaProjectiles.Add(_obj.GetComponent<MediaProjectile>());
        _obj.GetComponent<MediaProjectile>().SetData(_bp.Material, _bp.MediaTopic);

        projectileCounter++;
        if(projectileCounter < projectileLimit) { StartCoroutine(Start()); }
        

        float _time = 0f;
        while (_time < 1f)
        {
            _time += Time.deltaTime / travelDuration;
            float _evaluatedTime = travelCurve.Evaluate(_time);

            if(_obj != null) { _obj.transform.position = Vector3.Lerp(_p1.position, _endPoint, _evaluatedTime); }
            
            yield return null;
        }

        _obj.GetComponent<Collider>().enabled = true;

        _time = 0f;
        while (_time < 1f)
        {
            _time += Time.deltaTime / decisionDuration;

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Debug.Log("LIKED");
            //    RemoveMediaProjectile(_obj.GetComponent<MediaProjectile>(), true);
            //}

            //Visualiseer hoe lang je nog hebt met text
            yield return null;
        }

        if (_obj != null) { RemoveMediaProjectile(_obj.GetComponent<MediaProjectile>(), false); }

        //StartCoroutine(Start());
        yield return null;
    }

    private void RemoveMediaProjectile(MediaProjectile _mp, bool _liked)
    {
        if (activeMediaProjectiles.Contains(_mp))
        {
            activeMediaProjectiles.Remove(_mp);
            projectilesDestroyed++;

            MediaItem _mi = new MediaItem(_mp.Material, _liked, _mp.MediaTopic);
            DataManager.Instance.MediaData.AddMediaItem(_mi);
            Debug.Log("liked = " + _liked);

            IFocusable _focus = _mp.GetComponent<IFocusable>();
            if(previousFocusable == _focus) { previousFocusable = null; }
        }

        Destroy(_mp.gameObject);

        //End "game"
        if (projectilesDestroyed >= projectileLimit)
        {
            CameraController.Instance.ChangeCamera();
            televisionControl.SpawnTelevisions();
        }
    }

    public void RestartSpawnCycle()
    {
        StopAllCoroutines();
        StartCoroutine(Start());
    }
}

[System.Serializable]
public class MP_Blueprint
{
    public MP_Blueprint(Material _material, MediaTopic _mediaTopic)
    {
        Material = _material;
        MediaTopic = _mediaTopic;
    }

    public Material Material;
    public MediaTopic MediaTopic;
}

