using HealthClaim.BAL.Repository.Concrete;
using HealthClaim.BAL.Repository.Interface;
using HealthClaim.Model.Dtos.Claims;
using HealthClaim.Model.Dtos.EmpAdvance;
using HealthClaim.Model.Dtos.Employee;
using HealthClaim.Model.Dtos.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthClaim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IEmpAdvanceRepository employeeAdvanceRepository;
        public ClaimController(IEmpAdvanceRepository _employeeAdvanceRepository)
        {
            employeeAdvanceRepository = _employeeAdvanceRepository;
        }

        [HttpGet]
        public async Task<ResponeModel> Get()
        {
            var response = await employeeAdvanceRepository.Get(0);
            return response;
        }
        [HttpGet("{id}")]
        public async Task<ResponeModel> Get(int id)
        {
            var response = await employeeAdvanceRepository.Get(id);
            return response;
        }

        //[HttpPost]
        //public async Task<ResponeModel> Post([FromBody] EmpAdvanceModel employeeAdvance)
        //{
        //    var response = await employeeAdvanceRepository.Create(employeeAdvance);
        //    return response;
        //}

        // POST api/<AdvanceClaimController>
        [HttpPost("AdvanceRequest")]
        //[Route("/api/CreateEmpRelation")]
        public async Task<ResponeModel> AdvanceRequest([FromForm] EmpAdvanceModel empAdvanceModel)
        {
            var response = await employeeAdvanceRepository.AdvanceRequest(empAdvanceModel);
            return response;
        }

        // POST api/<AdvanceClaimController>
        [HttpPost("SubmitEmpClaimProcess")]
        //[Route("/api/CreateEmpRelation")]
        public async Task<ResponeModel> SubmitEmpClaimProcess(EmpClaimProcessDetailsModel empClaimProcess)
        {
            var response = await employeeAdvanceRepository.SubmitEmpClaimProcessDetails(empClaimProcess);
            return response;
        }


        [HttpPut("{id}")]
        public async Task<ResponeModel> Put(int id, [FromBody] EmpAdvanceUpdateModel employeeAdvance)
        {
            var response = await employeeAdvanceRepository.Update(employeeAdvance, id);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ResponeModel> Delete(int id)
        {
            var response = await employeeAdvanceRepository.Delete(id);
            return response;
        }
        
        // GET api/<EmployeeController>/5
        [HttpGet("GetAdvanceClaimApproved")]
        public async Task<ResponeModel> GetAdvanceClaimApproved(long? empId)
        {
            var response = await employeeAdvanceRepository.GetAdvanceClaimApproved(empId);
            return response;
        }
        // GET api/<EmployeeController>/5
        [HttpGet("GetAdvanceClaim")]
        public async Task<ResponeModel> GetAdvanceClaim(long? empId)
        {
            var response = await employeeAdvanceRepository.GetAdvanceClaimRequest(empId);
            return response;
        }
        // GET api/<EmployeeController>/5
        [HttpGet("GetDirectClaimApproved")]
        public async Task<ResponeModel> GetDirectClaimApproved(long? empId)
        {
            var response = await employeeAdvanceRepository.GetDirectClaimApproved(empId);
            return response;
        }
        // GET api/<EmployeeController>/5
        [HttpGet("GetDirectClaimRequest")]
        public async Task<ResponeModel> GetDirectClaimRequest(long? empId)
        {
            var response = await employeeAdvanceRepository.GetDirectClaimRequest(empId);
            return response;
        }

        // POST api/<ClaimController>
        [HttpPost("DirectClaimRequest")]
        //[Route("/api/CreateEmpRelation")]
        public async Task<ResponeModel> DirectClaimRequest([FromForm] PatientAndOtherDetailsModel patientAndOtherDetails)
        {
            var response = await employeeAdvanceRepository.DirectCreateClaim(patientAndOtherDetails);
            return response;
        }

        // GET api/<ClaimController>/25
        // This is for view the details for HR
        [HttpGet("GetAdvanceDetails/{advanceId}")]
        public async Task<ResponeModel> GetAdvanceDetails(long advanceId)
        {
            var url = HttpContext.Request.Scheme + "/" + HttpContext.Request.Host + "/";
            var response = await employeeAdvanceRepository.GetAdvanceDetails(advanceId, url);
            return response;
        }
    }
}
