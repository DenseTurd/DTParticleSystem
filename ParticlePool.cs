using UnityEngine;
public class ParticlePool : GenericObjectPool<Component>
{
    [HideInInspector] public override Component Prefab { get => base.Prefab; set => base.Prefab = value; }

}
