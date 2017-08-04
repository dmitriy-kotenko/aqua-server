using System;
using System.Data;

namespace AquaServer.Core.Data
{
	public static class IDataRecordExtensions
	{
		public static T GetFieldValue<T>(this IDataRecord record, string fieldName)
		{
			int ordinal = record.GetOrdinal(fieldName);

			if (record.IsDBNull(ordinal))
			{
				return default(T);
			}

			Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

			if (type.IsEnum)
			{
				return (T)Enum.ToObject(type, record.GetValue(ordinal));
			}

			return (T)record.GetValue(ordinal);
		}
	}
}
