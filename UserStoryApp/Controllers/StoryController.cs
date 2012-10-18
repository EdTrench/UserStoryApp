using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UserStoryApp.Repositories.Interfaces;
using UserStoryApp.Models;

namespace UserStoryApp.Controllers
{
    public class StoryController : Controller
    {

        IStoryRepository _storyRepository;

        public StoryController(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }
        //
        // GET: /Story/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Story/Details/5

        public ActionResult Details(int id)
        {
            //var model = _storyRepository.GetAllDescendantsOfStory(id);
            var model = _storyRepository.GetLeafNodes(id);
            return View(model);
        }

        //
        // GET: /Story/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Story/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Story/Edit/5

        public ActionResult Edit(int id)
        {
            var model = _storyRepository.GetById(id);
            return View(model);
        }

        //
        // POST: /Story/Edit/5

        [HttpPost]
        public ActionResult Edit(Story story)
        {
            try
            {
                _storyRepository.Update(story);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Story/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Story/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
