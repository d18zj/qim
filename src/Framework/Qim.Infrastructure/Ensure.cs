using System;

namespace Qim
{
    public static class Ensure
    {
        /// <summary>
        ///     确保参数不为null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void NotNull<T>(T argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }

        /// <summary>
        ///     确保字符串参数不为null及String.Empty
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void NotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(argument, argumentName);
        }

        /// <summary>
        ///     确保字符串参数不为null、空字符串及空白字符
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void NotNullOrWhiteSpace(string argument, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(argument))
                throw new ArgumentNullException(argument, argumentName);
        }

        /// <summary>
        ///     确保参数大于0
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void Positive(int number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be positive.");
        }

        /// <summary>
        ///     确保参数大于0
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void Positive(long number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be positive.");
        }

        /// <summary>
        ///     确定参数不为负(>=0)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void Nonnegative(long number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be non negative.");
        }

        /// <summary>
        ///     确定参数不为负(>=0)
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void Nonnegative(int number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be non negative.");
        }

        /// <summary>
        ///     确定参数不为0
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void NotZero(int number, string argumentName)
        {
            if (number == 0)
                throw new ArgumentOutOfRangeException(argumentName, argumentName + " should be non zero.");
        }

        /// <summary>
        ///     确保参数不为空(Guid.Empty)
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="argumentName"></param>
        public static void NotEmptyGuid(Guid guid, string argumentName)
        {
            if (Guid.Empty == guid)
                throw new ArgumentException(argumentName, argumentName + " shoud be non-empty GUID.");
        }

        /// <summary>
        ///     确保两个参数相等
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="argumentName"></param>
        public static void Equal(int expected, int actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentException(string.Format("{0} expected value: {1}, actual value: {2}", argumentName,
                    expected, actual));
        }

        /// <summary>
        ///     确保两个参数相等
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="argumentName"></param>
        public static void Equal(long expected, long actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentException(string.Format("{0} expected value: {1}, actual value: {2}", argumentName,
                    expected, actual));
        }

        /// <summary>
        ///     确保两个参数相等
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="argumentName"></param>
        public static void Equal(bool expected, bool actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentException(string.Format("{0} expected value: {1}, actual value: {2}", argumentName,
                    expected, actual));
        }
    }
}