using System.Web.Mvc;

namespace BindingAlias {
	public class ValueProviderFactory : System.Web.Mvc.ValueProviderFactory {
		private readonly BindingAliasParameter[] _aliases;

		public ValueProviderFactory(params BindingAliasParameter[] aliases) {
			_aliases = aliases;
		}

		public override IValueProvider GetValueProvider(ControllerContext controllerContext) {
			var providerCollection = new ValueProviderCollection();

			var bindingAliasProvider = new ValueProvider(controllerContext);
			bindingAliasProvider.AddBindingAlias(_aliases);
			providerCollection.Add(bindingAliasProvider);

			return providerCollection;
		}
	}
}