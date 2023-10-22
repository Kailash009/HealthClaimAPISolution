using HealthClaim.DAL.Models;
using HealthClaim.Model.Dtos.Claims;
using HealthClaim.Model.Dtos.Common;
using HealthClaim.Model.Dtos.EmpAdvance;
using HealthClaim.Model.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthClaim.BAL.Repository.Interface
{
    public interface IEmpAdvanceRepository : IGenricRepository<EmpAdvance>
    {
        public Task<ResponeModel> Create(EmpAdvanceModel empAdvanceModel);
        public Task<ResponeModel> Update(EmpAdvanceUpdateModel empAdvanceModel, int id);
        public Task<ResponeModel> Delete(int id);
        public Task<ResponeModel> Get(int? id);
        Task<ResponeModel> AdvanceRequest(EmpAdvanceModel empAdvanceModel);
        Task<ResponeModel> GetAdvanceClaimApproved(long? empId);
        Task<ResponeModel> GetAdvanceClaimRequest(long? empId);
        Task<ResponeModel> GetDirectClaimApproved(long? empId);
        Task<ResponeModel> GetDirectClaimRequest(long? empId);
        Task<ResponeModel> DirectCreateClaim(PatientAndOtherDetailsModel patientAndOtherDetails);
        public Task<ResponeModel> SubmitEmpClaimProcessDetails(EmpClaimProcessDetailsModel claimProcessDetailsModel);
        Task<ResponeModel> GetAdvanceDetails(long advanceId, string url);
    }
}
