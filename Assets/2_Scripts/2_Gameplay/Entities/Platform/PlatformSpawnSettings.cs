using System;
using System.Collections.Generic;
using JumpRabbit.Utils;
using UnityEngine;

namespace JumpRabbit.GamePlay.Entities
{
    public enum PlatformSize
    {
        Small,
        Medium,
        Large
    }

    public readonly struct PlatformSpawnData
    {
        public readonly Platform Prefab;
        public readonly PlatformSize Size;
        public readonly Vector2 SpawnOffset;

        public PlatformSpawnData(Platform prefab, PlatformSize size, Vector2 spawnOffset)
        {
            Prefab = prefab;
            Size = size;
            SpawnOffset = spawnOffset;
        }
    }

    [Serializable]
    public class PlatformGroup
    {
        [SerializeField] private PlatformSize _size;
        [SerializeField] private Platform[] _prefabs;

        [Header("Spawn Range")]
        [SerializeField] private float _baseXGap;
        [SerializeField] private FloatRange _xGapRange;
        [SerializeField] private float _baseYOffset = 0f;
        [SerializeField] private FloatRange _yOffsetRange;

        public PlatformSize Size => _size;
        public Platform[] Prefabs => _prefabs;

        public Platform GetRandomPrefab()
        {
            if (_prefabs == null || _prefabs.Length == 0)
                return null;

            return _prefabs[UnityEngine.Random.Range(0, _prefabs.Length)];
        }

        public Vector2 GetSpawnOffset()
        {
            return new Vector2(
                _baseXGap + _xGapRange.GetRandom(),
                _baseYOffset + _yOffsetRange.GetRandom());
        }
    }

    [Serializable]
    public class CarrotSpawnData
    {
        [SerializeField] private BonusCarrot _carrotPrefab;
        [SerializeField] private float _carrotSpawnChance = 0.05f;
        [SerializeField] private Vector3 _carrotSpawnOffset = new Vector3(0, 5f, 0);

        public BonusCarrot CarrotPrefab => _carrotPrefab;
        public float CarrotSpawnChance => _carrotSpawnChance;
        public Vector3 CarrotSpawnOffset => _carrotSpawnOffset;
    }

    [CreateAssetMenu(
        fileName = "PlatformSpawnSettings",
        menuName = "JumpRabbit/Gameplay/Platform Spawn Settings")]
    public class PlatformSpawnSettings : ScriptableObject
    {
        [SerializeField] private List<PlatformGroup> _platformGroups = new();
        [SerializeField] private CarrotSpawnData _carrotSpawnData;

        public IReadOnlyList<PlatformGroup> PlatformGroups => _platformGroups;
        public CarrotSpawnData CarrotSpawnData => _carrotSpawnData;

        public bool TryGetGroup(PlatformSize size, out PlatformGroup result)
        {
            result = null;

            foreach (PlatformGroup group in _platformGroups)
            {
                if (group == null)
                    continue;

                if (group.Size == size)
                {
                    result = group;
                    return true;
                }
            }

            return false;
        }

        public bool TryGetRandomPlatform(out PlatformSpawnData result)
        {
            result = default;

            if (_platformGroups == null || _platformGroups.Count == 0)
                return false;

            PlatformGroup group = _platformGroups[
                UnityEngine.Random.Range(0, _platformGroups.Count)];

            if (group == null)
                return false;

            Platform prefab = group.GetRandomPrefab();

            if (prefab == null)
                return false;

            result = new PlatformSpawnData(
                prefab,
                group.Size,
                group.GetSpawnOffset());

            return true;
        }
    }
}