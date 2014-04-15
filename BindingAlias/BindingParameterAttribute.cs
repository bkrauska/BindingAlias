using System.Web.Mvc;

namespace BindingAlias {
	public class BindingAliasParameterAttribute : AuthorizeAttribute {
		private readonly string _parameterName;
		private readonly string _aliasName;

		public BindingAliasParameterAttribute(string parameterName, string aliasName) {
			_parameterName = parameterName;
			_aliasName = aliasName;
		}

		public override void OnAuthorization(AuthorizationContext filterContext) {
			IValueProvider valueProvider = filterContext.Controller.ValueProvider;
			var provider = new ValueProviderCollection();
			provider.Add(valueProvider);

			var bindingAliasProvider = new ValueProvider(valueProvider);
			bindingAliasProvider.AddBindingAlias(new BindingAliasParameter(_parameterName, _aliasName));

			provider.Add(bindingAliasProvider);

			filterContext.Controller.ValueProvider = provider;
		}
	}
}