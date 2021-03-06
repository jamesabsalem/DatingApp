﻿using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.Data;
using DatingApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            foreach (var user in users)
            {
                user.Photos = await _repo.GetPhoto(user.Id);
            }

            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            user.Photos = await _repo.GetPhoto(user.Id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            var user = await _repo.GetUser(id);
            if (user == null)
                return Unauthorized();
            var userMap = _mapper.Map<User>(userForUpdateDto);
            var isUpdate = await _repo.UserUpdate(id, userMap);
            if (isUpdate)
                return NoContent();
            throw new Exception($"Updating user {id} failed on save");
        }
    }
}