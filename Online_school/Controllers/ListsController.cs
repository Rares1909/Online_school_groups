using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_school.Data;
using Online_school.Models;

namespace Online_school.Controllers
{
    public class ListsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public ListsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }
        public IActionResult Show(int id)
        {
            SetAccesRghts(id);
            ViewBag.list=db.GroupsUsers.Include("Group").Include("User").Where(g => g.GroupId == id);
            foreach (var it in ViewBag.list)
                SetAccesRights_member(id, it.UserId);
            return View();
            
        }
        [HttpPost]
        public async Task<IActionResult> MakeEditor(string Userid,int Groupid)
        {
            Group_User user1 = db.GroupsUsers.Find(Userid, Groupid);
            user1.moderator = true;
            db.SaveChanges();

            var user = await _userManager.FindByIdAsync(Userid);

            await _userManager.RemoveFromRoleAsync(user, "User");

            await _userManager.AddToRoleAsync(user, "Editor");

            await _userManager.UpdateAsync(user);

           return RedirectToAction("Show", new {id=Groupid});

            
        }
        [HttpPost]
        public async Task<IActionResult>Kick(string Userid, int Groupid)
        {
            Group_User user1 = db.GroupsUsers.Find(Userid, Groupid);
            db.GroupsUsers.Remove(user1);
            db.SaveChanges();

            var all = db.GroupsUsers;
            var ok = 0;
            foreach (var item in all)
                if (item.UserId == Userid && item.moderator==true)
                    ok = 1;
            if(ok == 0 && !User.IsInRole("Admin"))
            {
                var user = await _userManager.FindByIdAsync(Userid);

                await _userManager.RemoveFromRoleAsync(user, "Editor");

                await _userManager.AddToRoleAsync(user, "User");

                await _userManager.UpdateAsync(user);

            }
            return RedirectToAction("Show" ,new { id = Groupid });
        }

        private void SetAccesRghts(int id)
        {
            ViewBag.moderator = false;
            var user=db.GroupsUsers.Find(_userManager.GetUserId(User), id);
            if (User.IsInRole("Admin"))
            {
                ViewBag.moderator = true;
                return;
            }
            if (user.moderator == true)
                ViewBag.moderator = true;
        }

        private void SetAccesRights_member(int id,string user)
        {   
            ViewBag.eu=_userManager.GetUserId(User);
            ViewData[user]=false;
            var u=db.GroupsUsers.Find(user ,id);
            if(u.moderator == true)
                ViewData[user]=true;
        }

        public async Task<IActionResult> make_user(string UserId, int GroupId)
        {
            Group_User user1 = db.GroupsUsers.Find(UserId, GroupId);
            user1.moderator = false;
            db.SaveChanges();
            var ok = 0;
            foreach (var item in db.GroupsUsers)
            {
                if (item.UserId == UserId && item.moderator == true)
                    ok = 1;
            }
            if (ok == 0)
            {
                var user = await _userManager.FindByIdAsync(UserId);

                await _userManager.RemoveFromRoleAsync(user, "Editor");

                await _userManager.AddToRoleAsync(user, "User");

                await _userManager.UpdateAsync(user);

            }
            return RedirectToAction("Show", new { id = GroupId });

        }
    }
}
