using System;
using System.Collections;

namespace nm2
{
    public abstract class Base : IFormattable, IEnumerable, ICloneable

    {
        private static uint _nextIdCounter = 1;
        private uint _id;

        public uint Id
        {
            get => _id;
            private set => _id = value;
        }


        protected Base()
        {
            Id = _nextIdCounter++;
        }

        // public abstract bool CanAdd(object left, object right);
        // public abstract bool CanMul();
        public abstract double GetMin();
        public abstract double GetMax();

        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return $"Объект с id: {Id}";
        }

        public abstract IEnumerator GetEnumerator();

        object ICloneable.Clone()
        {
            return Clone();
        }

        public abstract object Clone();

        
    }

}