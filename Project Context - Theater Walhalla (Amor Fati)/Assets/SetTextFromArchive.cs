using UnityEngine;
using TMPro;

public class SetTextFromArchive : MonoBehaviour
{
    [SerializeField] ArchiveOutput archive;
    TextMeshPro text;
    [SerializeField] float lifetime = 5.0f;

    void Start()
    {
        text = GetComponent<TextMeshPro>();

        archive = GameObject.Find("ArchiveOutput").GetComponent<ArchiveOutput>();

        text.text = archive.textToObject;
        
        Destroy(gameObject, lifetime);
    }
}
