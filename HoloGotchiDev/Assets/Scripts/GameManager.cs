using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private HoloPal Holopal;

    void Update()
    {
        GetComponent<DecalProjector>().fadeFactor = 1f - Holopal.clean;
    }
}
