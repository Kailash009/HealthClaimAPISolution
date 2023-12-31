﻿using HealthClaim.API.Services;
using HealthClaim.BAL.Repository.Interface;
using HealthClaim.DAL.Models;
using HealthClaim.Model.Dtos.Response;
using HealthClaim.Model.Dtos.UsersDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HealthClaim.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IAccountRepository _accountRepository;
        public readonly TokenService _tokenService;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("login")]
        public async Task<ResponeModel> Login(LoginDto loginDto)
        {
            ResponeModel response = await _accountRepository.Login(loginDto);
            return response;
        }

        [HttpPost("register")]
        public async Task<ResponeModel> Register(RegisterDto registerDto)
        {
            ResponeModel response = await _accountRepository.AccountCreate(registerDto);
            return response;
        }

        private UserDto CreateUserObject(ApplicationUser user)
        {
            return new UserDto
            {

                //Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CrateToken(user),
                UserName = user.UserName
            };
        }
    }
}

