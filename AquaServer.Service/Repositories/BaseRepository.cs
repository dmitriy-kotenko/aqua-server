using AquaServer.Core.Data;

namespace AquaServer.Service.Repositories
{
	public class BaseRepository
	{
		protected readonly SqlExecutionHelper SqlExecutionHelper = new SqlExecutionHelper();
	}
}
