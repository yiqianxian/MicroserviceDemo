﻿using AuthWeb.Config;
using CommonLibrary.Utilities;
using IdentityModel.Client;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ICallAuthServiceApi _callAuthServiceApi;
        public ResourceOwnerPasswordValidator(ICallAuthServiceApi callAuthServiceApi)
        {
            _callAuthServiceApi = callAuthServiceApi;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // call api
            UserLoginRequest userLoginRequest = new UserLoginRequest() { UserCode = context.UserName, UserPassword = context.Password };
            UserLoginResponse userLoginResponse = await _callAuthServiceApi.UserLogin(userLoginRequest);
            if (!userLoginResponse.IsSuccess)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, userLoginResponse.Message);
            }
            else
            {
                context.Result = new GrantValidationResult(
                    subject: userLoginResponse.UserCode,
                    authenticationMethod: "custom"
                );
            }
        }


    }
}