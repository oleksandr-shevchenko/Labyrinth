using UnityEngine;
using System.Collections;
namespace OtherScripts
{
    /// <summary>
    /// This class implements data structure of range minimum query.
    /// </summary>
    public class RMQ
    {
        private int _size;
        private int[] _arrayOfValues;
        private int[] _arrayOfIndeces;

        public RMQ(int size)
        {
            this._size = size;

            _arrayOfValues = new int[2 * size];
            _arrayOfIndeces = new int[2 * size];

            for (int i = 0; i < _arrayOfValues.Length; i++)
                _arrayOfValues[i] = int.MaxValue / 2;

            for (int i = 0; i < size; i++)
                _arrayOfIndeces[size + i] = i;
        }

        // This method adds a new element.
        public void Set(int index, int value)
        {
            _arrayOfValues[_size + index] = value;
            for (int v = (_size + index) / 2; v > 0; v /= 2)
            {
                int leftIndex = 2 * v;
                int rightIndex = leftIndex + 1;

                if (_arrayOfValues[leftIndex] <= _arrayOfValues[rightIndex])
                {
                    _arrayOfValues[v] = _arrayOfValues[leftIndex];
                    _arrayOfIndeces[v] = _arrayOfIndeces[leftIndex];
                }
                else
                {
                    _arrayOfValues[v] = _arrayOfValues[rightIndex];
                    _arrayOfIndeces[v] = _arrayOfIndeces[rightIndex];
                }
            }
        }

        // This method returns a minimal index.
        public int MinIndex()
        {
            return _arrayOfValues[1] < int.MaxValue / 2 ? _arrayOfIndeces[1] : -1;
        }
    }
}