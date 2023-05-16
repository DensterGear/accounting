using Accounting.Api.Mappers;
using Accounting.Api.Models;
using Accounting.BL;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FSharp.Core;
using MongoDB.Bson;

namespace Accounting.Api.Tests.Mappers;

#nullable disable

[TestClass]
public class UserMapperTests
{
    private const string Id = "646287aff5f885ca9158c5aa";
    private const string Name = "John";
    private const string Email = "john@example.com";
    private const int Age = 25;
    private const string City = "New York";
    
    private IMapper _mapper; 
    
    [TestInitialize]
    public void TestInit()
    {
        var services = new ServiceCollection();
        services.AddAutoMapper(typeof(UserMapper));
        var serviceProvider = services.BuildServiceProvider();
        _mapper = serviceProvider.GetService<IMapper>();
    }
    
    [TestMethod]
    public void UserViewModel_MapToFSharpModel_ShouldMapAllFields()
    {
        //Arrange
        var userViewModel = new UserViewModel
        {
            Id = Id,
            Name = Name,
            LastName = "Smith",
            Email = Email,
            Age = Age,
            Gender = Gender.Female,
            City = City
        };
        
        //Act
        var actualUser = _mapper.Map<UserViewModel, Domain.User>(userViewModel);

        //Assert
        Assert.AreEqual(userViewModel.Id, actualUser.Id.ToString());
        Assert.AreEqual(userViewModel.Name, actualUser.Name);
        Assert.AreEqual(userViewModel.LastName, actualUser.LastName);
        Assert.AreEqual(userViewModel.Email, actualUser.Email);
        Assert.AreEqual(userViewModel.Age, actualUser.Age);
        Assert.AreEqual(userViewModel.Gender.ToString(), actualUser.Gender.ToString());
        Assert.AreEqual(userViewModel.City, actualUser.City);
    }
    
    [TestMethod]
    public void UserBl_MapToViewModel_ShouldMapAllFields()
    {
        //Arrange
        var user = new Domain.User(new BsonObjectId(ObjectId.GenerateNewId()), Name, FSharpOption<string>.None, Email, Age, Domain.Gender.Female, City);
        
        //Act
        var actualViewModelUser = _mapper.Map<Domain.User, UserViewModel>(user);

        //Assert
        Assert.AreEqual(user.Id.ToString(), actualViewModelUser.Id);
        Assert.AreEqual(user.Name, actualViewModelUser.Name);
        Assert.AreEqual(user.Email, actualViewModelUser.Email);
        Assert.AreEqual(user.Age, actualViewModelUser.Age);
        Assert.AreEqual(user.Gender.ToString(), actualViewModelUser.Gender.ToString());
        Assert.AreEqual(user.City, actualViewModelUser.City);
    }
}