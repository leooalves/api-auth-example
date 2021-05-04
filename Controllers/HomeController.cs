using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api_auth_example.Data;
using api_auth_example.Models;

namespace api_auth_example.Controllers
{
    [Route("v1")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<dynamic>> Get([FromServices] DataContext context)
        {
            var employee = new User { Id = 1, Username = "employee", Password = "employee", Role = "employee" };
            var manager = new User { Id = 2, Username = "admin", Password = "admin", Role = "admin" };
            
            context.Users.Add(employee);
            context.Users.Add(manager);
            
            await context.SaveChangesAsync();

            return Ok(new
            {
                message = "Dados configurados"
            });
        }
    }
}