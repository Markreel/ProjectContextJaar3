using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [HideInInspector] public Camera ActiveCamera;

    [SerializeField] private Camera cam1;
    [SerializeField] private Camera cam2;

    private void Awake()
    {
        Instance = Instance ?? this;
        ActiveCamera = cam1;
    }

    public void ChangeCamera()
    {
        cam2.gameObject.SetActive(true);
        ActiveCamera = cam2;
        cam1.gameObject.SetActive(false);
    }
}
