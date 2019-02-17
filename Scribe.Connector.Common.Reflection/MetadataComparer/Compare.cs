namespace MetadataComparer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Scribe.Core.ConnectorApi.Metadata;

    public static class Compare
    {
        public static IDictionary<string, Result> CompareActions(
            IEnumerable<IActionDefinition> old,
            IEnumerable<IActionDefinition> newActions, bool strict = false)
        {
            var d = new Dictionary<string, Result>();

            foreach (var o in old)
            {
                var n = newActions.FirstOrDefault(a => a.FullName == o.FullName);
                if (n == null)
                {
                    d.Add(
                        o.FullName,
                        Result.Bad(
                            $"Action {o.FullName} does not have a corresponding ActionDefintiion in the new Actions."));
                    continue;
                }

                var bad = false;
                StringBuilder sb = new StringBuilder();
                if (o.SupportsBulk != n.SupportsBulk)
                {
                    bad = true;
                    sb.AppendLine($"Supports bulk has changed from {o.SupportsBulk} to {n.SupportsBulk}.");
                }

                if (o.SupportsConstraints != n.SupportsConstraints && strict)
                {
                    bad = true;
                    sb.AppendLine(
                        $"SupportsConstraints has changed from {o.SupportsConstraints} to {n.SupportsConstraints}.");
                }
                if (o.SupportsInput != n.SupportsInput)
                {
                    bad = true;
                    sb.AppendLine($"SupportsInput has changed from {o.SupportsInput} to {n.SupportsInput}.");
                }
                if (o.SupportsLookupConditions != n.SupportsLookupConditions && strict)
                {
                    bad = true;
                    sb.AppendLine(
                        $"SupportsLookupConditions has changed from {o.SupportsLookupConditions} to {n.SupportsLookupConditions}.");
                }
                if (o.SupportsMultipleRecordOperations != n.SupportsMultipleRecordOperations)
                {
                    bad = true;
                    sb.AppendLine(
                        $"SupportsMultipleRecordOperations has changed from {o.SupportsMultipleRecordOperations} to {n.SupportsMultipleRecordOperations}.");
                }
                if (o.SupportsRelations != n.SupportsRelations && strict)
                {
                    bad = true;
                    sb.AppendLine($"Supports bulk has changed from {o.SupportsRelations} to {n.SupportsRelations}.");
                }
                if (o.SupportsSequences != n.SupportsSequences && strict)
                {
                    bad = true;
                    sb.AppendLine($"Supports bulk has changed from {o.SupportsSequences} to {n.SupportsSequences}.");
                }
                if (o.Name != n.Name)
                {
                    bad = true;
                    sb.AppendLine($"Name has changed from {o.Name} to {n.Name}.");
                }
                if (o.FullName != n.FullName)
                {
                    bad = true;
                    sb.AppendLine($"FullName has changed from {o.FullName} to {n.FullName}.");
                }
                if (o.Description != n.Description && strict)
                {
                    bad = true;
                    sb.AppendLine($"Description has changed from {o.Description} to {n.Description}.");
                }

                if (o.KnownActionType != n.KnownActionType)
                {
                    bad = true;
                    sb.AppendLine($"KnownActionType has changed from {o.KnownActionType} to {n.KnownActionType}.");
                }

                d.Add(o.Name, bad ? Result.Bad(sb.ToString()) : Result.Good());
            }

            var oldNames = old.Select(oa => oa.FullName).ToList();
            foreach (var n in newActions.Where(na => !oldNames.Contains(na.FullName)))
            {
                d.Add(n.FullName, Result.Bad($"The action {n.FullName} is new."));
            }

            return d;
        }

        public static IDictionary<string, Result> CompareObjects(
            IEnumerable<IObjectDefinition> oldObjects,
            IEnumerable<IObjectDefinition> newObjects)
        {
            var oldD = oldObjects.ToDictionary(od => od.FullName, od => od);
            var newD = newObjects.ToDictionary(od => od.FullName, od => od);
            var d = new Dictionary<string, Result>();

            foreach (var okv in oldD)
            {
                if (newD.TryGetValue(okv.Key, out var n))
                {
                    var o = okv.Value;
                    var bad = false;
                    StringBuilder sb = new StringBuilder();
                    if (o.Name != o.Name)
                    {
                        bad = true;
                        sb.AppendLine($"Name has changed from {o.Name} to {n.Name}.");
                    }
                    if (o.Description != o.Description)
                    {
                        bad = true;
                        sb.AppendLine($"Description has changed from {o.Description} to {n.Description}.");
                    }
                    if (o.Hidden != o.Hidden)
                    {
                        bad = true;
                        sb.AppendLine($"Hidden has changed from {o.Hidden} to {n.Hidden}.");
                    }
                    if (StructuralComparisons.StructuralComparer.Compare(
                            o.SupportedActionFullNames.ToArray(),
                            n.SupportedActionFullNames.ToArray()) != 0)
                    {
                        bad = true;
                        sb.AppendLine(
                            $"o.SupportedActionFullNames has changed from {string.Join(", ", o.SupportedActionFullNames)} to {string.Join(", ", n.SupportedActionFullNames)}.");
                    }
                    if (o.RelationshipDefinitions?.Count != n.RelationshipDefinitions?.Count)
                    {
                        bad = true;
                        sb.AppendLine(
                            $"The number of Relationship definitions has changed from {o.RelationshipDefinitions?.Count} to {n.RelationshipDefinitions?.Count}.");
                    }
                    if (o.PropertyDefinitions?.Count != n.PropertyDefinitions?.Count)
                    {
                        bad = true;
                        sb.AppendLine(
                            $"The number of Property Definitions has changed from {o.PropertyDefinitions?.Count} to {n.PropertyDefinitions?.Count}.");
                    }

                    d.Add(o.FullName, bad ? Result.Bad(sb.ToString()) : Result.Good());

                }
                else
                {
                    d.Add(
                        okv.Key,
                        Result.Bad(
                            $"Object {okv.Key} does not have a corresponding ObjectDefintiion in the new Objects."));
                }
            }

            foreach (var n in newD.Keys.Where(name => !oldD.ContainsKey(name)))
            {
                d.Add(n, Result.Bad($"The Object Definition {n} is new."));
            }

            return d;
        }

        public static IDictionary<string, IDictionary<string, Result>> CompareObjectProperties(
            IEnumerable<IObjectDefinition> oldObjects,
            IEnumerable<IObjectDefinition> newObjects, bool strict = false)
        {
            var oldD = oldObjects.ToDictionary(od => od.FullName, od => od);
            var newD = newObjects.ToDictionary(od => od.FullName, od => od);
            var d = new Dictionary<string, IDictionary<string, Result>>();

            foreach (var n in oldD.Keys.Where(name => !newD.ContainsKey(name)))
            {
                d.Add(n, CompareProperties(oldD[n].PropertyDefinitions, newD[n].PropertyDefinitions, strict));
            }

            return d;
        }

        private static IDictionary<string, Result> CompareProperties(
            IEnumerable<IPropertyDefinition> oldP,
            IEnumerable<IPropertyDefinition> newP, bool strict)
        {
            var oldD = oldP.ToDictionary(
                propertyDefinition => propertyDefinition.FullName,
                propertyDefinition => propertyDefinition);
            var newD = newP.ToDictionary(
                propertyDefinition => propertyDefinition.FullName,
                propertyDefinition => propertyDefinition);
            var d = new Dictionary<string, Result>();

            foreach (var pkv in oldD)
            {
                if (newD.TryGetValue(pkv.Key, out var n))
                {
                    var o = pkv.Value;
                    var bad = false;
                    StringBuilder sb = new StringBuilder();


                    if (o.Name != n.Name)
                    {
                        bad = true;
                        sb.AppendLine($"Name has changed from {o.Name} to {n.Name}.");
                    }

                    if (o.Description != n.Description)
                    {
                        bad = true;
                        sb.AppendLine($"Description has changed from {o.Description} to {n.Description}.");
                    }
                    if (o.PresentationType != n.PresentationType && strict)
                    {
                        bad = true;
                        sb.AppendLine($"PresentationType has changed from {o.PresentationType} to {n.PresentationType}.");
                    }
                    if (o.PropertyType != n.PropertyType)
                    {
                        bad = true;
                        sb.AppendLine($"Description has changed from {o.PropertyType} to {n.PropertyType}.");
                    }
                    if (o.IsPrimaryKey != n.IsPrimaryKey)
                    {
                        bad = true;
                        sb.AppendLine($"IsPrimaryKey has changed from {o.IsPrimaryKey} to {n.IsPrimaryKey}.");
                    }
                    if (o.MaxOccurs != n.MaxOccurs)
                    {
                        bad = true;
                        sb.AppendLine($"MaxOccurs has changed from {o.MaxOccurs} to {n.MaxOccurs}.");
                    }
                    if (o.MinOccurs != n.MinOccurs && strict)
                    {
                        bad = true;
                        sb.AppendLine($"MinOccurs has changed from {o.MinOccurs} to {n.MinOccurs}.");
                    }
                    if (o.Nullable != n.Nullable)
                    {
                        bad = true;
                        sb.AppendLine($"Nullable has changed from {o.Nullable} to {n.Nullable}.");
                    }
                    if (o.NumericPrecision != n.NumericPrecision && strict)
                    {
                        bad = true;
                        sb.AppendLine($"NumericPrecision has changed from {o.NumericPrecision} to {n.NumericPrecision}.");
                    }
                    if (o.NumericScale != n.NumericScale)
                    {
                        bad = true;
                        sb.AppendLine($"NumericScale has changed from {o.NumericScale} to {n.NumericScale}.");
                    }
                    if (o.Size != n.Size)
                    {
                        bad = true;
                        sb.AppendLine($"Size has changed from {o.Size} to {n.Size}.");
                    }
                    if (o.RequiredInActionInput != n.RequiredInActionInput)
                    {
                        bad = true;
                        sb.AppendLine($"RequiredInActionInput has changed from {o.RequiredInActionInput} to {n.RequiredInActionInput}.");
                    }
                    if (o.UsedInQuerySelect != n.UsedInQuerySelect)
                    {
                        bad = true;
                        sb.AppendLine($"UsedInQuerySelect has changed from {o.UsedInQuerySelect} to {n.UsedInQuerySelect}.");
                    }
                    if (o.UsedInQueryConstraint != n.UsedInQueryConstraint)
                    {
                        bad = true;
                        sb.AppendLine($"UsedInQueryConstraint has changed from {o.UsedInQueryConstraint} to {n.UsedInQueryConstraint}.");
                    }
                    if (o.UsedInQuerySequence != n.UsedInQuerySequence)
                    {
                        bad = true;
                        sb.AppendLine($"UsedInQuerySequence has changed from {o.UsedInQuerySequence} to {n.UsedInQuerySequence}.");
                    }
                    if (o.UsedInActionInput != n.UsedInActionInput)
                    {
                        bad = true;
                        sb.AppendLine($"UsedInActionInput has changed from {o.UsedInActionInput} to {n.UsedInActionInput}.");
                    }
                    if (o.UsedInLookupCondition != n.UsedInLookupCondition)
                    {
                        bad = true;
                        sb.AppendLine($"UsedInLookupCondition has changed from {o.UsedInLookupCondition} to {n.UsedInLookupCondition}.");
                    }
                    if (o.UsedInActionOutput != n.UsedInActionOutput)
                    {
                        bad = true;
                        sb.AppendLine($"UsedInActionOutput has changed from {o.UsedInActionOutput} to {n.UsedInActionOutput}.");
                    }


                    d.Add(o.FullName, bad ? Result.Bad(sb.ToString()) : Result.Good());
                }
                else
                {
                    d.Add(
                        pkv.Key,
                        Result.Bad(
                            $"Object {pkv.Key} does not have a corresponding ObjectDefintiion in the new Objects."));
                }
            }

            foreach (var n in newD.Keys.Where(name => !oldD.ContainsKey(name)))
            {
                d.Add(n, Result.Bad($"The Object Definition {n} is new."));
            }

            return d;
        }
    }
}