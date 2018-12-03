﻿using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoWatcher.Api.Requests;
using CryptoWatcher.Api.ResponseExamples;
using CryptoWatcher.Api.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace CryptoWatcher.Api.Controllers
{
    // ReSharper disable once InconsistentNaming
    public class B_UsersController : Controller
    {
        private readonly IMediator _mediator;

        public B_UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        [Route("users")]
        [SwaggerResponse(200, Type = typeof(List<UserResponse>))]
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(UserListResponseExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Users" }, OperationId = "Users_GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            // Reponse
            var response = await _mediator.Send(new GetAllUsersRequest());

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Get a user
        /// </summary>
        [HttpGet]
        [Route("users/{userId}", Name = "Users_GetUser")]
        [SwaggerResponse(200, Type = typeof(UserResponse))]
        [SwaggerResponse(404, Type = typeof(ErrorResponse))]
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(200, typeof(UserResponseExample))]
        [SwaggerResponseExample(404, typeof(NotFoundExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Users" }, OperationId = "Users_GetUser")]
        public async Task<IActionResult> GetUser(string userId)
        {
            // Reponse
            var response = await _mediator.Send(new GetUserRequest() { UserId = userId });

            // Return
            return Ok(response);
        }

        /// <summary>
        /// Add a user
        /// </summary>
        [HttpPost]
        [Route("users")]
        [SwaggerResponse(201, Type = typeof(UserResponse))]
        [SwaggerResponse(400, Type = typeof(ErrorResponse))]
        [SwaggerResponse(409, Type = typeof(ErrorResponse))]
        [SwaggerResponse(422, Type = typeof(ValidationResponse))]
        [SwaggerResponse(500, Type = typeof(ErrorResponse))]
        [SwaggerResponseExample(201, typeof(UserResponseExample))]
        [SwaggerResponseExample(400, typeof(BadRequestExample))]
        [SwaggerResponseExample(409, typeof(ConflictExample))]
        [SwaggerResponseExample(422, typeof(InvalidRequestExample))]
        [SwaggerResponseExample(500, typeof(InternalServerErrorExample))]
        [SwaggerOperation(Tags = new[] { "Users" }, OperationId = "Users_AddUser")]
        public async Task<IActionResult> AddUser([FromBody]AddUserRequest request)
        {
            // Reponse
            var response = await _mediator.Send(request);

            // Return
            return CreatedAtRoute("Users_GetUser", new { response.UserId }, response);
        }
    }
}

