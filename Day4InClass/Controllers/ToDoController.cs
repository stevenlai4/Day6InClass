using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Day4InClass.Data;
using Day4InClass.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day4InClass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoContext _db;

        public ToDoController(ToDoContext db)
        {
            _db = db;
        }

        // Since we have cookie authentication and Jwt authentication we must
        // specify that we want Jwt authentication here.
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetAll()
        {
            // access the claim from the HttpContext
            var claim = HttpContext.User.Claims.ElementAt(0);
            // use the claim to get the aspnetusers username
            string userName = claim.Value;
            // aspnetusers id
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var todos = _db.ToDos.ToList();

                if (!todos.Any())
                {
                    return NoContent();
                }

                return Ok(todos);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}", Name = "GetOne")]
        public IActionResult GetById(int id)
        {
            var todo = _db.ToDos.Where(t => t.Id == id).FirstOrDefault();
            return new ObjectResult(todo);
        }

        [HttpPost]
        public IActionResult Create(ToDo todo)
        {
            if (todo.Description != null || todo.Description != "")
            {
                try
                {
                    _db.ToDos.Add(todo);
                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
                return Ok(todo);
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("MyEdit")]
        public IActionResult Update(ToDo todo)
        {
            // Old value
            var item = _db.ToDos.Where(t => t.Id == todo.Id).FirstOrDefault();

            if(item == null)
            {
                return NotFound();
            }
            else
            {
                item.IsComplete = todo.IsComplete;
                _db.SaveChanges();
            }

            return Ok(item);
        }

        [HttpDelete]
        [Route("MyDelete")]
        public IActionResult Delete(long Id)
        {
            var item = _db.ToDos.Where(t => t.Id == Id).FirstOrDefault();

            if(item == null)
            {
                return NotFound();
            }
     
            _db.ToDos.Remove(item);
            _db.SaveChanges();
            return Ok(item);
        }
    }
}
