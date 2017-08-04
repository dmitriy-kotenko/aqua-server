using System;
using System.Configuration;
using System.Data.SqlClient;

namespace AquaServer.Core.Data
{
	public class SqlExecutionHelper
	{
		private readonly string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

		public void ExecuteNonQuery(SqlCommandInfo sqlCommandInfo)
		{
			executeCommand(sqlCommandInfo, realCommand => realCommand.ExecuteNonQuery());
		}

		public T ExecuteScalar<T>(SqlCommandInfo sqlCommandInfo)
		{
			T result = default(T);
			executeCommand(sqlCommandInfo, realCommand =>
				{
					object executeResult = realCommand.ExecuteScalar();
					if (executeResult == null || executeResult is DBNull)
					{
						return;
					}

					result = (T)executeResult;
				});

			return result;
		}

		public void ExecuteReader(SqlCommandInfo sqlCommandInfo, Action<SqlDataReader> processRowAction)
		{
			executeCommand(sqlCommandInfo, realCommand =>
				{
					using (SqlDataReader reader = realCommand.ExecuteReader())
					{
						while (reader.Read())
						{
							processRowAction(reader);
						}
					}
				});
		}

		private void executeCommand(SqlCommandInfo sqlCommandInfo, Action<SqlCommand> executeAction)
		{
			using (var sqlConnection = new SqlConnection(_connectionString))
			{
				sqlConnection.Open();
				SqlCommand realCommand = sqlCommandInfo.CreateRealCommand(sqlConnection);

				executeAction(realCommand);
			}
		}
	}
}
