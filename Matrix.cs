using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace nm2
{
        public class Matrix : Base, IComparable<Matrix>
    {
        private double[,] _data;
        private uint _rows;
        private uint _cols;

        public Matrix(uint rows = 0, uint cols = 0, double[] data = null)
        {
            Console.Out.WriteLine($"Конструктор матрицы {Id}");
            _rows = rows;
            _cols = cols;
            _data = new double[_rows, _cols];
            if (data == null)
            {
                for (int i = 0; i < _rows; i++)
                {
                    for (int j = 0; j < _cols; j++)
                    {
                        _data[i, j] = 0;
                    }
                }
                return;
            }
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _data[i, j] = data[i * _cols + j];
                }
            }
        }

        public Matrix(uint rows = 0, double[] data = null) : this(rows, rows, data) { }

        public Matrix(double[,] data) : this((uint)data.GetLength(0), (uint)data.GetLength(1),
            data.Cast<double>().ToArray()) { }

        public Matrix(Matrix other)
        {
            _rows = other._rows;
            _cols = other._cols;
            _data = new double[_rows, _cols];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    _data[i, j] = other._data[i, j];
                }
            }
        }

        ~Matrix()
        {
            Console.Out.WriteLine($"Деструктор матрицы {Id}");
        }


        public uint Rows
        {
            get => _rows;
            private set => _rows = value;
        }

        public uint Cols
        {
            get => _cols;
            private set => _cols = value;
        }
        
        public double[,] Data
        {
            get => _data;
            private set => _data = _data;
        }
        
        public double this[uint row, uint col]
        {
            get
            {
                if (col >= Cols || row >= Rows)
                    throw new ArgumentException("Выход за границы");
                return _data[row, col];
            }
            internal set
            {
                if (col >= Cols || row >= Rows)
                    throw new ArgumentException("Выход за границы");
                _data[row, col] = value;
            }
        }

        public override IEnumerator GetEnumerator()
        {
            foreach (var val in _data)
            {
                yield return val;
            }
        }

        public override double GetMin()
        {
            double res = this[0, 0];
            foreach (double val in this)
            {
                res = Math.Min(res, val);
            }

            return res;
        }

        public override double GetMax()
        {
            double res = this[0, 0];
            foreach (double val in this)
            {
                res = Math.Max(res, val);
            }

            return res;
        }

        public override object Clone()
        {
            Matrix res = new Matrix(this);
            return res;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Матрица id: {Id}");
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++)
                {
                    sb.Append($"\t{_data[i, j]}");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public int CompareTo(Matrix obj)
        {
            if (_cols * _rows != obj._cols * obj._rows)
            {
                return (_cols * _rows).CompareTo(obj._cols * obj._rows);
            }

            return GetMax().CompareTo(obj.GetMax());
        }

        public static bool CanAdd(Matrix left, Matrix right)
        {
            if (left._rows != right._rows || left._cols != right._cols)
                return false;
            return true;
        }

        public static bool CanMul(Matrix left, Matrix right)
        {
            if (left._cols != right._rows)
                return false;
            return true;
        }

        public static bool CanMul(Matrix left, Vector right)
        {
            if (left._cols != right.Size)
                return false;
            return true;
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
        
        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (!CanAdd(left, right))
                throw new ArgumentException($"Не могу сложить {left.Id}*{right.Id}");
            Matrix res = new Matrix(left);
            for (uint i = 0; i < res._rows; i++)
            {
                for (uint j = 0; j < res._cols; j++)
                {
                    res[i, j] += right[i, j];
                }
            }

            return res;
        }

        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (!CanAdd(left, right))
                throw new ArgumentException($"Не могу вычесть {left.Id}-{right.Id}");
            Matrix res = new Matrix(left);
            for (uint i = 0; i < res._rows; i++)
            {
                for (uint j = 0; j < res._cols; j++)
                {
                    res[i, j] -= right[i, j];
                }
            }

            return res;
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (!CanMul(left, right))
                throw new ArgumentException($"Не могу умножить {left.Id}*{right.Id}");
            
            Matrix result = new Matrix(left._rows, right._cols);
            for (uint i = 0; i < left._rows; i++)
            {
                for (uint j = 0; j < right._cols; j++)
                {
                    double sum = 0;
                    for (uint k = 0; k < left._cols; k++)
                    {
                        sum += left[i,k] * right[k,j];
                    }
                    result[i,j] = sum;
                }
            }
            return result;
        }

        public static Matrix operator *(Matrix left, double scalar)
        {
            Matrix res = new Matrix(left);
            for (uint i = 0; i < left._rows; i++)
            {
                for (uint j = 0; j < left._cols; j++)
                {
                    res[i, j] *= scalar;
                }
            }

            return res;
        }
        
        public static Matrix operator *(Matrix left, Vector right)
        {
            Matrix result = new Matrix(left._rows, 1, left._data.Cast<double>().ToArray());
            if (CanMul(left, right) != true)
                throw new ArgumentException($"Не могу умножить {left.Id}*{right.Id}");

            for (uint i = 0; i < left._rows; i++)
            {
                double temp = 0;
                for (uint j = 0; j < left._cols; j++)
                {
                    temp += left[i,j] * right[j];
                }
                result[i, 0] = temp;
            }
            return result;
        }
    }

}