using System.Collections.Generic;
using UnityEngine;


namespace JumpRabbit.GamePlay.Entities
{
    public class PlatformPool
    {
        private readonly Transform _poolRoot;

        private readonly Dictionary<Platform, Queue<Platform>> _pools = new();
        private readonly Dictionary<Platform, Platform> _originPrefabs = new();

        private int _prewarmCount = 2;

        public PlatformPool(Transform poolRoot)
        {
            _poolRoot = poolRoot;
        }

        public void Init(IReadOnlyList<PlatformGroup> platformGroups, int prewarmCount)
        {
            Prewarm(platformGroups);
            _prewarmCount = prewarmCount;
        }

        private void Prewarm(IReadOnlyList<PlatformGroup> platformGroups)
        {
            if (platformGroups == null)
                return;

            foreach (PlatformGroup group in platformGroups)
            {
                if (group == null || group.Prefabs == null)
                    continue;

                foreach (Platform prefab in group.Prefabs)
                {
                    if (prefab == null)
                        continue;

                    if (!_pools.ContainsKey(prefab))
                        _pools[prefab] = new Queue<Platform>();

                    for (int i = 0; i < _prewarmCount; i++)
                    {
                        Platform platform = CreateNew(prefab);
                        platform.gameObject.SetActive(false);
                        _pools[prefab].Enqueue(platform);
                    }
                }
            }
            
        }
        public Platform Get(Platform prefab)
        {
            if (prefab == null)
                return null;

            if (!_pools.TryGetValue(prefab, out Queue<Platform> pool))
            {
                pool = new Queue<Platform>();
                _pools[prefab] = pool;
            }

            Platform platform = pool.Count > 0
                ? pool.Dequeue()
                : CreateNew(prefab);

            platform.transform.SetParent(null);
     
            platform.gameObject.SetActive(true);

            return platform;
        }

        public void Release(Platform platform)
        {
            if (platform == null)
                return;

            if (!_originPrefabs.TryGetValue(platform, out Platform prefab))
            {
                Object.Destroy(platform.gameObject);
                return;
            }

            platform.gameObject.SetActive(false);
            platform.transform.SetParent(_poolRoot);

            _pools[prefab].Enqueue(platform);
        }


        private Platform CreateNew(Platform prefab)
        {
            Platform platform = Object.Instantiate(prefab, _poolRoot);
            platform.Init();
            _originPrefabs[platform] = prefab;
            return platform;
        }

    }
}

