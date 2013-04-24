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
                var leaves = _storyRepository.GetLeafNodes(id);
                if (leaves.Count <= 0)
                {
                    var story = _storyRepository.GetById(id);
                    var parents = _storyRepository.GetByParentId(story.Parent.Id);
                    return View(parents);
                }
                else
                {
                    return View(leaves);
                }
            }
            else
            {
                var story = _storyRepository.GetById(id);
                if (story.Parent != null)
                {
                    var parents = _storyRepository.GetByParentId(story.Parent.Id);
                    return View(parents);
                }
                else
                {
                    return View("../Home/Index", story);
                }
            }
        }

        //
        // GET: /Story/Create

        public ActionResult Create(int parentId)
        {
            var model = new Story();
            var parent = _storyRepository.GetById(parentId);
            parent.AddChildStory(model);
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
                //do we have any leaves left?
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

        //
        // Create a Diagram for Children
        public ActionResult Diagram(int id)
        {
            //var model = _storyRepository.GetAllDescendantsOfStory(id);
            var model = _storyRepository.GetById(id);
            return View(model);
        }
    }
}
