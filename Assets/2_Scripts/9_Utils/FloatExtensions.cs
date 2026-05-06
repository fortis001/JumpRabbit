using UnityEngine;

namespace LSH.JumpRabbit.Utils
{
    /// <summary>
    /// float 타입에 대한 편의 기능을 제공하는 확장 메서드 클래스입니다.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// 숫자를 천 단위 구분 기호(,)와 지정된 소수점 자릿수가 포함된 문자열로 변환합니다.
        /// </summary>
        /// <param name="value">변환할 숫자</param>
        /// <param name="pointNumber">표시할 소수점 자릿수 (0 이상의 정수)</param>
        /// <returns>포맷팅된 문자열 (예: 1,234.56)</returns>
        public static string ToFormatString(this float value, int pointNumber = 0)
        {
            int precision = Mathf.Max(0, pointNumber);

            return value.ToString($"N{precision}");
        }
    }
}

