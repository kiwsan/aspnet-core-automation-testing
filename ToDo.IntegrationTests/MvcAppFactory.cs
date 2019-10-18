using Common.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ToDo.IntegrationTests
{
    public class MvcAppFactory : CustomWebApplicationFactory<ToDo.Mvc.Startup>
    {
    }
}
