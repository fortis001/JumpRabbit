using System.Collections.Generic;
using JumpRabbit.GamePlay.Entities;
using UnityEngine;


namespace JumpRabbit.GamePlay.InGame
{
    public class PlatformManager : MonoBehaviour
    {
        [SerializeField] private PlatformSpawnSettings _spawnSettings;
        [SerializeField] private Transform _poolRoot;
        [SerializeField] private Platform _startPlatform;
        [SerializeField] private int _prewarmCount = 2;
        [SerializeField] private int _initialSpawnCount = 5;
        [SerializeField] private int _maxPlatformCount = 10;
        

        private InGameCameraController _cameraController;
        private PlatformPool _platformPool;
        private Platform _lastPlatform;
        private Queue<Platform> _platformQueue = new();
        private int _currentPlayerIndex = 0;
        private int _currentPlatformIndex;



        public void Init(InGameCameraController cameraController)
        {
            _platformPool = new PlatformPool(_poolRoot);
            _platformPool.Init(_spawnSettings.PlatformGroups, _prewarmCount);

            _lastPlatform = _startPlatform;


            for (int i = 0; i < _initialSpawnCount; i++)
            {
                SpawnNextPlatform();
            }

            _cameraController = cameraController;
        }

        private void SpawnNextPlatform()
        {
            if (!_spawnSettings.TryGetRandomPlatform(out PlatformSpawnData spawnData))
                return;


            Platform platform = _platformPool.Get(spawnData.Prefab);

            if (platform == null)
                return;

            Vector3 spawnPosition = GetNextPosition(platform, spawnData.SpawnOffset);
            platform.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);

            _currentPlatformIndex++;
            platform.Activate(_currentPlatformIndex);
            platform.OnPlayerLanded += HandlePlayerLandedPlatform;

            _platformQueue.Enqueue(platform);

            _lastPlatform = platform;

        }

        private Vector3 GetNextPosition(Platform nextPlatform, Vector2 spawnoffset)
        {
            Vector2 lastPosition = _lastPlatform.transform.position;
            
            float halfWidthSum = _lastPlatform.HalfWidth + nextPlatform.HalfWidth;

            Vector2 spawnGap = new Vector2(spawnoffset.x + halfWidthSum, spawnoffset.y);

            Vector2 nextPosition = lastPosition + spawnGap;

            return nextPosition;
        }

        private void HandlePlayerLandedPlatform(Platform platform, bool isFirstVisit)
        {
            if (isFirstVisit && platform.Index > _currentPlayerIndex)
            {
                int passCount = platform.Index - _currentPlayerIndex;
                _currentPlayerIndex = platform.Index;

                for (int i = 0; i < passCount; i++)
                {
                    SpawnNextPlatform();
                }
                ReleaseOldPlatform();

            }

            if (_cameraController != null)
            {
                _cameraController.FocusToPlayer();
            }
        }

        private void ReleaseOldPlatform()
        {
            while (_platformQueue.Count > _maxPlatformCount)
            {
                Platform platform = _platformQueue.Dequeue();

                if (platform == null)
                    continue;

                platform.OnPlayerLanded -= HandlePlayerLandedPlatform;
                DespawnPlatform(platform);
            }
        }

        private void DespawnPlatform(Platform platform)
        {
            platform.Deactivate();
            _platformPool.Release(platform);
        }
    }
}

