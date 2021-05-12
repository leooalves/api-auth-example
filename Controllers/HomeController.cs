using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api_auth_example.Data;
using api_auth_example.Models;

namespace api_auth_example.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {
        /// <summary>
        /// Use this method to create the first users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var admin = new User { Id = 1, Username = "batman", Password = "batman", Role = "admin" };
            var employee = new User { Id = 2, Username = "robin", Password = "robin", Role = "employee" };


            context.Users.Add(employee);
            context.Users.Add(admin);

            await context.SaveChangesAsync();

            return Ok(new
            {
                employee,
                admin
            });
        }
    }
}