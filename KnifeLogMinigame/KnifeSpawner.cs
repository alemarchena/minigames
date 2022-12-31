using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    [SerializeField]
    Transform logTarget = null;

    public float retardoSpawn = 0;

    [SerializeField]
    Knife currentKnife = null;

    [SerializeField]
    GameObject prefabCuchillo = null;

    public void ThrowKnife()
    {
        currentKnife.Throw(logTarget.position);
    }

    public void CreaCuchillo()
    {
        GameObject cuchillo;
        cuchillo = Instantiate(prefabCuchillo) as GameObject;
        cuchillo.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        currentKnife = cuchillo.GetComponent<Knife>();
        cuchillo.tag = "Cuchillo";
    }
}
