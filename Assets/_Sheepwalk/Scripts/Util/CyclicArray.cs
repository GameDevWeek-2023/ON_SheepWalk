using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sheepwalk.util
{
    public class CyclicArray<T> : IEnumerable<T>
    {
        private readonly T[] _buffer;
        private readonly int _capacity;
        private int _size;
        private int _start;
        private int _end;
        
        public CyclicArray(int capacity)
        {
            if (capacity < 1) throw new ArgumentException("Cannot have 0 or negative capacity");
            _buffer = new T[capacity];
            _capacity = capacity;
            _size = 0;
            _start = 0;
            _end = 0;
        }

        public bool IsEmpty
        {
            get { return _size == 0; }
        }
        
        public bool IsFull
        {
            get { return _size == _capacity; }
        }

        public int Count
        {
            get { return _size; }
        }

        public int Capacity
        {
            get { return _capacity; }
        }

        private int CircularInc(int number)
        {
            return (number+1) % _capacity;
        }

        private int CircularAdd(int a, int b)
        {
            return (a + b) % _capacity;
        }

        public void Add(T item)
        {
            if (IsFull) _start = CircularInc(_start);
            else _size++;

            _end = CircularInc(_end);
            _buffer[_end] = item;
        }

        public T this[int index]
        {
            get
            {
                if (index < -_size || index > _size - 1) 
                    throw new ArgumentException("Index " + index + " out of range.");

                return _buffer[CircularAdd(_start, index)];
            }
            set
            {
                if (index < -_size || index > _size - 1) 
                    throw new ArgumentException("Index " + index + " out of range.");
                _buffer[CircularAdd(_start, index)] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_size <= 0) yield break;
            var position = _start;

            while (position != _end)
            {
                yield return _buffer[position];
                position = CircularInc(position);
            }

            yield return _buffer[_end];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }    
}

