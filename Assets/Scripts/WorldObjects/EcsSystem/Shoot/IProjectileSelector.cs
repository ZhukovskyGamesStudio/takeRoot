using UnityEngine;

public interface IProjectileSelector
{
    GameObject SelectProjectile(Shooter shooter);
}

public enum ProjectileSelectorType
{
    Random
}