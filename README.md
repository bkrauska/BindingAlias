BindingAlias
============

.Net MVC Model Binding Alias Functionality

Extension of the functionality built and described in the following [blog post](http://jeffreypalermo.com/blog/adding-an-alias-to-an-action-parameter-for-model-binding-in-asp-net-mvc/).

Gives the developer the ability to override the binding for properties and action parameters. Most useful when trying to manage query string length and readability. Also useful for handling breaking changes that would effect parameter name changes.

## Using

Start by adding `GlobalFilters.Filters.Add(new BindingAlias.Initializer());` to the `Application_Start()` in `Global.asax.cs`

From there you can add the `BindingAlias` attribute to action parameters and model properties that are handled by the default model binding in .NET MVC.

## Example

Example of paramerter aliasing.

```C#
public class DefaultController : Controller {
	public JsonResult Search([BindingAlias("p")] int pageNumber) {
		return Json(new { PageNumber = pageNumber }, JsonRequestBehavior.AllowGet);
	}
}
```

/default/search?p=3 Responds with

```
{"PageNumber":3}
```

Example of model property aliasing

```C#
public class DefaultController : Controller {
	public JsonResult Index(TestViewModel model) {
		return Json(model, JsonRequestBehavior.AllowGet);
	}
}

public class TestViewModel {
	[BindingAlias("p", "p_id", "id")]
	public int ProductId { get; set; }
	[BindingAlias("n")]
	public string Name { get; set; }
	[BindingAlias("u")]
	public int UserId { get; set; }
}
```

/default/index?pid=123&n=Awesome+Product&u=456 Responds with

```
{"ProductId":123,"Name":"Awesome Product","UserId":456}
```