using UnityEngine;

public class BeerBottle : MonoBehaviour
{
    public DizzyEffect dizzyEffect;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        audioSource.Play();
        dizzyEffect.StartDizzy();
    }
}
