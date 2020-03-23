using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPP.DbModels;
using WebAPP.Models;

namespace WebAPP.Controllers
{
    /// <summary>
    /// This controler is required to work with Deaprtment-User database.  
    /// </summary>
    [ApiController]
    [Route("UserDepartmentDbApi")]
    public class UserDepartmentController : ControllerBase
    {
        private readonly InMemoryDbContext _context;
        private const string DEP_NOTFOUND_MESSAGE = "There are no department with such name.";

        public UserDepartmentController(InMemoryDbContext context)
        {
            _context = context;
        }

        #region UserApi

        [HttpGet, Route("Users")]
        [SwaggerOperation(OperationId = "UserList",
            Description = "List of all users.",
            Summary = "List of all users.",
            Tags = new[] { "UserApi" })]
        public ActionResult<IEnumerable<UserModel>> GetUserList()
        {
            var users = _context.Users.
                Select(x => new UserModel()
                {
                    Name = x.Name,
                    DepartmentName = _context.Departments.Where(y=>y.Id == x.DepartmentId).Single().Name
                });

            return Ok(users);
        }

        [HttpGet, Route("{departmentCode:alpha}/Users")]
        [SwaggerOperation(OperationId = "GetDepartmentUsers",
            Description = "List of all department users.",
            Summary = "List of all department users.",
            Tags = new[] { "UserApi" })]
        [SwaggerResponse(200, "Well done!")]
        [SwaggerResponse(400, "List are not returned. Incorrect parameters or problems in database.")]
        public ActionResult<IEnumerable<UserModel>> GetDepartmentUsers(string departmentCode)
        {

            try
            {
                if (_context.Departments.Where(x => x.Name == departmentCode).Count() == 0)
                    return BadRequest(DEP_NOTFOUND_MESSAGE);

                var department = _context.Departments.Where(x => x.Name == departmentCode).Single();

                return Ok(_context.Users.
                    Where(x => x.DepartmentId == department.Id).
                    Select(
                        x => new UserModel() 
                        { 
                            DepartmentName = _context.Departments.Where(y => y.Id == x.DepartmentId).Single().Name,
                            Name = x.Name
                        }
                    )
               );
            }
            catch (Exception e)
            {
                return BadRequest($"Error in database transactions: {e}");
            }
        }

        [HttpPost, Route("AddUser")]
        [SwaggerOperation(OperationId = "AddNewUser",
            Description ="Add new user if departement with such id are exists",
            Summary = "Add new user if departement with such id are exists",
            Tags = new[] { "UserApi" })]
        [SwaggerResponse(200, "New user add to table.")]
        [SwaggerResponse(400, "User are not added. Incorrect parameters.")]
        public ActionResult AddUser(UserModel user)
        {
            if (_context.Departments.Where(x => x.Name == user.DepartmentName).Count() == 0)
                return BadRequest($"Department with  {user.DepartmentName} is not exist.");

            var userDb = new UserDbModel()
            {
                DepartmentId = _context.Departments.Where(y => y.Name == user.DepartmentName).Single().Id,
                Name = user.Name
            };

            _context.Users.Add(userDb);
            _context.SaveChanges();
            return Ok();
        }

        #endregion

        #region DeaprtmentApi
        [HttpGet, Route("Departments")]
        [SwaggerOperation(OperationId = "GetDepartmnetsList",
            Description = "Get Departments List",
            Summary = "Get Departments List",
            Tags = new[] { "DepartmentApi" })]
        public IEnumerable<DepartmentDbModel> GetDeps()
        {
            return _context.Departments;
        }

        [HttpPost, Route("DepartmentCreate")]
        [SwaggerOperation(OperationId = "DepartmentCreate",
            Description = "Create new department.",
            Summary = "Create new department.",
            Tags = new[] { "DepartmentApi" })]
        [SwaggerResponse(200, "Department is created.")]
        [SwaggerResponse(400, "Department is not created. There are problems in input parameters or database data.")]
        public ActionResult CreateDepartment(DepartmentModel model)
        {
            try
            {
                if (_context.Departments.Where(x => x.Name == model.Name).Count() > 0)
                    return BadRequest("Department with this code already exists");

                _context.Departments.Add
                    (
                        new DepartmentDbModel()
                        {
                            Name = model.Name
                        }
                    );

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest($"Error in database transactions: {e}");
            }

        }

        [HttpDelete, Route("DepartmentDelete/{code:alpha}")]
        [SwaggerOperation(OperationId = "DepartmentRemove",
            Description = "Remove department by code. All users with this department id are also removed.",
            Summary = "Remove department by code",
            Tags = new[] { "DepartmentApi" })]
        [SwaggerResponse(200, "Department is removed.")]
        [SwaggerResponse(400, "Department is not removed. There are problems in input parameters or database data.")]
        public ActionResult DeleteDepartment(string code)
        {
            try
            {
                if (_context.Departments.Where(x => x.Name == code).Count() == 0)
                    return BadRequest(DEP_NOTFOUND_MESSAGE);

                var departmentToRemove = _context.Departments.Where(x => x.Name == code).Single();

                _context.Departments.RemoveRange(departmentToRemove);

                _context.Users.RemoveRange(_context.Users.Where(x => x.DepartmentId == departmentToRemove.Id));

                _context.SaveChanges();
            }
            catch(Exception e)
            {
                return BadRequest($"Error in database transactions: {e}");
            }

            return Ok();
        }

        [HttpPost, Route("DepartmentRename")]
        [SwaggerOperation(OperationId = "DepartmentRename",
            Description = "Rename department. Found department by old name and rename to new name.",
            Summary = "Rename department.",
            Tags = new[] { "DepartmentApi" })]
        [SwaggerResponse(200, "Department is renamed.")]
        [SwaggerResponse(400, "Department is not renamed. There are problems in input parameters or database data.")]
        public ActionResult RenameDepartment(DepartmentRenameModel renameModel)
        {
            try
            {
                if (_context.Departments.Where(x => x.Name == renameModel.OldName).Count() == 0)
                    return BadRequest(DEP_NOTFOUND_MESSAGE);

                var departmentToRename = _context.Departments.Where(x => x.Name == renameModel.OldName).Single();

                departmentToRename.Name = renameModel.NewName;
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest($"Error in database transactions: {e}");
            }

            return Ok();
        }

        #endregion

    }
}
