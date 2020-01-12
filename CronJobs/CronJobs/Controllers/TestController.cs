using CronJobs.Data.Entity;
using CronJobs.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CronJobs.Controllers
{
    public class TestController:BaseController
    {
        private readonly IUserRepository _userRepository;

        public TestController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("List")]
        public async Task<List<User>> List()
        {
            return await _userRepository.GetListAsync();
        }

        [HttpPost("Add")]
        public async Task<string> Add([FromBody] User user)
        { 
            await _userRepository.AddAsync(user);
            return user.Id;
        }

        [HttpPost("Delete")]
        public async Task<DeleteResult> Delete([FromBody]string id)
        {
             return await _userRepository.DeleteOneAsync(c=>c.Id==id);
              
        }

    }
}
