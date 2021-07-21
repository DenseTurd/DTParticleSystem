using UnityEngine;

[ExecuteAlways]
public class ParticleController : MonoBehaviour
{
    float initialScale;
    float finalScale;
    bool growThenShrink;
    Vector2 growth;

    Vector3 dir;
    Vector3 gravity;
    Vector3 initialGravity;

    float speed;
    float drag;

    float currentLifeTime;
    float maxLifeTime;

    public DTParticleSystem dtps;
    public ParticleDatas particleDatas;
    bool initialized;

    float deltaTime;
    float lastFrameTime;

    public void Init(ParticleDatas PD)
    {
        particleDatas = PD;
        SetPosition(PD);
        SetRotation(PD);
        SetScaling(PD);
        SetDirection(PD);
        SetGravity(PD);
        SetLifeTime(PD);

        speed = Rand.Range(PD.Speed.x, PD.Speed.y);
        drag = PD.Drag;

        initialized = true;
        lastFrameTime = Time.realtimeSinceStartup;
    }

    void SetPosition(ParticleDatas PD)
    {
        Vector3 randomSpawnPos = Random.insideUnitSphere * PD.Area;
        transform.position = PD.SpawnPos + randomSpawnPos;
    }

    void SetRotation(ParticleDatas PD)
    {
        transform.rotation = Quaternion.Euler(new Vector3(Rand.Range(-PD.Rotation, PD.Rotation), Rand.Range(-PD.Rotation, PD.Rotation), Rand.Range(-PD.Rotation, PD.Rotation)));
    }

    void SetScaling(ParticleDatas PD)
    {
        initialScale = Rand.Range(PD.Scale.x, PD.Scale.y);

        finalScale = Rand.Range(initialScale * PD.Growth.x, initialScale * PD.Growth.y);

        growThenShrink = PD.GrowThenShrink;
        growth = PD.Growth;

        float realScale = growThenShrink ? initialScale * growth.x : initialScale;
        transform.localScale = new Vector3(realScale, realScale, realScale);
    }

    void SetDirection(ParticleDatas PD)
    {
        if (PD.RandomDirection)
        {
            dir = Random.insideUnitSphere.normalized;
        }
        else
        {
            float variance = PD.DirectionVariance / 90;
            Vector3 dirNorm = PD.Direction.normalized;
            float x = CalcVariance(dirNorm.x, variance);
            float y = CalcVariance(dirNorm.y, variance);
            float z = CalcVariance(dirNorm.z, variance);
            dir = new Vector3(x, y, z).normalized;
        }
    }

    float CalcVariance(float f, float variance)
    {
        float offset = Rand.Range(-variance, variance);
        float val = f - (f * (variance / 2)) + offset;
        return Mathf.Abs(val) > 1 ? val > 0 ? 1 : -1 : f + offset;
    }

    void SetGravity(ParticleDatas PD)
    {
        if (PD.Gravity)
        {
            initialGravity = PD.GravityDirection.normalized * PD.GravityScale;
            gravity = initialGravity;
        }
    }

    void SetLifeTime(ParticleDatas PD)
    {
        currentLifeTime = 0;
        maxLifeTime = Rand.Range(PD.LifeTime.x, PD.LifeTime.y);
    }

    public void Update()
    {
        if (!initialized) return;

        deltaTime = Application.isPlaying ? Time.deltaTime : Time.realtimeSinceStartup - lastFrameTime;
        lastFrameTime = Time.realtimeSinceStartup;

        Move();
        Drag();
        Gravity();
        Growth();
        LifeTime();
    }

    void Move()
    {
        transform.Translate(dir * speed * deltaTime, Space.World);
    }

    void Drag()
    {
        speed -= speed * deltaTime * drag;
    }

    void Gravity()
    {
        if (particleDatas.Gravity)
        {
            transform.Translate(gravity * deltaTime, Space.World);
            gravity += initialGravity * deltaTime * 9.8f;
        }
    }

    void Growth()
    {
        float t = currentLifeTime / maxLifeTime;
        float scale;
        if (growThenShrink)
        {
            scale = Mathf.Sin(Mathf.Lerp(0, 180, t) * Mathf.Deg2Rad);
            float span = growth.y - growth.x;
            scale *= span;
            scale += initialScale * growth.x;
            scale *= initialScale;
        }
        else
        {
            scale = Mathf.Lerp(initialScale, finalScale, t);
        }
        transform.localScale = new Vector3(scale, scale, scale);
    }

    void LifeTime()
    {
        currentLifeTime += deltaTime;

        if (currentLifeTime >= maxLifeTime)
        {
            initialized = false;
            ReturnToPool();
        }
    }

    public void ReturnToPool()
    {
        dtps.pool.ReturnToPool(this);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (initialized)
            {
                Update();
            }
        }
    }
#endif
}
