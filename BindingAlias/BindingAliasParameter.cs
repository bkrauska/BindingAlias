namespace BindingAlias {
	public class BindingAliasParameter {
		public BindingAliasParameter(string parameterName, params string[] aliases) {
			ParameterName = parameterName;
			BindingAliases = aliases;
		}

		public string ParameterName { get; set; }
		public string[] BindingAliases { get; set; }
	}
}