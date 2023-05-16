using Accounting.Api.Models;
using Accounting.BL;
using Accounting.BL.UserService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;

    public UserController(IMapper mapper)
    {
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IEnumerable<UserViewModel> GetAll()
    {
        var users = UserService.GetAll();
        return users.Select(user => _mapper.Map<UserViewModel>(user));
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public UserViewModel Get(string id)
    {
        var user = UserService.GetById(id);
        return _mapper.Map<UserViewModel>(user);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public UserViewModel Post(UserViewModel userViewModel)
    {
        var user = _mapper.Map<Domain.User>(userViewModel);
        var createdUser = UserService.Create(user);
        return _mapper.Map<UserViewModel>(createdUser);
    }
    
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public UserViewModel Put(string id, UserViewModel userViewModel)
    {
        var user = _mapper.Map<Domain.User>(userViewModel);
        var createdUser = UserService.Update(id, user);
        return _mapper.Map<UserViewModel>(createdUser);
    }
    
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Delete(string id)
    {
        UserService.Delete(id);
        return Ok();
    }
}