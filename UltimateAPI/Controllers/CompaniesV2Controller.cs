using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;

namespace WebAPI.Controllers
{
    [ApiVersion("2.0", Deprecated = true)]
    [Route("api/companies")]
    [ApiController]
    public class CompaniesV2Controller : Controller
    {
        private readonly IRepositoryManager _repository; 
        public CompaniesV2Controller(IRepositoryManager repository) 
        { 
            _repository = repository; 
        }

        [HttpGet] 
        public async Task<IActionResult> GetCompanies() 
        { 
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges: false); 
            return Ok(companies); 
        }
    }
}
