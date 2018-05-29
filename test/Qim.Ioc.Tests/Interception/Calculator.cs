using Qim.Ioc.Interception;

namespace Qim.Ioc.Tests.Interception
{

    public interface ICalculator : IInterceptingTarget
    {
        int Add(int x, int y);

        int Subtract(int x, int y);
    }

    public class Calculator : ICalculator
    {

        [Empty(Order = 10)]
        [PlusOne(Order = 100)]
        public int Add(int x, int y)
        {
            return x + y;
        }

        [Empty(Order = 220)]
        [PlusOne(Order = 10)]
        public int Subtract(int x, int y)
        {
            return x - y;
        }
    }

    public class NumberPy: IInterceptingTarget
    {
        /// <summary>
        ///  虚方法拦截
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [PlusOne]
        public virtual int Divide(int x, int y)
        {
            Ensure.NotZero(y,nameof(y));
            return x/y;
        }
    }
}