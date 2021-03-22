using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSatisfactoryCalculator.code.Production
{
	public class ProcessedPlan
	{
		private readonly HashSet<HashSet<Connection>> normalConnectionGroups;
		private readonly HashSet<Connection> abnormalConnections;

		public ProcessedPlan(Plan plan)
		{
			normalConnectionGroups = new HashSet<HashSet<Connection>>();
			abnormalConnections = new HashSet<Connection>();
		MainLoop:
			foreach (Connection connection in plan)
			{
				if (connection.Type == Connection.OverallConnectionType.NORMAL)
				{
					foreach (HashSet<Connection> connections in normalConnectionGroups)
					{
						if (connections.First().IsConnectedNormallyTo(connection))
						{
							if (connections.Add(connection))
							{
								goto MainLoop;
							}
						}
					}
				}
				else
				{
					abnormalConnections.Add(connection);
				}
			}
		}
	}
}
