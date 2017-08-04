using System;
using System.Collections.Generic;

namespace AquaServer.Core.Data
{
	public class SqlCommandText : ICloneable
	{
		public string CommandText { get; set; }

		public Dictionary<string, object> Parameters { get; }

		public bool IsEmpty => string.IsNullOrEmpty(CommandText);

		public static SqlCommandText Empty => new SqlCommandText();

		public SqlCommandText()
		{
			Parameters = new Dictionary<string, object>();
		}

		public SqlCommandText(string commandText) : this()
		{
			CommandText = commandText;
		}

		public void AddParameters(SqlCommandText source)
		{
			foreach (KeyValuePair<string, object> parameter in source.Parameters)
			{
				Parameters.Add(parameter.Key, parameter.Value);
			}
		}

		public SqlCommandText Concat(SqlCommandText other)
		{
			if (IsEmpty)
			{
				return (SqlCommandText)other.Clone();
			}

			if (other.IsEmpty)
			{
				return (SqlCommandText)Clone();
			}

			var concatenatedCommandText = new SqlCommandText($"({CommandText}) AND ({other.CommandText})");
			concatenatedCommandText.AddParameters(this);
			concatenatedCommandText.AddParameters(other);

			return concatenatedCommandText;
		}

		public object Clone()
		{
			var clone = new SqlCommandText(CommandText);
			clone.AddParameters(this);
			return clone;
		}
	}
}
