using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using FEMEE.Domain.Enums;

namespace FEMEE.Application.Interfaces.Services
{
    public interface IAuthorizationService
    {
        Task<bool> HasPermissionAsync(int userId,  Permissions permission);
        Task<bool> HasRoleAsync (int userId, TipoUsuario role);

        Task<IEnumerable<Permissions>> GetUserPermissionsAsync(int userId);

        Task<bool> IsResourceOwnerAsync(int userId, int resourceOwnerId);
    }
}