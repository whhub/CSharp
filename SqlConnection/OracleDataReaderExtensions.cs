using System;
using Oracle.ManagedDataAccess.Client;

namespace SqlConnection
{
    public static class OracleDataReaderExtensions
    {
        private static bool IsNullOrEmpty(object obj)
        {
            if (obj == null)
            {
                return true;
            }
            if (obj is string)
            {
                return string.IsNullOrWhiteSpace(obj.ToString());
            }
            return false;
        }

        public static T Get<T>(this OracleDataReader reader, int i)
        {
            try
            {
                if (typeof(T) == typeof(string))
                {
                    return reader.IsDBNull(i) ? default(T) : (T)(reader.GetString(i) as object);
                }
                var isNullable = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>);
                if (reader.IsDBNull(i))
                {
                    if (!isNullable)
                    {
                        throw new Exception("value IsDBNull.");
                    }
                    return default(T);
                }
                else
                {
                    var obj = reader.GetValue(i);
                    if (IsNullOrEmpty(obj))
                    {
                        if (!isNullable)
                        {
                            throw new Exception("value is empty.");
                        }
                        return default(T);
                    }
                    var type = isNullable ? Nullable.GetUnderlyingType(typeof(T)) : typeof(T);
                    return (T)Convert.ChangeType(obj, type);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
