using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductAPI.Identity.BindingModels;
using ProductAPI.Identity.Models;
using ProductAPI.Services.Interfaces;

namespace ProductAPI.Controllers {
    public class UserController : DefaultController {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        #region GET
        [HttpGet()]
        // [AllowAuthorized(AccessRoles.Admin)]
        //[AllowAnonymous]
        public async Task<IActionResult> GetAllAsync ()
        {
            List<AppUser> userList = await _userService.GetAllUsersAsync();
            if (userList.IsNullOrEmpty())
                return BadRequest($"Could not find any users");
            return Ok(userList);
        }
    
    
        [HttpGet("{id}")]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> GetAsyncById(string id)
        {
            AppUser user = await _userService.GetUserByIdAsync(id);
            if (user != null) 
                return Ok (user);
            return BadRequest($"Could not find user with Id : {id}");
        }

        #endregion
    
        #region POST
        [HttpPost()]
        //[AllowAuthorizedAttribute(AccessRoles.Admin)]
        public async Task<IActionResult> CreateAsync([FromBody]UserPostModel request)
        {
           
            AppUser user = new AppUser()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.Password,
                IsActivated = request.IsActivated
            };
            AppUser resultUser = await _userService.CreateUserAsync(user,request.Roles, request.Password);
        
            if(resultUser == null) 
                return BadRequest($"Could not create user with Email : {request.Email}");
            return Ok(resultUser);
        }
 
    
        #endregion
        
        #region PUT
        [HttpPut()]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody]UserPutModel request)
        {
            AppUser fetchedUser = await _userService.GetUserByIdAsync(request.Id);
            if(fetchedUser == null) 
                return BadRequest($"Could not find user with Id : {request.Id}");
        
            AppUser requestUser = new AppUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = request.Password,
                IsActivated = request.IsActivated
            };
            AppUser updatedUser = await _userService.UpdateUserAsync(requestUser);
        
            if(updatedUser == null) 
                return BadRequest($"Could not update user with Id : {request.Id}");
            return Ok(updatedUser);
                
        }
        
        #endregion

    
        #region DELETE

        [HttpDelete("{id}")]
        //[Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            AppUser fetchedUser = await _userService.GetUserByIdAsync(id);
            if(fetchedUser == null) BadRequest($"Could not find user with {id}");
            bool result = await _userService.DeleteAsync(fetchedUser.Id); 
            if(result == null) BadRequest($"Could not delete user with {id}");
            return Ok($"User with Id : {id} has been deleted !");
        }
        #endregion
    
    }
}