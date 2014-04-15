using System;

namespace BindingAlias {
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public class BindingAlias : Attribute {
		public BindingAlias() { }
		public BindingAlias(params string[] name) { Names = name; }

		public string[] Names { get; set; }
	}
}