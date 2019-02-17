namespace MetadataComparer
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FullResults
    {
        public IDictionary<string, Result> ActionResults;
        public IDictionary<string, Result> ObjectResults;
        public IDictionary<string, IDictionary<string, Result>> PropertyResults;

        public FullResults(Metadata old, Metadata newM)
        {
            this.ActionResults = Compare.CompareActions(old.Actions, newM.Actions);
            this.ObjectResults = Compare.CompareObjects(old.Objects, newM.Objects);
            this.PropertyResults = Compare.CompareObjectProperties(old.Objects, newM.Objects);
        }

        public bool AreSame()
        {
            return this.ActionResults.Values.All(r => r.IsSuccess)
                   && this.ObjectResults.Values.All(r => r.IsSuccess)
                   && this.PropertyResults.Values.SelectMany(x => x.Values).All(r => r.IsSuccess);
        }

        public void PrintActions(StringBuilder sb)
        {
            sb.AppendLine("Action comparisons");
            sb.AppendLine("------");
            sb.AppendLine();

            if (this.ActionResults.Values.All(r => r.IsSuccess))
            {
                var s = this.ActionResults.Count == 1 ? "s" : string.Empty;
                sb.AppendLine($"There are {this.ActionResults.Count} action{s} that are the same.");
            }
            else
            {
                var success = this.ActionResults.Where(a => a.Value.IsSuccess).ToList();
                var failures = this.ActionResults.Where(a => !a.Value.IsSuccess).ToList();
                if (success.Count == 0)
                {
                }
                else if (success.Count == 1)
                {
                    sb.AppendLine($"{success.First().Key} is the same.");
                    sb.AppendLine();
                }
                else
                {
                    var sameActions = string.Join(", ", success.Select(x => x.Key));
                    sb.AppendLine($"{sameActions} are the same.");
                    sb.AppendLine();
                }

                foreach (var failure in failures)
                {
                    sb.AppendLine($"{failure.Key} failed with the following:");
                    sb.AppendLine(failure.Value.FailureCondition);
                    sb.AppendLine();
                }

            }
        }

        public void PrintObjects(StringBuilder sb)
        {
            sb.AppendLine("Object comparisons");
            sb.AppendLine("------");
            sb.AppendLine();

            if (this.ObjectResults.Values.All(r => r.IsSuccess))
            {
                var s = this.ObjectResults.Count == 1 ? "s" : string.Empty;
                var are = this.ObjectResults.Count == 1 ? "is" : "are";
                sb.AppendLine($"There{are} {this.ObjectResults.Count} object{s} that {are} the same.");
            }
            else
            {
                var osuccess = this.ObjectResults.Where(a => a.Value.IsSuccess).ToList();
                var ofailures = this.ObjectResults.Where(a => !a.Value.IsSuccess).ToList();
                if (osuccess.Count == 0)
                {
                }
                else if (osuccess.Count == 1)
                {
                    sb.AppendLine($"{osuccess.First().Key} is the same.");
                    sb.AppendLine();
                }
                else
                {
                    var sameobjects = string.Join(", ", osuccess.Select(x => x.Key));
                    sb.AppendLine($"{sameobjects} are the same.");
                    sb.AppendLine();
                }

                foreach (var failure in ofailures)
                {
                    sb.AppendLine($"{failure.Key} failed with the following:");
                    sb.AppendLine(failure.Value.FailureCondition);
                    sb.AppendLine();
                }
            }

        }

        private void PrintProperties(StringBuilder sb)
        {
            sb.AppendLine("Property comparisons");
            sb.AppendLine("------");
            sb.AppendLine();

            foreach (var obj in this.PropertyResults)
            {
                var objName = obj.Key;
                var props = obj.Value;
                if (props.Values.All(r => r.IsSuccess))
                {
                    sb.AppendLine($"{obj.Key} properties are all the same.");
                }
                else
                {
                    var osuccess = props.Where(a => a.Value.IsSuccess).ToList();
                    var ofailures = props.Where(a => !a.Value.IsSuccess).ToList();
                    if (osuccess.Count == 0)
                    {
                    }
                    else if (osuccess.Count == 1)
                    {
                        sb.AppendLine($"{osuccess.First().Key} is the same.");
                        sb.AppendLine();
                    }
                    else
                    {
                        var sameobjects = string.Join(", ", osuccess.Select(x => x.Key));
                        sb.AppendLine($"{sameobjects} are the same.");
                        sb.AppendLine();
                    }

                    foreach (var failure in ofailures)
                    {
                        sb.AppendLine($"{failure.Key} failed with the following:");
                        sb.AppendLine(failure.Value.FailureCondition);
                        sb.AppendLine();
                    }
                }
            }
        }

        public string Print()
        {
            if (this.AreSame()) return "Same";
            StringBuilder sb = new StringBuilder();

            this.PrintActions(sb);

            this.PrintObjects(sb);

            this.PrintProperties(sb);

            return sb.ToString();

        }


    }
}