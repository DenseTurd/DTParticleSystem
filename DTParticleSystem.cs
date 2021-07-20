using System;
using UnityEngine;
using UnityEditor;
[ExecuteAlways]
public struct ParticleDatas
{
    public Vector3 SpawnPos;
    public float Area;
    public Vector2 Speed;
    public Vector2 LifeTime;
    public float Rotation;

    public bool RandomDirection;
    public Vector3 Direction;
    public float DirectionVariance;

    public bool Gravity;
    public float GravityScale;
    public Vector3 GravityDirection;

    public float Drag;

    public Vector2 Scale;
    public Vector2 Growth;
    public bool GrowThenShrink;

    public GenericObjectPool<Component> Pool;
}

[RequireComponent(typeof(ParticlePool))]
public class DTParticleSystem : MonoBehaviour
{
    public Component prefab;

    public bool triggered;
    public Vector4 triggeredAmount = new Vector4(2, 7, 1, 10);
    public Vector3 duration = new Vector3(0.5f, 0.01f, 5);

    
    public Vector3 area = new Vector3(0.5f, 0.1f, 10f);
    public Vector3 frequency = new Vector3(20f, 0.1f, 30f);
    public Vector4 speed = new Vector4(3, 6, 0.1f, 30);
    public Vector4 lifeTime = new Vector4(0.5f, 1f, 0.1f, 5);
    public Vector3 rotation = new Vector3(22, 0, 180);

    [Space]
    public bool randomDirection = true;
    public Vector3 direction = new Vector3(1, 0, 0);
    public Vector3 directionVariance = new Vector3(22, 0, 180);

    [Space]
    public bool gravity = true;
    public Vector3 gravityScale = new Vector3(1.5f, 0.1f, 5);
    public Vector3 gravityDirection = new Vector3(0, 1, 0);

    [Space]
    public Vector3 drag = new Vector3(3, 0, 5);

    [Space]
    public Vector4 scale = new Vector4(0.5f, 1.5f, 0.01f, 10);
    public Vector4 growth = new Vector4(1, 1, 0.1f, 10);
    public bool growThenShrink;

    ParticleDatas particleDatas;

    float spawnTime;
    float spawnTimer;
    int totalParticles;
    [HideInInspector] public GenericObjectPool<Component> pool;

    bool initialized;
    float deltaTime;
    float lastFrameTime;
    public void Start()
    {
        SetParticleDatas();
        spawnTime = 1 / frequency.x;
        totalParticles = (int)Mathf.Ceil(frequency.x * lifeTime.y);

        ParticleController particleController = prefab.GetComponent<ParticleController>();
        if (!particleController)
        {
            particleController = (ParticleController)prefab.gameObject.AddComponent(typeof(ParticleController));
        }
        particleController.dtps = this;

        pool = this.GetComponentOrComplain<GenericObjectPool<Component>>();
        pool.Prefab = prefab;
        pool.StartingPool(totalParticles);

        initialized = true;

        lastFrameTime = Time.realtimeSinceStartup;

        EditorApplication.playModeStateChanged += ResetSystem;
    }

    void SetParticleDatas()
    {
        particleDatas.SpawnPos = transform.position;
        particleDatas.Area = area.x;
        particleDatas.Speed = new Vector2(speed.x, speed.y);
        particleDatas.LifeTime = new Vector2(lifeTime.x, lifeTime.y);
        particleDatas.Rotation = rotation.x;

        particleDatas.RandomDirection = randomDirection;
        particleDatas.Direction = direction;
        particleDatas.DirectionVariance = directionVariance.x;

        particleDatas.Gravity = gravity;
        particleDatas.GravityScale = gravityScale.x;
        particleDatas.GravityDirection = gravityDirection;

        particleDatas.Drag = drag.x;

        particleDatas.Scale = new Vector2(scale.x, scale.y);
        particleDatas.Growth = new Vector2(growth.x, growth.y);
        particleDatas.GrowThenShrink = growThenShrink;

        particleDatas.Pool = pool;
    }

    const float activateCheckTime = 0.15f;
    float activateCheckTimer;
    bool active;
    public void Update()
    {
        deltaTime = Application.isPlaying ? Time.deltaTime : Time.realtimeSinceStartup - lastFrameTime;
        lastFrameTime = Time.realtimeSinceStartup;
        ActivationCheck();
        if (!active) return;

        spawnTimer -= deltaTime;

        if (spawnTimer <= 0)
        {
            int amountToSpawn = (int)(Mathf.Ceil(deltaTime / spawnTime));
            spawnTimer = spawnTime; 
            Spawn();
        }
    }

    void ActivationCheck()
    {
        activateCheckTimer -= deltaTime;
        if (activateCheckTimer <= 0)
        {
            activateCheckTimer = activateCheckTime;
            if (Vector2.Distance(Camera.main.transform.position, transform.position) < 30)
            {
                active = true;
            }
            else
            {
                active = false;
            }
        } 
    }

    public void Spawn()
    {
        var particle = pool.Get();

        particle.transform.SetParent(transform, false);

        ParticleController particleController = particle.GetComponent<ParticleController>();
        particleController.Init(particleDatas);

        particle.gameObject.SetActive(true);
    }

    public void OnValidate()
    {
        spawnTime = 1 / frequency.x;
        SetParticleDatas();
    }

    public void ResetSystem(PlayModeStateChange state)
    {
        pool.Clear();
    }

#if UNITY_EDITOR

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, area.x);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (!initialized)
            {
                Start();
            }
            Update();
            UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            UnityEditor.SceneView.RepaintAll();
        }
    }

#endif
}
