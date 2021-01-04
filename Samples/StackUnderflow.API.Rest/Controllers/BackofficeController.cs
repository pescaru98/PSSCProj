﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Access.Primitives.Extensions.ObjectExtensions;
using Access.Primitives.IO;
using Access.Primitives.EFCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using StackUnderflow.Domain.Core;
using StackUnderflow.Domain.Core.Contexts;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.EF.Models;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using StackUnderflow.Domain.Schema.Backoffice;

using LanguageExt;

using Orleans;
using GrainInterfaces;

namespace StackUnderflow.API.Rest.Controllers
{
    [ApiController]
    [Route("backoffice")]
    public class BackofficeController : ControllerBase
    {
        private readonly IInterpreterAsync _interpreter;
        private readonly StackUnderflowContext _dbContext;
        private readonly IClusterClient _client;

        public BackofficeController(IInterpreterAsync interpreter, StackUnderflowContext dbContext, IClusterClient client)
        {
            _interpreter = interpreter;
            _dbContext = dbContext;
            _client = client;
        }

        [HttpPost("tenant")]
        public async Task<IActionResult> CreateTenantAsyncAndInviteAdmin([FromBody] CreateTenantCmd createTenantCmd)
        {

            var dbTenants = _dbContext.Tenant.ToList();
            var dbTenantUsers = _dbContext.TenantUser.ToList();
            var dbUsers = _dbContext.User.ToList();

            _dbContext.Tenant.AttachRange(dbTenants);
            _dbContext.TenantUser.AttachRange(dbTenantUsers);
            _dbContext.User.AttachRange(dbUsers);

            BackofficeWriteContext ctx = new BackofficeWriteContext(
                new EFList<Tenant>(_dbContext.Tenant),
                new EFList<TenantUser>(_dbContext.TenantUser),
                new EFList<User>(_dbContext.User));

            var dependencies = new BackofficeDependencies();
            dependencies.GenerateInvitationToken = () => Guid.NewGuid().ToString();
            dependencies.SendInvitationEmail = SendEmail;

            var expr = from createTenantResult in BackofficeDomain.CreateTenant(createTenantCmd)
                       let adminUser = createTenantResult.SafeCast<CreateTenantResult.TenantCreated>().Select(p => p.AdminUser)
                       let inviteAdminCmd = new InviteTenantAdminCmd(adminUser)
                       from inviteAdminResult in BackofficeDomain.InviteTenantAdmin(inviteAdminCmd)
                       select new { createTenantResult, inviteAdminResult };

            var r = await _interpreter.Interpret(expr, ctx, dependencies);
            _dbContext.SaveChanges();
            return r.createTenantResult.Match(
                created => (IActionResult)Ok(created.Tenant.TenantId),
                notCreated => StatusCode(StatusCodes.Status500InternalServerError, "Tenant could not be created."),//todo return 500 (),
            invalidRequest => BadRequest("Invalid request."));
        }

        private TryAsync<InvitationAcknowledgement> SendEmail(InvitationLetter letter)
        => async () =>
        {
            var emialSender = _client.GetGrain<IEmailSender>(0);
            await emialSender.SendEmailAsync(letter.Letter);
            return new InvitationAcknowledgement(Guid.NewGuid().ToString());
        };
    }
}
