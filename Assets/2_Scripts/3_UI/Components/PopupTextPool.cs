using System.Collections.Generic;
using UnityEngine;

namespace JumpRabbit.UI
{
    public class PopupTextPool
    {
        private readonly PopupText _popupPrefab;
        private readonly Transform _poolRoot;

        private readonly Queue<PopupText> _pool = new();


        public PopupTextPool(PopupText prefab, Transform root)
        {
            _popupPrefab = prefab;
            _poolRoot = root;
        }

        public void Init(int prewarmCount = 3)
        {
            Prewarm(prewarmCount);
        }

        private void Prewarm(int prewarmCount)
        {
            for (int i = 0; i < prewarmCount; i++)
            {
                PopupText popupText = CreateNew(_popupPrefab);
                popupText.gameObject.SetActive(false);
                _pool.Enqueue(popupText);
            }
        }

        public PopupText Get()
        {
            PopupText popupText = _pool.Count > 0
                ? _pool.Dequeue()
                : CreateNew(_popupPrefab);

            return popupText;
        }

        public void Release(PopupText popupText)
        {
            popupText.gameObject.SetActive(false);
            popupText.transform.SetParent(_poolRoot);

            _pool.Enqueue(popupText);
        }

        private PopupText CreateNew(PopupText prefab)
        {
            PopupText popupText = Object.Instantiate(prefab, _poolRoot);

            return popupText;
        }
    }

}
