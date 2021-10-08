using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("[controller]")] 
    [ApiController] 
    public class WeatherForecastController : ControllerBase 
    { 
        private readonly IRepositoryManager _repository; 
        public WeatherForecastController(IRepositoryManager repository) 
        { 
            _repository = repository; 
        }
        [HttpGet] 
        public ActionResult<IEnumerable<string>> Get() 
        {
            //_repository.Company.AnyMethodFromCompanyRepository(); 
            //_repository.Employee.AnyMethodFromEmployeeRepository(); 
            return new string[] { "value1", "value2" }; 
        } 
    }
}
