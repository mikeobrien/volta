using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;

namespace Volta.Core.Infrastructure.Framework.Data
{
    public class JetQuery : IDisposable
    {
        private const string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0}";
        private readonly Lazy<OleDbConnection> _connection;
        private readonly Lazy<IList<string>> _tables;

        public JetQuery(string path)
        {
            _connection = new Lazy<OleDbConnection>(() => {
                var connection = new OleDbConnection(string.Format(ConnectionString, path));
                connection.Open();
                return connection;
            });
            _tables = new Lazy<IList<string>>(GetTables);
        }

        public IEnumerable<T> Table<T>() where T : new()
        {
            Func<string, string, bool> namesMatch = (x, y) => x.Replace("_", "").Replace("/", "").
                Equals(y, StringComparison.OrdinalIgnoreCase);
            var table = _tables.Value.FirstOrDefault(x => namesMatch(x.Replace("Table", ""), typeof(T).Name)) ?? typeof(T).Name;
            using (var reader = new OleDbCommand("SELECT * FROM " + table, _connection.Value).ExecuteReader())
            {
                var mapping = reader.GetSchemaTable().Rows.Cast<DataRow>()
                    .Select(x => (string)x["ColumnName"])
                    .Select(x => new { Column = x, Property = 
                        typeof (T).GetProperties().FirstOrDefault(y => namesMatch(x, y.Name) ||
                            namesMatch(y.Name, x.Match(@"^(.*?)\d+$"))) })
                    .Where(x => x.Property != null)
                    .OrderBy(x => x.Column).ToList();
                while (reader.Read())
                {
                    var entity = new T();
                    mapping.ForEach(x =>
                        x.Property.SetValue(entity,
                            x.Property.PropertyType.IsArray ?
                                new ArrayList((Array)(x.Property.GetValue(entity, null) ?? 
                                        Activator.CreateInstance(x.Property.PropertyType, 0))) { reader[x.Column] }
                                        .ToArray(x.Property.PropertyType.GetElementType()) :
                                reader[x.Column],
                            null));
                    yield return entity;
                }
            }
        }

        private IList<string> GetTables()
        {
            return _connection.Value.GetSchema("Tables").Rows.Cast<DataRow>().Select(x => (string)x["table_name"]).ToList();
        }

        public void Dispose()
        {
            if (_connection.IsValueCreated) _connection.Value.Dispose();
        }
    }
}