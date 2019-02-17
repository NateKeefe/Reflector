using System;

namespace Scribe.Connector.Common.Interfaces
{
    public class IndexedResult<TValue> : IIndexedResult<TValue>
    {
        private readonly int index;

        private readonly TValue value;

        private readonly Exception error;

        private readonly bool hasError;

        private readonly bool isFatalError;

        private readonly int objectsEffected;

        public IndexedResult(IIndexed<TValue> indexed)
            : this(indexed, new Result())
        {
        }

        public IndexedResult(IIndexed<TValue> indexed, IResult result)
        {
            this.index = indexed.Index;
            this.value = indexed.Value;

            this.error = result.Error;
            this.hasError = result.HasError;
            this.isFatalError = result.IsFatalError;
            this.objectsEffected = result.ObjectsEffected;
        }

        public int Index
        {
            get
            {
                return this.index;
            }
        }

        public Exception Error
        {
            get
            {
                return this.error;
            }
        }

        public bool HasError
        {
            get
            {
                return this.hasError;
            }
        }

        public bool IsFatalError
        {
            get
            {
                return this.isFatalError;
            }
        }

        public int ObjectsEffected
        {
            get
            {
                return this.objectsEffected;
            }
        }

        public TValue Value
        {
            get
            {
                return this.value;
            }
        }
    }
}