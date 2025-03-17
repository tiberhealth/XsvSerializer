namespace Tiberhealth.XsvSerializer
{
    internal class Mapper<TType> where TType : class, new()
    {
        private readonly ConcurrentDictionary<int, PropertyInfo> _map = new ConcurrentDictionary<int, PropertyInfo>();

        public Mapper(XsvReader reader)
        {
            var headerLine = reader.GetNextLine();
            this.MapXsv(headerLine.ToArray());
        }

        private void MapXsv(string[] xsvHeader)
        {
            var properties = typeof(TType).GetProperties();

            for (var idx=0; idx < xsvHeader.Length; idx++)
            {
                var field = xsvHeader[idx];
                if (this.GetFindProperty(properties, field, out var property))
                {
                    this._map.TryAdd(idx, property);
                    continue;
                }

                throw new FieldNotDefinedException(field);
            }
        }

        private bool GetFindProperty(PropertyInfo[] properties, string name, out PropertyInfo property)
        {
            property = properties.SingleOrDefault(
                item =>
                    item.ObjectColumnName(() => item.Name).Equals(name, System.StringComparison.OrdinalIgnoreCase)
                );

            return property != null;
        }

        internal TType MapObject(IEnumerable<string> xsvLine, out IEnumerable<string> warnings)
        {
            var warningList = new List<string>(); 
            var returnObj = new TType();
            var xsvValues = xsvLine.ToArray();


            foreach (var key in this._map.Keys)
            {
                var property = this._map[key];
                var propType = property.PropertyType;

                object typedValue = default;

                try
                {
                    if (propType.IsGenericType)
                    {
                        var nullableType = Nullable.GetUnderlyingType(propType);
                        if (nullableType == null) throw new NotSupportedException("Generic type definitions are not support");

                        propType = nullableType;
                    }

                    typedValue = string.IsNullOrWhiteSpace(xsvValues[key]) ? null : Convert.ChangeType(xsvValues[key].Trim(), propType);
                }
                catch (Exception ex)
                {
                    warningList.Add($"Unable to add value at position {key} to field {property.Name} possible incompatibility of types. Error message: {ex.Message}");
                }

                property.SetValue(returnObj, typedValue);
            }

            warnings = warningList;
            return returnObj;
        }
    }
}
