using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scribe.Connector.Common.Reflection;

namespace ReflectionTestModels
{
    /// <summary>
    /// Reproducing the Bug in Core requires a Property that is ObjDef.
    /// The bug is that the DataEntity passed in has the PropertyName as the ObjectDef FullName
    /// </summary>
    [ObjectDefinition]
    public class CoreBugWorkaround
    {
        [PropertyDefinition]
        public string Str { get; set; }

        [PropertyDefinition]
        public CoreBugWorkaround PropName { get; set; }
    }
}
