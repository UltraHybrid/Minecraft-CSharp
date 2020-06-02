using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using tmp.Interfaces;

namespace tmp
{
    public class ChunkManager<TSource, TResult>
    {
        private readonly Queue<TSource> futureChunks;
        private readonly object inputLock = new object();
        private readonly Queue<TResult> output;
        private readonly object outputLock = new object();
        private readonly Func<TSource, TResult> function;
        private bool isActive;

        public bool IsEmpty { get; private set; }

        public ChunkManager(Func<TSource, TResult> function)
        {
            this.function = function;
            futureChunks = new Queue<TSource>();
            output = new Queue<TResult>();
            IsEmpty = true;
        }

        public void Push(TSource position)
        {
            lock (inputLock)
            {
                futureChunks.Enqueue(position);
            }
        }

        public TResult Pop()
        {
            TResult result;
            lock (outputLock)
            {
                if (output.Count == 0)
                    throw new InvalidOperationException("Очередь пуста");
                result = output.Dequeue();
                if (output.Count == 0)
                    IsEmpty = true;
            }

            return result;
        }

        public void Start()
        {
            isActive = true;
            new Thread(Loop).Start();
        }

        private void Loop()
        {
            while (isActive)
            {
                if (futureChunks.Count != 0)
                {
                    TSource current;
                    lock (inputLock)
                    {
                        current = futureChunks.Dequeue();
                    }

                    var result = function(current);
                    lock (outputLock)
                    {
                        output.Enqueue(result);
                        IsEmpty = false;
                    }
                }
            }
        }

        public void Stop()
        {
            isActive = false;
        }
    }
}