using System.Collections.Generic;

namespace TypeTrait
{
    namespace Struct
    {
        public struct Tuple<T0> : IEqualityComparer<Tuple<T0>>
        {
            public T0 data0;

            public Tuple(T0 t0)
            {
                data0 = t0;
            }

            public bool Equals(Tuple<T0> left, Tuple<T0> right)
            {
                return left.data0.Equals(right.data0);
            }

            public int GetHashCode(Tuple<T0> tuple)
            {
                return tuple.GetHashCode();
            }
        }

        public struct Tuple<T0, T1> : IEqualityComparer<Tuple<T0, T1>>
        {
            public T0 data0;
            public T1 data1;

            public Tuple(T0 t0, T1 t1)
            {
                data0 = t0;
                data1 = t1;
            }

            public bool Equals(Tuple<T0, T1> left, Tuple<T0, T1> right)
            {
                return left.data0.Equals(right.data0) && left.data1.Equals(right.data1);
            }

            public int GetHashCode(Tuple<T0, T1> tuple)
            {
                return tuple.GetHashCode();
            }
        }
    }
    namespace Class
    {
        public class Tuple<T0> : IEqualityComparer<Tuple<T0>>
        {
            public T0 data0;

            public Tuple(T0 t0)
            {
                data0 = t0;
            }

            public bool Equals(Tuple<T0> left, Tuple<T0> right)
            {
                return left.data0.Equals(right.data0);
            }

            public int GetHashCode(Tuple<T0> tuple)
            {
                return tuple.GetHashCode();
            }
        }

        public class Tuple<T0, T1> : IEqualityComparer<Tuple<T0, T1>>
        {
            public T0 data0;
            public T1 data1;

            public Tuple(T0 t0, T1 t1)
            {
                data0 = t0;
                data1 = t1;
            }

            public bool Equals(Tuple<T0, T1> left, Tuple<T0, T1> right)
            {
                return left.data0.Equals(right.data0) && left.data1.Equals(right.data1);
            }

            public int GetHashCode(Tuple<T0, T1> tuple)
            {
                return tuple.GetHashCode();
            }
        }
    }
}