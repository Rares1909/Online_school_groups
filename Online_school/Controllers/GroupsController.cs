using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_school.Data;
using Online_school.Models;
using System.Linq;

namespace Online_school.Controllers
{
    [Authorize]
    public class GroupsController : Controller

    {   
            private readonly ApplicationDbContext db;

            private readonly UserManager<ApplicationUser> _userManager;

            private readonly RoleManager<IdentityRole> _roleManager;

            public GroupsController(
                ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager
                )
            {
                db = context;

                _userManager = userManager;

                _roleManager = roleManager;
            }

            [NonAction]
            public IEnumerable<SelectListItem> GetAllCategories()
            {

                var selectList = new List<SelectListItem>();


                var categories = from cat in db.Categories
                                 select cat;

                // iteram prin categorii
                foreach (var category in categories)
                {

                    selectList.Add(new SelectListItem
                    {
                        Value = category.CategoryId.ToString(),
                        Text = category.Category_Name.ToString()
                    });
                }

                return selectList;
            }

        public IActionResult Index2(string id)
        {
                var groups = db.GroupsUsers.Include("Group").Include("User").Where(g => g.UserId == id);
                var rez = db.Groups.Include("Category").Join(groups, rez => rez.GroupId, group => group.GroupId, (rez, group) => rez);
                ViewBag.Groups = rez;
                foreach (var group in rez)
                    SetAccesRights_Index(group.GroupId);
            var search = "";
            if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
            {
                search = Convert.ToString(HttpContext.Request.Query["search"]).Trim();


                rez = rez.Where
                                        (
                                         at => at.Group_Name.Contains(search)
                                         || at.Description.Contains(search));

            }
            ViewBag.SearchString = search;
            ViewBag.Groups = rez;
            return View("Index");
           

        }

        [Authorize(Roles = "User,Editor,Admin")]
        public IActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var groups = db.Groups.Include("Category").Where(group => group.CategoryId == id.Value);
                ViewBag.Groups = groups;
                foreach (var group in groups)
                    SetAccesRights_Index(group.GroupId);

                var search = "";
                if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
                {
                    search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); 


                    groups = groups.Where
                                            (
                                             at => at.Group_Name.Contains(search)
                                             || at.Description.Contains(search));

                }
                ViewBag.SearchString = search;
                ViewBag.Groups = groups;
                return View();
            }
            else
            {
                var groups = db.Groups.Include("Category");
                // ViewBag.OriceDenumireSugestiva
                foreach (var group in groups)
                    SetAccesRights_Index(group.GroupId);
                var search = "";
                if (Convert.ToString(HttpContext.Request.Query["search"]) != null)
                {
                    search = Convert.ToString(HttpContext.Request.Query["search"]).Trim(); // eliminam spatiile libere 

                    // Cautare in articol (Title si Content)

                    groups = db.Groups.Where
                                            (
                                             at => at.Group_Name.Contains(search)
                                             || at.Description.Contains(search));

                }
                ViewBag.SearchString = search;
                ViewBag.Groups = groups;
                return View();
            }

            }
        

            [Authorize(Roles = "User,Editor,Admin")]
            public IActionResult Show(int id)
            {
            var check = db.GroupsUsers.Find(_userManager.GetUserId(User), id);
            if(check == null&&!User.IsInRole("Admin")) 
            {
                var adaug = new Group_User();
                adaug.UserId = _userManager.GetUserId(User);
                adaug.GroupId = id;
                adaug.moderator = false;
                db.GroupsUsers.Add(adaug);
                db.SaveChanges();
            }
                Group group = db.Groups.Include("Category")
                                                .Include("Messages")
                                                .Include("Messages.User")
                                                .Where(group => group.GroupId == id)
                                                .First();
                SetAccessRights_Show();
                return View(group);
            }

            [HttpPost]
            [Authorize(Roles = "User,Editor,Admin")]
            public IActionResult Show([FromForm] Message message)
            {
                message.Date = DateTime.Now;
                message.UserId = _userManager.GetUserId(User);

                if (ModelState.IsValid)
                {
                    db.Messages.Add(message);
                    db.SaveChanges();
                    return Redirect("/Groups/Show/" + message.GroupId);
                }

                else
                {

                    Group group = db.Groups.Include("Category")
                                                .Include("Messages")
                                                .Include("Messages.User")
                                                .Where(group => group.GroupId == message.GroupId)
                                                .First();

                SetAccessRights_Show();
                    return View(group);


                }
            }
           [Authorize(Roles = "User,Editor,Admin")]
            public IActionResult New()
            {
                Group group = new Group();

                group.Categ = GetAllCategories();
                return View(group);


            }
            [HttpPost]
            [Authorize(Roles = "User,Editor,Admin")]
            public async Task<IActionResult> New(Group group)
            {
                group.Created = DateTime.Now;


                if (ModelState.IsValid)
                {
                    db.Groups.Add(group);
                    db.SaveChanges();
                if (User.IsInRole("User"))
                {

                    var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));

                    await _userManager.RemoveFromRoleAsync(user, "User");

                    await _userManager.AddToRoleAsync(user, "Editor");

                    await _userManager.UpdateAsync(user);
                }
                if (!User.IsInRole("Admin"))
                {
                    Group_User g = new Group_User();
                    g.UserId = _userManager.GetUserId(User);
                    g.GroupId = group.GroupId;
                    g.moderator = true;
                    db.GroupsUsers.Add(g);
                    db.SaveChanges();
                }
                    return RedirectToAction("Index");
                }
                else
                {
                    group.Categ = GetAllCategories();
                    return View(group);
                }
            }

            public IActionResult Edit(int id)
        {
            var group = db.Groups.Find(id);
            group.Categ = GetAllCategories();
            return View(group);
        }



        [HttpPost]
        public IActionResult Edit(int id,Group RequestGroup)
        {
            Group group = db.Groups.Find(id);


            if (ModelState.IsValid)
            {
                group.Group_Name= RequestGroup.Group_Name;

                group.Description= RequestGroup.Description;

                group.CategoryId= RequestGroup.CategoryId;

                db.SaveChanges();

                return Redirect("/Groups/Show/" + group.GroupId);
            }
            else
            {
                RequestGroup.Categ = GetAllCategories();
                return View(RequestGroup);
            }


        }

        [HttpPost]
            //[Authorize(Roles = "Editor,Admin")]
            public IActionResult Delete(int Id)
            {
                Group group = db.Groups.Where(g => g.GroupId == Id)
                                             .First();
            var messages = db.Messages.Where(m => m.GroupId == Id);
            foreach(var message in messages)
                db.Messages.Remove(message);

                db.Groups.Remove(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        

             [HttpPost]
            [Authorize(Roles = "User,Editor,Admin")]
            public IActionResult Leave(int Id)
            {
            var elimin = db.GroupsUsers.Find(_userManager.GetUserId(User),Id);
                db.GroupsUsers.Remove(elimin);  
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        private void SetAccessRights_Show()
        {
            ViewBag.AfisareButoane = false;


            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }

        private void SetAccesRights_Index(int id)
        {
            var user_group = db.GroupsUsers;
            ViewData[id.ToString()] = 1;
            if (User.IsInRole("Admin"))
            { ViewData[id.ToString()] = 3;
                return; }
            foreach (var ug in user_group)
            {
                if (ug.GroupId == id && ug.UserId == _userManager.GetUserId(User))
                {
                    ViewData[id.ToString()] = 2;
                    if (ug.moderator == true)
                        ViewData[id.ToString()] = 3;
                }
            }
           
        }
        


    }


    }


   
    

   
