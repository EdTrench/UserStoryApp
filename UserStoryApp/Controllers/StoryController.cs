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

        public ActionResult Details(int id, bool leafNodes = true)
        {
            if (leafNodes)
            {
                var model = _storyRepository.GetLeafNodes(id);
                return View(model);
            }
            else
            {
                var model = _storyRepository.GetById(id);
                var parent = _storyRepository.GetByParentId(model.Parent.Id);
                return View(parent);
            }
        }

        //
        // GET: /Story/Create

        public ActionResult Create(int parentId)
        {
            var model = new Story();
            var parent = _storyRepository.GetById(parentId);
            model.Parent = parent;
            return View(model);
        }

        //
        // POST: /Story/Create

        [HttpPost]
        public ActionResult Create(Story story)
        {
            try
            {
                _storyRepository.Add(story);
                return View("Details", _storyRepository.GetLeafNodes(story.Parent.Id));
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
                return View("Details", _storyRepository.GetLeafNodes(story.Parent.Id));
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
            var model = _storyRepository.GetById(id);
            return View(model);
        }

        //
        // POST: /Story/Delete/5

        [HttpPost]
        public ActionResult Delete(Story story)
        {
            try
            {
                story = _storyRepository.GetById(story.Id);
                _storyRepository.Delete(story);
                if (_storyRepository.GetLeafNodes(story.Parent.Id).Count <= 0)
                {
                    var next = _storyRepository.GetAllAncestorsOfStory(story.Parent.Id).First();
                    return View("Details", _storyRepository.GetLeafNodes(next.Id));
                }
                else
                {
                    return View("Details", _storyRepository.GetLeafNodes(story.Parent.Id));
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
