using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BindingAlias {
	public class ValueProvider : IValueProvider {
		private readonly Func<IValueProvider> _defaultValueProvider;
		private readonly Dictionary<string, BindingAliasParameter> _bindingAliasParameter;

		public ValueProvider(IValueProvider defaultValueProvider) {
			_defaultValueProvider = () => defaultValueProvider;
			_bindingAliasParameter = new Dictionary<string, BindingAliasParameter>();
		}

		public ValueProvider(ControllerContext controllerContext) {
			_defaultValueProvider = () => controllerContext.Controller.ValueProvider;
			_bindingAliasParameter = new Dictionary<string, BindingAliasParameter>();
		}

		public void AddBindingAlias(IEnumerable<BindingAliasParameter> bindingAliasParameteres) {
			foreach (var bindingAliasParameter in bindingAliasParameteres) {
				AddBindingAlias(bindingAliasParameter);
			}
		}

		public void AddBindingAlias(BindingAliasParameter bindingAliasParameter) {
			_bindingAliasParameter.Add(bindingAliasParameter.ParameterName.ToLower(), bindingAliasParameter);
		}

		public bool ContainsPrefix(string prefix) {
			if (_bindingAliasParameter.ContainsKey(prefix.ToLower())) {
				foreach (var alias in _bindingAliasParameter[prefix.ToLower()].BindingAliases) {
					if (_defaultValueProvider().ContainsPrefix(alias)) {
						return true;
					}
				}
			}

			return false;
		}

		public ValueProviderResult GetValue(string key) {
			if (!_bindingAliasParameter.ContainsKey(key.ToLower())) {
				return null;
			}

			ValueProviderResult result = null;

			foreach (var alias in _bindingAliasParameter[key.ToLower()].BindingAliases) {
				result = _defaultValueProvider().GetValue(alias);

				if (result != null) {
					return result;
				}
			}

			return result;
		}
	}
}