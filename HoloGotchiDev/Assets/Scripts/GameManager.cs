using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private HoloPal Holopal;
    [SerializeField]
    private AudioSource audioSource;

    public AudioSource AudioSource => audioSource;

    void Update()
    {
        GetComponentInChildren<DecalProjector>().fadeFactor = 1f - Holopal.clean;
    }
}
