using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace BindingAlias {
	public class Initializer : IAuthorizationFilter {
		public void OnAuthorization(AuthorizationContext filterContext) {
			var parameterAliases = new List<BindingAliasParameter>();
			ParameterDescriptor[] parameterDescriptors = filterContext.ActionDescriptor.GetParameters();
			AddAliases(parameterAliases, parameterDescriptors);

			IValueProvider valueProvider = filterContext.Controller.ValueProvider;
			var provider = new ValueProviderCollection { valueProvider };

			var bindingAliasProvider = new ValueProvider(valueProvider);
			bindingAliasProvider.AddBindingAlias(parameterAliases);
			provider.Add(bindingAliasProvider);

			filterContext.Controller.ValueProvider = provider;
		}

		private static void AddAliases(List<BindingAliasParameter> parameterAliases, IEnumerable<ParameterDescriptor> parameterDescriptors) {
			foreach (ParameterDescriptor descriptor in parameterDescriptors) {
				AddAlias(parameterAliases, descriptor, descriptor.ParameterType, descriptor.ParameterName);
			}
		}

		private static void AddAliases(List<BindingAliasParameter> parameterAliases, IEnumerable<PropertyInfo> incomingPropertyInfos) {
			foreach (PropertyInfo propertyInfo in incomingPropertyInfos) {
				AddAlias(parameterAliases, propertyInfo, propertyInfo.PropertyType, propertyInfo.Name);
			}
		}

		private static void AddAlias(List<BindingAliasParameter> parameterAliases, ICustomAttributeProvider propertyInfo, Type propertyType, string propertyName) {
			string parameterName = propertyName;
			var aliases = (BindingAlias[])propertyInfo.GetCustomAttributes(typeof(BindingAlias), false);
			if (aliases.Any()) {
				foreach (BindingAlias @alias in aliases) {
					parameterAliases.Add(new BindingAliasParameter(parameterName, @alias.Names));
				}
			}

			PropertyInfo[] propertyInfos = propertyType.GetProperties();
			AddAliases(parameterAliases, propertyInfos);
		}
	}
}