﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Project2_GSG.Core.Manager.Interface;
using Project2_GSG.ModelViews;
using Project2_GSG.ModelViews.ModelView;
using System.Linq;
using Tazeez.Common.Extensions;

namespace Project2_GSG.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : Controller
    {
        private UserModel _loggedInUser;
        private TodoResponse _loggedInTodo;
        protected UserModel LoggedInUser
        {
            get
            {
                if (_loggedInUser != null)
                {
                    return _loggedInUser;
                }

                Request.Headers.TryGetValue("Authorization", out StringValues Token);

                if (string.IsNullOrWhiteSpace(Token))
                {
                    _loggedInUser = null;
                    return _loggedInUser;
                }

                var ClaimId = User.Claims.FirstOrDefault(c => c.Type == "Id");

                if (ClaimId == null || !int.TryParse(ClaimId.Value, out int id))
                {
                    throw new ServiceValidationException(401, "Invalid or expired token");
                }

                var commonManager = HttpContext.RequestServices.GetService(typeof(ICommonManager)) as ICommonManager;

                _loggedInUser = commonManager.GetUserRole(new UserModel { Id = id });

                return _loggedInUser;
            }
        }

        protected TodoResponse LoggedInTodo
        {
            get
            {
                if (_loggedInTodo != null)
                {
                    return _loggedInTodo;
                }

                Request.Headers.TryGetValue("Authorization", out StringValues Token);

                if (string.IsNullOrWhiteSpace(Token))
                {
                    _loggedInTodo = null;
                    return _loggedInTodo;
                }

                var ClaimId = User.Claims.FirstOrDefault(c => c.Type == "Id");

                if (ClaimId == null || !int.TryParse(ClaimId.Value, out int id))
                {
                    throw new ServiceValidationException(401, "Invalid or expired token");
                }

                var commonTodoManager = HttpContext.RequestServices.GetService(typeof(ICommonTodoManager)) as ICommonTodoManager;

                _loggedInTodo = commonTodoManager.GetTodoRole(new TodoResponse { Id = id });

                return _loggedInTodo;
            }
        }
    }
}
