namespace DbConnectionTools
{
    public sealed class Command
    {
        internal string? Query { get; private set; }
        internal bool IsStoredProcedure { get; private set; }
        internal Dictionary<string, object>? Parameters { get; private set; }

        public Command(string query, bool isStoredProcedure = false)
        {
            Query = query;
            IsStoredProcedure = isStoredProcedure;
            Parameters = new Dictionary<string, object>();
        }
        public void AddParameters(string parameterName, object value)
        {
            Parameters.Add(parameterName, value ?? DBNull.Value);
        }
    }
}
