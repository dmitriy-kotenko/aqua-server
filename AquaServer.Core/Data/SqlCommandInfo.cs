using System.Data.SqlClient;

namespace AquaServer.Core.Data
{
	public class SqlCommandInfo : SqlCommandText
	{
		public SqlCommandInfo(string commandText) : base(commandText)
		{
		}

		public SqlCommand CreateRealCommand(SqlConnection sqlConnection)
		{
			SqlCommand realCommand = sqlConnection.CreateCommand();
			realCommand.CommandText = CommandText;

			foreach (var parameter in Parameters)
			{
				realCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
			}

			return realCommand;
		}
	}
}
