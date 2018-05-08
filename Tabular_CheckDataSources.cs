using Microsoft.AnalysisServices.Tabular;
/*Installation instructions: https://docs.microsoft.com/en-us/sql/analysis-services/tabular-model-programming-compatibility-level-1200/install-distribute-and-reference-the-tabular-object-model?view=sql-analysis-services-2017
*/

string serverName = Dts.Variables["$Project::SSAS_ServerName"].Value.ToString(); // SSIS project parameter
string connectionString = "DataSource=" + serverName;
string oldServerName = "";
int counter = 0;

using (Server server = new Server())
{
	server.Connect(connectionString);

	// Loop over all databases
	foreach(Database db in server.Databases)
	{
		counter = 0;
		foreach (ProviderDataSource ds in db.Model.DataSources) // use ProviderDataSource and not the DataSource object
		{
			// only check the connections for specific connection. Remove if not necessary
			if(ds.Name == "conn1" || ds.Name == "conn2")
			{
				if(!ds.ConnectionString.ToUpper().Contains(serverName.ToUpper()))
				{
					string conn = ds.ConnectionString;
					// Example of connection string: "Provider=SQLNCLI11;Data Source=localhost;Integrated..."
					// We need to find the old servername. Split up the connection string using ; as delimiter, find the data source and extract the servername.
					char delimiter = ';';
					string[] args = conn.Split(delimiter);
						
					foreach (string s in args)
					{
						// we need the connection property that starts with "Data Source="
						if(s.StartsWith("Data Source="))
						{
							oldServerName = s.Replace("Data Source=", ""); // fetch the server name by getting rid of everything else in the property
						}
					}
					ds.ConnectionString = conn.Replace(oldServerName, serverName);
					counter++;
				}
			}
		}
		if (counter > 0) // one or more connection strings have been updated
		{
			db.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull); // this only seems to work if ExpandFull is used
		}
	}
}