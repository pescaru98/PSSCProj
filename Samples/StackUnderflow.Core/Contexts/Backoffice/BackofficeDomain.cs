using System;
using System.Collections.Generic;
using System.Text;
using Access.Primitives.IO;
using LanguageExt;
using StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp;
using StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp;
using StackUnderflow.Domain.Schema.Backoffice.InviteUserOp;
using StackUnderflow.Domain.Schema.Backoffice.SetPermissionsOp;
using StackUnderflow.EF.Models;
using static PortExt;
using static StackUnderflow.Domain.Schema.Backoffice.CreateTenantOp.CreateTenantResult;
using static StackUnderflow.Domain.Schema.Backoffice.InviteTenantAdminOp.InviteTenantAdminResult;
using static StackUnderflow.Domain.Schema.Backoffice.InviteUserOp.InviteUserResult;

namespace StackUnderflow.Domain.Core
{
    public static class BackofficeDomain
    {
        public static Port<ICreateTenantResult> CreateTenant(CreateTenantCmd command) => NewPort<CreateTenantCmd, ICreateTenantResult>(command);

        public static Port<IInviteTenantAdminResult> InviteTenantAdmin(InviteTenantAdminCmd command) => NewPort<InviteTenantAdminCmd, IInviteTenantAdminResult>(command);

        public static Port<IInviteUserResult> InviteUser(InviteUserCmd command) => NewPort<InviteUserCmd, IInviteUserResult>(command);

        public static Port<SetPermissionResult> SetPermissions(SetPermissionCmd command) => NewPort<SetPermissionCmd, SetPermissionResult>(command);
    }
}

