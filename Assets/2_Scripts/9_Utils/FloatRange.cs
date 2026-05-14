namespace JumpRabbit.Utils
{
    [System.Serializable]
    public struct FloatRange
    {
        [UnityEngine.SerializeField] private float _min;
        [UnityEngine.SerializeField] private float _max;

        public float Min => _min;
        public float Max => _max;

        public float GetRandom()
        {
            return UnityEngine.Random.Range(_min, _max);
        }
    }
}
