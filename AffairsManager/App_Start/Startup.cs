﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using AffairsManager.Models;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;

[assembly: OwinStartup(typeof(AffairsManager.Startup))]

namespace AffairsManager
{
    public class Startup
    {
       
            public void Configuration(IAppBuilder app)
            {
                // настраиваем контекст и менеджер
                app.CreatePerOwinContext<ApplicationContext>(ApplicationContext.Create);
                app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Account/Login"),
                });
            }
    }
}