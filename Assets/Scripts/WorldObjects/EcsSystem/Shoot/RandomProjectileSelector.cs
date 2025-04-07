using UnityEngine;

public class RandomProjectileSelector : MonoBehaviour, IProjectileSelector
{
    public GameObject[] ProjectilePrefab;
    public GameObject SelectProjectile(Shooter shooter)
    {
        return ProjectilePrefab[Random.Range(0, ProjectilePrefab.Length)];
    }
}