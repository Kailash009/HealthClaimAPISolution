using HealthClaim.Model.Dtos.Common;
using HealthClaim.Model.Dtos.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthClaim.BAL.Repository.Interface
{
    public interface ICommanRepository
    {
        public Task<ResponeModel> CreateUploadDocType(UplodDocTypeModel uplodDocType);
        public Task<ResponeModel> UpdateUploadDocType(UplodDocTypeModel uplodDocType, int id);
        public Task<ResponeModel> DeleteUploadDocType(int id);
        public Task<ResponeModel> Get(int? id);
        public Task<ResponeModel> GetEmpRelations(int? id);
        public Task<ResponeModel> CreateHospitalAccountDetail(HospitalAccountDetailDto hospitalAccountDetailDto);
        public Task<ResponeModel> UpdateHospitalAccountDetail(HospitalAccountDetailDto hospitalAccountDetailDto, int id);
        public Task<ResponeModel> DeleteHospitalAccountDetail(int id);
        public Task<ResponeModel> GetHospitalAccountDetail(int? id);
        public  Task<ResponeModel> IsIFSCCodeValid(string ifscCode);
        public  Task<List<UploadDocumentResponseModel>> UploadDocumets(List<IFormFile> files, string folderPathName = "AdmissionAdvice");

    }
}
