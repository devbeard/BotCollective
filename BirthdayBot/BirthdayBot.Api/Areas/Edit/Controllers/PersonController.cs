using System.Web.Mvc;

namespace BirthdayBot.Api.Areas.Edit.Controllers
{
    public class PersonController : Controller
    {
        // GET: Edit/Person
        public ActionResult Index()
        {
            return View();
        }

        // GET: Edit/Person/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Edit/Person/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Edit/Person/Create
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

        // GET: Edit/Person/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Edit/Person/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Edit/Person/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Edit/Person/Delete/5
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
