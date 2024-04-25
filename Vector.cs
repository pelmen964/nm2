using System;
using System.Collections;
using System.Text;

namespace nm2
{
        public class Vector : Base

    {
        private double[] _data;
        private uint _size;


        public Vector(uint len, double[] arr = null)
        {
            Console.Out.WriteLine($"Конструктор вектора {Id}");
            _size = len;
            _data = new double[len];

            if (arr == null)
            {
                for (int i = 0; i < _size; i++)
                {
                    _data[i] = 0;
                }
                return;
            }
            
            if (_size < arr.Length)
            {
                for (int i = 0; i < _size; i++)
                {
                    _data[i] = arr[i];
                }
            }
            else
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    _data[i] = arr[i];
                }

                for (int i = arr.Length; i < _size; i++)
                {
                    _data[i] = 0;
                }
            }
        }

        public Vector(double[] arr) : this((uint)arr.Length, arr) { }

        public Vector(Vector other)
        {
            Size = other.Size;
            _data = new double[Size];
            for (uint i = 0; i < Size; i++)
            {
                _data[i] = other[i];
            }
        }

        ~Vector()
        {
            Console.Out.WriteLine($"Деструктор вектора {Id}");
        }

        public uint Size
        {
            get => _size;
            private set => _size = value;
        }
        
        public double[] Data
        {
            get => _data;
            private set => _data = value;
        }
        
        public double this[uint index]
        {
            get
            {
                if (index > _size)
                    throw new ArgumentException("Выход за пределы");

                return _data[index];
            }
            set
            {
                if (index > _size)
                    throw new ArgumentException("Выход за пределы");

                _data[index] = value;
            }
        }

        public override double GetMax()
        {
            double max = _data[0];
            for (int i = 1; i < _size; i++)
            {
                max = Math.Max(max, _data[i]);
            }

            return max;
        }

        public override double GetMin()
        {
            double min = _data[0];
            for (int i = 1; i < _size; i++)
            {
                min = Math.Min(min, _data[i]);
            }

            return min;
        }

        public static bool CanAdd(Vector left, Vector right)
        {
            return left.Size == right.Size;
        }

        public static bool CanMul(Vector left, Vector right)
        {
            return CanAdd(left, right);
        }

        public override IEnumerator GetEnumerator()
        {
            for (uint i = 0; i < Size; i++)
            {
                yield return _data[i];
            }
        }

        public override object Clone()
        {
            Vector res = new Vector(this);
            return res;
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Вектор id: {Id}");
            foreach (double var in this)
            {
                sb.Append($"\t{var}");
            }

            sb.Append("\n");
            return sb.ToString();
        }

        public int CompareTo(Vector obj)
        {
            if (Size != obj.Size)
            {
                return Size.CompareTo(obj.Size);
            }

            return GetMax().CompareTo(obj.GetMax());
        }

        public double Norm()
        {
            double res = 0;
            foreach (double val in this)
            {
                res += Math.Abs(val);
            }
            return res;
        }
        
        public static Vector operator +(Vector left, Vector right)
        {
            if (!CanAdd(left, right))
            {
                throw new ArgumentException($"Не могу сложить {left.Id}+{right.Id}");
            }

            Vector res = new Vector(left);

            for (uint i = 0; i < left.Size; i++)
            {
                res[i] += right[i];
            }

            return res;
        }

        public static Vector operator -(Vector left, Vector right)
        {
            if (!CanAdd(left, right))
            {
                throw new ArgumentException($"Не могу вычесть {left.Id}-{right.Id}");
            }

            Vector res = new Vector(left);

            for (uint i = 0; i < left.Size; i++)
            {
                res[i] -= right[i];
            }

            return res;
        }

        public static double operator *(Vector left, Vector right)
        {
            if (!CanMul(left, right))
            {
                throw new ArgumentException($"Не могу умножить {left.Id}*{right.Id}");
            }

            double dot_product = 0;
            for (uint i = 0; i < left.Size; ++i)
            {
                dot_product += left[i] * right[i];
            }

            return dot_product;
        }

        public static Vector operator *(Vector left, double scalar)
        {
            Vector res = new Vector(left);
            for (uint i = 0; i < left.Size; i++)
            {
                left[i] *= scalar;
            }

            return res;
        }
    }

}