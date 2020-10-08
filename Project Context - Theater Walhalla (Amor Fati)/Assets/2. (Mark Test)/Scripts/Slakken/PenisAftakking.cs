using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class PenisAftakking : MonoBehaviour
{
    [SerializeField] private float lifeTime = 3f;
    [Space]
    [SerializeField] private float minDelayPerNewDirection = 0.1f;
    [SerializeField] private float maxDelayPerNewDirection = 2f;
    [Space]
    [SerializeField] private float movementSpeed = 10f;
    [Space]
    [SerializeField] private GameObject aftakkingPrefab;

    private Vector3 direction;
    private Vector3 previousDirection = Vector3.zero;
    private TrailRenderer trailRenderer;

    private float delayPerNewDirection;
    private float startLifeTime;
    private float startTrailWidth;

    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        startTrailWidth = trailRenderer.widthMultiplier;
        Debug.Log(startTrailWidth);
        startLifeTime = lifeTime;
    }

    private IEnumerator Start()
    {
        SetRandomDirection();

        delayPerNewDirection = Random.Range(minDelayPerNewDirection, maxDelayPerNewDirection);
        float _startDelay = delayPerNewDirection;
        while (delayPerNewDirection > 0f)
        {
            delayPerNewDirection -= Time.deltaTime;

            previousDirection = Vector3.Lerp(previousDirection, direction, _startDelay + -delayPerNewDirection);

            transform.localPosition = Vector3.Lerp(transform.localPosition, 
                transform.localPosition + previousDirection * movementSpeed / 100f, _startDelay + -delayPerNewDirection);

            yield return null;
        }

        if (lifeTime > 0)
        {
            SpawnAftakking();
            StartCoroutine(Start());
        }

        yield return null;
    }

    private void FixedUpdate()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            trailRenderer.widthMultiplier = startTrailWidth / startLifeTime * lifeTime;

            //previousDirection = Vector3.Lerp(previousDirection, direction, startLifeTime + -lifeTime);
            //Debug.Log(GetInstanceID().ToString() + previousDirection);

            //transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + previousDirection * movementSpeed / 100f, startLifeTime + -lifeTime);
            //transform.LookAt(transform.localPosition + direction);
        }

        else
        {
            trailRenderer.widthMultiplier = 0;
            Invoke("DestroySelf", startLifeTime);
        }

    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    private void SetRandomDirection()
    {
        if (previousDirection == Vector3.zero) { previousDirection = direction; }
        direction = (Random.Range(0, 2) == 0 ? Vector3.left : Vector3.right) + (Vector3.down);
    }

    private void SpawnAftakking()
    {
        if (aftakkingPrefab == null) { return; }
        Instantiate(aftakkingPrefab, transform.position, aftakkingPrefab.transform.rotation, transform.parent);
    }
}
