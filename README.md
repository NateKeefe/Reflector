# Reflector
### Allows for attributing .NET classes and Properties to create Scribe metadata. 

Attributing Metadata example:

```dotnet
using Newtonsoft.Json;
using Scribe.Connector.Common.Reflection;
using Scribe.Connector.Common.Reflection.Actions;

namespace CDK.Entities.Person
{
    [Query]
    [CreateWith]
    [ObjectDefinition(Name = "Person")]
    public class Rootobject
    {
        [PropertyDefinition]
        public string firstname { get; set; }
        [PropertyDefinition]
        public Folder folder { get; set; }
        [PropertyDefinition]
        public Link[] links { get; set; }
        [PropertyDefinition]
        public string email { get; set; }
        [PropertyDefinition]
        public string lastname { get; set; }

        //Filters
        [PropertyDefinition(UsedInQueryConstraint = true, UsedInQuerySelect = false, UsedInActionInput = false, UsedInActionOutput = false)]
        [JsonIgnore]
        public string peopleId { get; set; }
        //Results
        [PropertyDefinition(RequiredInActionInput = false, UsedInActionInput = false, UsedInQueryConstraint = false)]
        [JsonIgnore]
        public string location { get; set; }
    }
    
    [ObjectDefinition]
    public class Folder
    {
        [PropertyDefinition]
        public string id { get; set; }
        [PropertyDefinition]
        public string formattedvalue { get; set; }
        [PropertyDefinition]
        public string value { get; set; }
    }
}
````
### Also translates between OperationInput and DataEntities, and DataEntities to ExecuteQuery.

For converting a Query example:

````dotnet
  public static IEnumerable<DataEntity> QueryApi()
  {
    var stringResponse = httpClientMethod.MakeHttpRequest();
    var tData = JsonConvert.DeserializeObject<T>(stringResponse);
    return r.ToDataEntities(new[] { tData }, query.RootEntity);
  }
````

For converting an Operation example:
````dotnet
  public OperationResult Create(DataEntity dataEntity)
  {
    var person = ToScribeModel<Entities.Person.Rootobject>(dataEntity);
  }
  
  private T ToScribeModel<T>(DataEntity input) where T : new()
  {
    T scribeModel;
    try
      {
        scribeModel = reflector.To<T>(input);
      }
    catch (Exception e)
    {
      throw new ArgumentException("Error translating from DataEntity to ScribeModel: " + e.Message, e);
    }
    return scribeModel;
   }
````
