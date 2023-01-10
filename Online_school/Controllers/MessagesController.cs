using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_school.Data;
using Online_school.Models;

namespace Online_school.Controllers
{
    public class MessagesController : Controller
    {

        private readonly ApplicationDbContext db;
        public MessagesController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Message mes = db.Messages.Find(id);
                db.Messages.Remove(mes);
                db.SaveChanges();
                return Redirect("/Groups/Show/" + mes.GroupId);
                  
        }

        public IActionResult Edit(int id)
        {
            Message message = db.Messages.Find(id);

            
                return View(message);
      
        }

        [HttpPost]
        
        public IActionResult Edit(int id, Message requestMessage)
        {
            Message message = db.Messages.Find(id);

            
                if (ModelState.IsValid)
                {
                    message.Content = requestMessage.Content;

                    db.SaveChanges();

                    return Redirect("/Groups/Show/" + message.GroupId);
                }
                else
                {
                    return View(requestMessage);
                }
            
           
        }
    }
}
