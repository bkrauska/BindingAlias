using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace BindingAlias {
	public class ModelBinder : DefaultModelBinder {
		readonly ValueProviderCollection _valueProviderCollection = new ValueProviderCollection();

		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, System.Type modelType) {
			object model = base.CreateModel(controllerContext, bindingContext, modelType);
			return model;
		}

		protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor) {
			base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
			ValueProviderCollection collection = new ValueProviderCollection();
			
			var bindingAliasProvider = new ValueProvider(controllerContext);
			foreach (string alias in bindingContext.ModelMetadata.AdditionalValues.Keys) {
				bindingAliasProvider.AddBindingAlias(new BindingAliasParameter(bindingContext.ModelName, alias));
			}
			collection.Add(bindingContext.ValueProvider);

			bindingContext.ValueProvider = collection;
			return base.BindModel(controllerContext, bindingContext);
		}

		private void AddAliasesForProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor) {
			IEnumerable<BindingAlias> aliases = propertyDescriptor.Attributes.OfType<BindingAlias>();
			if (aliases.Any()) {
				var bindingAliasProvider = new ValueProvider(controllerContext);
				foreach (var aliasAttribute in aliases) {
					bindingAliasProvider.AddBindingAlias(new BindingAliasParameter(propertyDescriptor.Name, aliasAttribute.Names));
				}
				_valueProviderCollection.Add(bindingAliasProvider);
			}
		}
	}
}