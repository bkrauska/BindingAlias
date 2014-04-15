using System.Web.Mvc;

namespace BindingAlias.SampleWeb.Controllers {
	public class DefaultController : Controller {
		public JsonResult Index(TestViewModel model) {
			return Json(model, JsonRequestBehavior.AllowGet);
		}

		public JsonResult Search([BindingAlias("p")] int pageNumber) {
			return Json(new { PageNumber = pageNumber }, JsonRequestBehavior.AllowGet);
		}
	}

	public class TestViewModel {
		[BindingAlias("p", "p_id", "id")]
		public int ProductId { get; set; }
		[BindingAlias("n")]
		public string Name { get; set; }
		[BindingAlias("u")]
		public int UserID { get; set; }
	}
}