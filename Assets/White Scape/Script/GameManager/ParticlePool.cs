/*	  - Codeby Bui Thanh Loc -
	contact : builoc08042004@gmail.com
*/

using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : ObjectPool<ParticleSystem>
{
    public new static ParticlePool Instance { get; private set; }

    public List<ParticleSystem> particlePrefabs;  // Danh sách các prefab particle

    private Dictionary<ParticleSystem, Queue<ParticleSystem>> particleDict;

    public new void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        base.Awake();
        particleDict = new Dictionary<ParticleSystem, Queue<ParticleSystem>>();

        // Tạo dictionary để lưu trữ queue cho từng loại particle
        foreach (var particlePrefab in particlePrefabs)
        {
            particleDict[particlePrefab] = new Queue<ParticleSystem>();

            // Khởi tạo pool cho từng particle prefab
            for (int i = 0; i < initialPoolSize; i++)
            {
                ParticleSystem newParticle = Instantiate(particlePrefab, PoolParent);
                newParticle.gameObject.SetActive(false);
                particleDict[particlePrefab].Enqueue(newParticle);
            }
        }
    }

    // Lấy particle dựa trên loại cụ thể
    public ParticleSystem GetParticle(ParticleSystem type)
    {
        if (!particleDict.ContainsKey(type))
        {
            Debug.LogError("Loại particle không tồn tại: " + type);
            return null;
        }

        // Nếu không còn particle nào trong pool, tạo thêm
        if (particleDict[type].Count == 0)
        {
            ParticleSystem newParticle = Instantiate(type, PoolParent);
            newParticle.gameObject.SetActive(false);
            particleDict[type].Enqueue(newParticle);
        }

        // Lấy particle từ pool
        ParticleSystem particleToReturn = particleDict[type].Dequeue();
        particleToReturn.transform.SetParent(ActiveParent);
        particleToReturn.gameObject.SetActive(true);
        return particleToReturn;
    }

    // Trả lại particle về pool
    public void ReturnParticle(ParticleSystem particle)
    {
        // Tìm loại particle để trả về đúng pool
        ParticleSystem particleType = null;
        foreach (var prefab in particlePrefabs)
        {
            if (prefab.name == particle.name.Replace("(Clone)", "").Trim())
            {
                particleType = prefab;
                break;
            }
        }

        if (particleType == null)
        {
            Debug.LogError("Particle không hợp lệ hoặc không thuộc quản lý của pool: " + particle.name);
            return;
        }

        particle.gameObject.SetActive(false);
        particle.transform.SetParent(PoolParent);
        particleDict[particleType].Enqueue(particle);
    }
}
