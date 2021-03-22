using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Models;
using TechnicalSupport.Models.ServiceModels;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Services
{
    public interface IAdminServiceProvider
    {
        public Task<List<Client>> GetClientListAsync();

        public Task<bool> ChangeClientAsync(Client _client);

        public Task<List<Employee>> GetEmployeeListAsync();

        public Task<bool> CreateEmployeeAsync(JoinEmployeeModel model);

        public Task<bool> ChangeEmployeeAsync(Employee _employee);

        public Task<bool> CreateAdminAsync(JoinAdminModel model);

        /// <summary>
        /// Create specific number of tokens and write them both in file and db
        /// </summary>
        public Task CreateTokensAsync();

        /// <summary>
        /// Allows to get last error logs
        /// </summary>
        /// <returns>List of logs</returns>
        public List<ErrorLog> GetErrorLogs();

        /// <summary>
        /// Allows to get last trace logs
        /// </summary>
        /// <returns></returns>
        public List<TraceLog> GetTraceLogs();

        /// <summary>
        /// Get specified error log
        /// </summary>
        /// <param name="id">Log id</param>
        public Task<ErrorLog> GetErrorLogAsync(int id);

        public Task<int> GetActiveOperatorsAsync();
    }
}
