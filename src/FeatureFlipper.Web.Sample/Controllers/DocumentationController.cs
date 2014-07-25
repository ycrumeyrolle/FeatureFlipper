namespace FeatureFlipper.Web.Sample.Controllers
{
    using System.Web.Mvc;

    public class DocumentationController : Controller
    {
        // GET: Documentation
        public ActionResult Index()
        {
            var explorer = Features.Services.GetService<IFeatureExplorer>();
            return View(explorer.GetFeatures());
        }
    }
}