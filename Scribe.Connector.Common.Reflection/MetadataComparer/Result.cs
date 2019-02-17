namespace MetadataComparer
{
    public class Result
    {
        public static Result Good()
        {
            return new Result();
        }

        public static Result Bad(string condition)
        {
            return new Result(condition);
        }

        private Result()
        {
            this.IsSuccess = true;
            this.FailureCondition = null;
        }

        private Result(string condition)
        {
            this.IsSuccess = false;
            this.FailureCondition = condition;
        }

        public bool IsSuccess { get; }

        public string FailureCondition { get; }
    }
}