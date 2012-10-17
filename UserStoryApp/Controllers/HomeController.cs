using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserStoryApp.Repositories.Interfaces;

namespace UserStoryApp.Controllers
{
    public class HomeController : Controller
    {

        IStoryRepository _storyRepository;

        public HomeController(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            var model = _storyRepository.GetRootAggregate();
            return View(model);
        }

    }
}
