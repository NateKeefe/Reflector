# Scribe Labs - Reflector
.NET Framework project to be referenced in Scribe Connectors, focusing on a framework for attributing .NET classes to easily generate Scribe metadata, with methods for interchanging data between these .NET classes and Scribe DataEntities/OperationInput (including hierarchal Scribe models).

This works well when paired with [Json.NET/Newtonsoft](https://github.com/JamesNK/Newtonsoft.Json), [XmlSerializer](https://docs.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer), [EDI.NET](https://github.com/indice-co/EDI.Net), or any other serializer. 

## Quick Start
Build this project and import its DLL it to your Scribe SDK/CDK project.

## Attributing Metadata:

```csharp
using Newtonsoft.Json;
using Scribe.Connector.Common.Reflection;
using Scribe.Connector.Common.Reflection.Actions;

namespace CDK.Entities.Person
{
    [Query] //Action Metadata
    [CreateWith] //Action Metadata
    [ObjectDefinition(Name = "Person")] //Object metadata
    public class Rootobject //.NET Class
    {
        [PropertyDefinition] //Property metadata
        public string firstname { get; set; } //.NET property
        
        [PropertyDefinition] //Property metadata for referenced object
        public Folder folder { get; set; }
        
        //Filters metadata
        [PropertyDefinition(UsedInQueryConstraint = true, UsedInQuerySelect = false, UsedInActionInput = false, UsedInActionOutput = false)] //Additional Property Metadata
        [JsonIgnore] //Newtonsoft attribute
        public string peopleId { get; set; }
        
        //Results metadata
        [PropertyDefinition(RequiredInActionInput = false, UsedInActionInput = false, UsedInQueryConstraint = false)]
        [JsonIgnore]
        public string location { get; set; }
    }
    
    [ObjectDefinition] //Hierarchal object Definition for  
    public class Folder
    {
        [PropertyDefinition]
        public string id { get; set; }
        [PropertyDefinition]
        public string value { get; set; }
    }
}
````
## Translations between OperationInput and DataEntities, and DataEntities to ExecuteQuery.

### For converting a Query example:

````csharp
  public static IEnumerable<DataEntity> QueryApi()
  {
    var stringResponse = httpClientMethod.MakeHttpRequest();
    var tData = JsonConvert.DeserializeObject<T>(stringResponse);
    return r.ToDataEntities(new[] { tData }, query.RootEntity);
  }
````

### For converting an Operation example:
````csharp
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
