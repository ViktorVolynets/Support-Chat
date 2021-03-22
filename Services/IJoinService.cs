using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    public interface IJoinService
    {
        /// <summary>
        /// Checks if can create a new user
        /// </summary>
        /// <param name="model">A model that includes user credentials</param>
        /// <returns>Returns true if can create a new user</returns>
        public Task<bool> canJoin(JoinModel model);

        /// <summary>
        /// Register a new client
        /// </summary>
        public Task JoinClient(JoinModel model);

        /// <summary>
        /// Register a new employee.
        /// Requires admin access to process.
        /// </summary>
        public Task<bool> JoinEmployee(JoinEmployeeModel model);

        /// <summary>
        /// Register a new admin.
        /// Requires admin access and specific admin-creation token.
        /// </summary>
        public Task<bool> JoinAdmin(JoinModel model);
    }
}
