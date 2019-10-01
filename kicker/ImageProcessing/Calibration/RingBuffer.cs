using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing.Calibration
{
    class RingBuffer<T> : IEnumerable<T>
    {
        private object _objectLock = new object();
        private T[] _buffer;
        private int _readPos = 0;
        private int _writePos = 0;

        public int Capacity { get; }

        public RingBuffer(int capacity)
        {
            _buffer = new T[capacity];
            Capacity = capacity;
        }

        public void Add(T element)
        {
            lock (_objectLock)
            {
                // Jump over read position if write pointer arrived there and the field not null
                if (_readPos == _writePos && _buffer[_readPos] != default)
                {
                    Increment(ref _writePos);
                }

                _buffer[_writePos] = element;
                Increment(ref _writePos);
            }
        }

        public T Take()
        {
            lock (_objectLock)
            {
                var element = _buffer[_readPos];
                _buffer[_readPos] = default;
                Increment(ref _readPos);

                return element;
            }
        }

        public void Clear()
        {
            lock (_objectLock)
            {
                for (int i = 0; i < _buffer.Length; i++)
                {
                    _buffer[i] = default;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_buffer).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)_buffer).GetEnumerator();
        }

        private void Increment(ref int position)
        {
            position = (position + 1) % Capacity;
        }
    }
}
