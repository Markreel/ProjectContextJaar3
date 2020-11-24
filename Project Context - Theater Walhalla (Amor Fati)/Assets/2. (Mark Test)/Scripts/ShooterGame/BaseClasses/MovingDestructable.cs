using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolingAndAudio;

namespace ShooterGame
{
    public class MovingDestructable : BaseDestructable
    {
        [Header("Default Settings: ")]
        [SerializeField] private float activationRange = 5;
        [SerializeField] private float minActivationDelay;
        [SerializeField] private float maxActivationDelay;

        [Header("Horizontal Movement: ")]
        [SerializeField] private bool moveHorizontally;
        [Space]
        [SerializeField] private bool loopHorizontalMovement;
        [SerializeField] private bool destroyAfterHorizontal;
        [SerializeField] private float horizontalStartPos;
        [SerializeField] private bool useObjectPosAsHorizontalStart;
        [SerializeField] private float horizontalEndPos;
        [SerializeField] private AnimationCurve horizontalMovementCurve;
        [SerializeField] private float horizontalMovementDuration;

        [Header("Vertical Movement: ")]
        [SerializeField] private bool moveVertically;
        [Space]
        [SerializeField] private bool loopVerticalMovement;
        [SerializeField] private bool destroyAfterVertical;
        [SerializeField] private float verticalStartPos;
        [SerializeField] private bool useObjectPosAsVerticalStart;
        [SerializeField] private float verticalEndPos;
        [SerializeField] private AnimationCurve verticalMovementCurve;
        [SerializeField] private float verticalMovementDuration;

        private bool isActivated = false;
        private Vector3 defaultPosition;
        private Vector3 newPosition;

        private void Awake()
        {
            defaultPosition = newPosition = transform.position;
            if (useObjectPosAsHorizontalStart) { horizontalStartPos = defaultPosition.x; }
            if (useObjectPosAsVerticalStart) { verticalStartPos = defaultPosition.y; }
        }

        private void Update()
        {
            if (!isActivated) { CheckForActivation(); }
        }

        private void CheckForActivation()
        {
            if (Camera.main.transform.position.x > transform.position.x - activationRange &&
                Camera.main.transform.position.x < transform.position.x + activationRange)
            {             
                isActivated = true;
                Invoke("StartMovement", Random.Range(minActivationDelay, maxActivationDelay));
            }
        }

        private void StartMovement()
        {
            if (moveHorizontally) { StartCoroutine(IEMoveHorizontally()); }
            if (moveVertically) { StartCoroutine(IEMoveVertically()); }
        }

        private IEnumerator IEMoveHorizontally()
        {
            float _tick = 0f;
            while (_tick < 1f)
            {
                //Calculate time and evaluate curve
                _tick += Time.deltaTime / horizontalMovementDuration;
                float _evaluatedTick = horizontalMovementCurve.Evaluate(_tick);

                //Calculate new X
                float _newX = newPosition.x;
                _newX = Mathf.Lerp(horizontalStartPos, horizontalEndPos, _evaluatedTick);

                //Set new X position
                newPosition.x = _newX;
                transform.position = newPosition;
                yield return null;
            }

            if (destroyAfterHorizontal) { SelfDestruct(); }
            if (loopHorizontalMovement && moveHorizontally) { StartCoroutine(IEMoveHorizontally()); }
            yield return null;
        }

        private IEnumerator IEMoveVertically()
        {
            float _tick = 0f;
            while (_tick < 1f)
            {
                //Calculate time and evaluate curve
                _tick += Time.deltaTime / verticalMovementDuration;
                float _evaluatedTick = verticalMovementCurve.Evaluate(_tick);

                //Calculate new Y
                float _newY = newPosition.y;
                _newY = Mathf.Lerp(verticalStartPos, verticalEndPos, _evaluatedTick);

                //Set new Y position
                newPosition.y = _newY;
                transform.position = newPosition;
                yield return null;
            }

            if (destroyAfterVertical) { SelfDestruct(); }
            if (loopVerticalMovement && moveVertically) { StartCoroutine(IEMoveVertically()); }
            yield return null;
        }
    }
}
