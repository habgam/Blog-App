using CloudApp.Data;
using CloudApp.Data.Model;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace CloudApp.Admin.Membership
{
    public class UserStoreService : IUserStore<CUser>,
     IUserPasswordStore<CUser>
    {
        DbDataContext _context;
        public UserStoreService(DbDataContext context)
        {
            _context = context;
        }

        public Task CreateAsync(CUser user)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(CUser user)
        {
            throw new NotImplementedException();
        }
        public Task<CUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }
       
        public Task<CUser> FindByNameAsync(string userName)
        {
            
            
            Task<CUser> task =
            _context.Users.Where(apu => apu.UserName == userName)
            .FirstOrDefaultAsync();

            return task;
        }
        public Task UpdateAsync(CUser user)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public Task<string> GetPasswordHashAsync(CUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Password);
        }
        public Task<bool> HasPasswordAsync(CUser user)
        {
            return Task.FromResult(user.Password != null);
        }
        public Task SetPasswordHashAsync(
          CUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}