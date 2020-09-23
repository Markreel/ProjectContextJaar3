using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Spawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float delayPerProjectile = 3;
    [SerializeField] private float travelDuration = 1f;
    [SerializeField] private float decisionDuration = 3f;

    [SerializeField] private AnimationCurve travelCurve;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform endPoint;

    [SerializeField] private Transform wayPointLiked;
    [SerializeField] private Transform wayPointNotLiked;

    [SerializeField] private MP_Blueprint[] blueprints;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(delayPerProjectile);

        Transform _p1 = spawnPoints[Random.Range(0, spawnPoints.Length)];
        MP_Blueprint _bp = blueprints[Random.Range(0, blueprints.Length)];

        GameObject _obj = Instantiate(projectilePrefab, _p1.position, _p1.rotation, transform);
        _obj.GetComponent<MediaProjectile>().SetData(_bp.Material, _bp.Text);

        //float _time = 0f;
        //while (_time < 1f)
        //{
        //    _time += Time.deltaTime / travelDuration;
        //    float _evaluatedTime = travelCurve.Evaluate(_time);

        //    _obj.transform.position = Vector3.Lerp(_p1.position, endPoint.position, _evaluatedTime);
        //    yield return null;
        //}

        //_time = 0f;
        //while (_time < 1f)
        //{
        //    _time += Time.deltaTime / decisionDuration;

        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Debug.Log("LIKED");
        //        Destroy(_obj);
        //        RestartSpawnCycle();
        //    }

        //    //Visualiseer hoe lang je nog hebt met text
        //    yield return null;
        //}

        //Destroy(_obj);

        StartCoroutine(Start());
        yield return null;
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
    public Material Material;
    public string Text;
}

