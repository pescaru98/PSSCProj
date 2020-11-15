using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Microsoft.AspNetCore.Mvc;
using StackUnderflow.Domain.Core;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.EF.Models;
using Access.Primitives.EFCore;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;

namespace StackUnderflow.API.Rest.Controllers
{
    [ApiController]
    [Route("backoffice")]
    public class BackofficeController : ControllerBase
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly StackUnderflowContext _dbContext;

        public BackofficeController(IInterpreterAsync interpreter, StackUnderflowContext dbContext)
        {
            _interpreter = interpreter;
            _dbContext = dbContext;
        }

        [HttpPost("createTenant")]
        public async Task<IActionResult> CreateTenantAsyncAndInviteAdmin([FromBody] CreateTenantCmd createTenantCmd)
        {
            BackofficeWriteContext ctx = await LoadDbContext(createTenantCmd);

            var expr = from createTenantResult in BackofficeDomain.CreateTenant(createTenantCmd)
                       let adminUser = createTenantResult.SafeCast<CreateTenantResult.TenantCreated>().Select(p => p.AdminUser)
                       let inviteAdminCmd = new InviteTenantAdminCmd(adminUser)
                       from inviteAdminResult in BackofficeDomain.InviteTenantAdmin(inviteAdminCmd)
                       select new { createTenantResult, inviteAdminResult };

            var r = await _interpreter.Interpret(expr, ctx);

            await _dbContext.SaveChangesAsync();

            return r.createTenantResult.Match(
                created => (IActionResult)Ok(created.Tenant.TenantId),
                notCreated => BadRequest("Tenant could not be created."),
                invalidRequest => BadRequest("Invalid request."));
        }

        async Task<BackofficeWriteContext> LoadDbContext(CreateTenantCmd createTenantCmd)
        {
            return await _dbContext.LoadAsync("dbo.BackofficeHttpController", new
            {
                OrganisationId = createTenantCmd.OrganisationId
            }, async reader =>
            {
                var tenants = await reader.ReadAsync<Tenant>();
                var tenantUsers = await reader.ReadAsync<TenantUser>();
                var users = await reader.ReadAsync<User>();

                _dbContext.AttachRange(tenants);
                _dbContext.AttachRange(tenantUsers);
                _dbContext.AttachRange(users);
                return new BackofficeWriteContext(
                    new EFList<Tenant>(_dbContext.Tenant),
                    new EFList<TenantUser>(_dbContext.TenantUser),
                    new EFList<User>(_dbContext.User));
            });
        }
    }
}
