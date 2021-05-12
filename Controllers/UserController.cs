using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_auth_example.Models;
using api_auth_example.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using api_auth_example.Data;

namespace api_auth_example.Controllers
{
    [Route("v1/users")]
    public class UserController : Controller
    {

        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }



        /// <summary>
        /// Get all users
        /// </summary>        
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "employee,admin")]
        // [ResponseCache(VaryByHeader = "User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)] // para definir o cache somente neste método.
        // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // caso toda a aplicação esteja com cache mas este método não pode ter cache.
        public async Task<ActionResult<List<User>>> Get()
        {
            var users = await _context
                .Users
                .AsNoTracking()
                .ToListAsync();

            return users;
        }


        /// <summary>
        /// Create a user
        /// </summary>        
        /// <param name="model">user</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        // [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> Post(
            [FromBody] User model)
        {
            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Força o usuário a ser sempre "funcionário"
                model.Role = "employee";

                _context.Users.Add(model);
                await _context.SaveChangesAsync();

                // Esconde a senha
                model.Password = "";
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });

            }
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="id">id of the user</param>
        /// <param name="model">user object</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> Put(
            int id,
            [FromBody] User model)
        {
            // Verifica se os dados são válidos
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o ID informado é o mesmo do modelo
            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrada" });

            try
            {
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível criar o usuário" });

            }
        }

        /// <summary>
        /// Delete a specifc user
        /// </summary>        
        /// <param name="id">id of the user</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<User>> Delete(
            int id)
        {
            var category = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound(new { message = "Usuário não encontrado" });

            try
            {
                _context.Users.Remove(category);
                await _context.SaveChangesAsync();
                return category;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível remover o usuário" });
            }
        }

        /// <summary>
        /// Authentication method, returns the JWT token
        /// </summary>        
        /// <remarks>
        /// Sample request:
        /// 
        ///     
        ///     {
        ///         "username":"batman",
        ///         "password":"batman"
        ///     }        
        ///     
        /// </remarks>
        /// <param name="model">user and password</param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody] User model)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(x => x.Username == model.Username && x.Password == model.Password)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            var token = TokenService.GenerateToken(user);

            user.Password = "";
            return new
            {
                user = user,
                token = token
            };

        }

    }
}