using System;
using System.Threading.Tasks;

namespace extid_invite_web
{
    public interface IInviteService
    {
        Task InviteUser(AppSettings settings, string dispalyName, string email);
    }
}