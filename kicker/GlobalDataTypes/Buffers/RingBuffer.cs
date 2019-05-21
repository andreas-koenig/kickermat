namespace GlobalDataTypes.Buffers
{
    using System;
    using System.Collections;

    /// <summary>
    /// System.Collections conform class for a ring-queue.
    /// </summary>
    /// <remarks>
    /// The collection support adding and removing at both ends and
    /// automatic expansion.
    /// The left end of the ring is referred to as Head, the right end as Tail.
    /// add / remove is O(1)
    /// expansion is O(n)
    /// indexed access and enumeration is O(1)
    /// </remarks>
    public class RingBuffer : ICollection, ICloneable
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer"/> class with capacity 32 and growth 2.
        /// </summary>
        public RingBuffer()
            : this(32, 2.0)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer"/> class with growth 2.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public RingBuffer(int capacity)
            : this(capacity, 2.0)
        { 
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="growthFactor">The growth factor.</param>
        public RingBuffer(int capacity, double growthFactor)
        {
            this.InnerList = new object[capacity];
            this.GrowthFactor = growthFactor;
            this.Head = this.Tail = this.Count = 0;
            this.Version = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer"/> class as a copy of a given collenction.
        /// </summary>
        /// <param name="collection">The source collection.</param>
        public RingBuffer(ICollection collection)
            : this(collection, collection.Count)
        { 
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer"/> class as a copy of a given collection and with a given capacity.
        /// </summary>
        /// <param name="collection">The source collection.</param>
        /// <param name="capacity">The capacity of the new Dequeue (must be >= collection.Count).</param>
        public RingBuffer(ICollection collection, int capacity)
            : this(capacity, 2.0)
        {
            this.EnqueueTailRange(collection);
        }
        
        /// <summary>
        /// Gets or sets the growth factorby which to grow the collection in case of expansion.
        /// </summary>
        /// <value>The growth factor.</value>
        public double GrowthFactor { get; set; }

        /// <summary>
        /// Gets the version of the dequeue. The version is increased with every changing operation.
        /// The main use is to invalidate all IEnumerators.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; private set; }

        /// <summary>
        /// Gets the capacity (The current amount of cells available to the dequeue).
        /// </summary>
        /// <value>The capacity.</value>
        public int Capacity
        {
            get { return this.InnerList.Length; }

            // set
            // {
            //    if (Capacity >= Count)
            //        SetSize(Capacity);
            //    else
            //        throw new ArgumentException("Capacity was smaller than Count!");
            // }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.
        /// </returns>
        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of elements contained in the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        public int Count { get; private set; }
        
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
        /// </returns>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }       

        /// <summary>
        /// Gets or sets the inner list.
        /// </summary>
        /// <value>The inner list.</value>
        protected object[] InnerList { get; set; }

        /// <summary>
        /// Gets or sets the head.
        /// </summary>
        /// <value>The head of the ring buffer.</value>
        protected int Head { get; set; }

        /// <summary>
        /// Gets or sets the tail.
        /// </summary>
        /// <value>The tail of the ringbuffer.</value>
        protected int Tail { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> at the specified index.
        /// Indexed access to all elements currently in the collection.
        /// Indexing starts at 0 (Head) and ends at Count-1 (Tail).
        /// </summary>
        /// <param name="index">The index of the object.</param>
        /// <value>The object at the specified index.</value>
        public object this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return this.InnerList[(this.Head + index) % this.Capacity];
            }

            set
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                this.InnerList[(this.Head + index) % this.Capacity] = value;
                this.Version++;
            }
        }

        /// <summary>
        /// Add the given object to the collections Head
        /// </summary>
        /// <param name="value">The object to enqueue</param>
        public void EnqueueHead(object value)
        {
            if (this.Count == this.Capacity)
            {
                this.SetSize((int)(this.Capacity * this.GrowthFactor));
            }

            this.Head--;
            if (this.Head < 0)
            {
                this.Head += this.Capacity;
            }

            this.InnerList[this.Head] = value;
            this.Count++;
            this.Version++;
        }

        /// <summary>
        /// Add the given object to the collections Tail
        /// </summary>
        /// <param name="value">The object to enqueue</param>
        public void EnqueueTail(object value)
        {
            if (this.Count == this.Capacity)
            {
                this.SetSize((int)(this.Capacity * this.GrowthFactor));
            }

            this.InnerList[this.Tail] = value;
            this.Tail++;
            this.Tail %= this.Capacity;
            this.Count++;
            this.Version++;
        }

        /// <summary>
        /// Retrieve and remove the current Head
        /// </summary>
        /// <returns>The removed object</returns>
        public object DequeueHead()
        {
            if (this.Count == 0)
            {
                throw new RingBufferException("Dequeue was empty!");
            }

            object r = this.InnerList[this.Head];
            this.Head++;
            this.Head %= this.Capacity;
            this.Count--;
            this.Version++;
            return r;
        }

        /// <summary>
        /// Retrieve and remove the current Tail
        /// </summary>
        /// <returns>The removed object</returns>
        public object DequeueTail()
        {
            if (this.Count == 0)
            {
                throw new RingBufferException("Dequeue was empty!");
            }

            object r = this.InnerList[this.Tail];
            this.Tail--;
            if (this.Tail < 0)
            {
                this.Tail += this.Capacity;
            }

            this.Count--;
            this.Version++;
            return r;
        }

        /// <summary>
        /// Add the given collection to the dequeues Tail
        /// </summary>
        /// <param name="collection">The source collection</param>
        public void EnqueueTailRange(ICollection collection)
        {
            int capacity = this.Capacity;
            while (capacity < collection.Count)
            {
                capacity = (int)(capacity * this.GrowthFactor);
            }

            if (capacity > this.Capacity)
            {
                this.SetSize(capacity);
            }

            foreach (object item in collection)
            {
                this.EnqueueTail(item);
            }
        }

        /// <summary>
        /// Add the given collection to the dequeues Head.
        /// To preserve the order in the collection, the entries are
        /// added in revers order.
        /// </summary>
        /// <param name="collection">The source collection</param>
        public void EnqueueHeadRange(ICollection collection)
        {
            int capacity = this.Capacity;
            while (capacity < collection.Count)
            {
                capacity = (int)(capacity * this.GrowthFactor);
            }

            if (capacity > this.Capacity)
            {
                this.SetSize(capacity);
            }

            ArrayList temp = new ArrayList(collection);
            temp.Reverse();
            foreach (object item in temp)
            {
                this.EnqueueHead(item);
            }
        }

        /// <summary>
        /// Deletes all entries from the collection
        /// </summary>
        public void Clear()
        {
            this.Head = this.Tail = this.Count = 0;
            this.Version++;
        }

        /// <summary>
        /// Sets the capacity to Count.
        /// </summary>
        public void TrimToSize()
        {
            this.SetSize(this.Count);
        }                             
       
        /// <summary>
        /// Implementation of the ICollection.CopyTo function.
        /// </summary>
        /// <param name="array">Target array</param>
        /// <param name="index">Start-Index in target array</param>
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, "Must be at least zero!");
            }

            if (array.Length - index < this.Count)
            {
                throw new ArgumentException("Array was to small!");
            }

            if (array.Rank > 1)
            {
                throw new ArgumentException("Array was multidimensional!");
            }

            int i;
            for (i = 0; i < this.Count; i++)
            {
                array.SetValue(this[i], i + index);
            }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="index">The index.</param>
        public void CopyTo(Exception[] array, int index)
        {
            ((ICollection)this).CopyTo(array, index);
        }       

        /// <summary>
        /// Standard implementation.
        /// </summary>
        /// <returns>A DequeueEnumerator on the current dequeue</returns>
        public IEnumerator GetEnumerator()
        {
            return new RingBufferEnumerator(this, 0, this.Count - 1);
        }

        /// <summary>
        /// Standard implementation.
        /// </summary>
        /// <returns>A Dequeue with a shallow copy of this one.</returns>
        public object Clone()
        {
            RingBuffer ringBuffer = new RingBuffer(this, this.Capacity);
            ringBuffer.GrowthFactor = this.GrowthFactor;
            ringBuffer.Version = this.Version;
            return ringBuffer;
        }

        /// <summary>
        /// Sets the collections capacity to newSize
        /// </summary>
        /// <param name="newSize">the new collection size (must be >= Count)</param>
        protected void SetSize(int newSize)
        {
            if (newSize < this.Count)
            {
                throw new ArgumentException("New Size was smaller than Count!");
            }

            object[] newList = new object[newSize];
            int i;
            for (i = 0; i < this.Count; i++)
            {
                newList[i] = this[i];
            }

            this.Head = 0;
            this.Tail = this.Count;
            this.InnerList = newList;
            this.Version++;
        }

        /// <summary>
        /// Implementation of the ring buffer enumerator.
        /// </summary>
        internal class RingBufferEnumerator : IEnumerator
        {
            /// <summary>
            /// Stefan Seifert fragen
            /// </summary>
            private readonly RingBuffer deq;
            
            /// <summary>
            /// Stefan Seifert fragen
            /// </summary>
            private readonly int start;

            /// <summary>
            /// Stefan Seifert fragen
            /// </summary>
            private readonly int end;

            /// <summary>
            /// Stefan Seifert fragen
            /// </summary>
            private readonly int version;

            /// <summary>
            /// Stefan Seifert fragen
            /// </summary>
            private int pos;

            /// <summary>
            /// Stefan Seifert fragen
            /// </summary>
            private object current;
          
            /// <summary>
            /// Initializes a new instance of the <see cref="RingBufferEnumerator"/> class.
            /// </summary>
            /// <param name="usedRingBuffer">The used ring buffer.</param>
            /// <param name="startPosition">The start position.</param>
            /// <param name="endPosition">The end position.</param>
            public RingBufferEnumerator(RingBuffer usedRingBuffer, int startPosition, int endPosition)
            {
                this.deq = usedRingBuffer;
                this.version = this.deq.Version;
                this.start = startPosition;
                this.end = endPosition;
                this.pos = this.start - 1;
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <value></value>
            /// <returns>
            /// The current element in the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The enumerator is positioned before the first element of the collection or after the last element.
            /// </exception>
            public object Current
            {
                get
                {
                    if (this.pos < this.start || this.pos > this.end)
                    {
                        throw new InvalidOperationException("Enumerator not valid!");
                    }

                    return this.current;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public void Reset()
            {
                this.pos = this.start - 1;
                if (this.version != this.deq.Version)
                {
                    throw new InvalidOperationException("Collection was changed!");
                }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">
            /// The collection was modified after the enumerator was created.
            /// </exception>
            public bool MoveNext()
            {
                if (this.version != this.deq.Version)
                {
                    throw new InvalidOperationException("Collection was changed!");
                }

                this.pos++;
                if (this.pos > this.end)
                {
                    return false;
                }

                this.current = this.deq[this.pos];
                return true;
            }
        }
    }
}