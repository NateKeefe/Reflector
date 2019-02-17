using System;

namespace Scribe.Connector.Common.Interfaces
{
    public class Result : IResult
    {
        private readonly Exception error;

        private readonly bool hasError;

        private readonly bool isFatalError;

        private readonly int objectsEffected;

        public Result()
        {
        }

        public Result(Exception ex)
        {
            this.error = ex;
            this.hasError = ex != null;
            this.objectsEffected = 0;
            this.isFatalError = false;
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
    }
}