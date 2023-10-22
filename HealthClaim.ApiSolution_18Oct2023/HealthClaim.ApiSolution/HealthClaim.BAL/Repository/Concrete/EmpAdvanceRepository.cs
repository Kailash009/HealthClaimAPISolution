using HealthClaim.BAL.Repository.Interface;
using HealthClaim.DAL;
using HealthClaim.DAL.Models;
using HealthClaim.Model.Dtos.Claims;
using HealthClaim.Model.Dtos.Common;
using HealthClaim.Model.Dtos.EmpAdvance;
using HealthClaim.Model.Dtos.Employee;
using HealthClaim.Model.Dtos.Response;
using HealthClaim.Utility.Eumus;
using HealthClaim.Utility.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HealthClaim.BAL.Repository.Concrete
{
    public class EmpAdvanceRepository : GenricRepository<EmpAdvance>, IEmpAdvanceRepository
    {
        private HealthClaimDbContext _dbContext;
        private ICommanRepository _commandRepo;
        #region Constructor
        /// <summary>
        /// This is constructor to set dependency injection
        /// </summary>
        /// <param name="db"></param>
        public EmpAdvanceRepository(HealthClaimDbContext db, ICommanRepository commandRepo) : base(db)
        {
            _dbContext = db;
            _commandRepo = commandRepo;
            //_commandRepo = commandRepo;
        }
        #endregion
        public async Task<ResponeModel> Create(EmpAdvanceModel empAdvanceModel)
        {
            ResponeModel responeModel = new ResponeModel();
            if (empAdvanceModel != null)
            {
                EmpAdvance employeeAdvance = new EmpAdvance()
                {
                    EmplId = empAdvanceModel.EmplId,
                    PatientId = empAdvanceModel.PatientId,
                    RequestSubmittedById = empAdvanceModel.RequestSubmittedById,
                    RequestName = empAdvanceModel.RequestName,
                    AdvanceRequestDate = empAdvanceModel.AdvanceRequestDate,
                    AdvanceAmount = empAdvanceModel.AdvanceAmount,
                    Reason = empAdvanceModel.Reason,
                    PayTo = empAdvanceModel.PayTo,
                    //ApprovalDate = DateTime.Now,
                    //ApprovedAmount = empAdvanceModel.ApprovedAmount,
                    //ApprovedById = empAdvanceModel.ApprovedById,
                    HospitalName = empAdvanceModel.HospitalName,
                    HospitalRegNo = empAdvanceModel.HospitalName,
                    DateOfAdmission = empAdvanceModel.DateOfAdmission,
                    DoctorName = empAdvanceModel.DoctorName,
                };
                _dbContext.Add(employeeAdvance);
                int id = await _dbContext.SaveChangesAsync();
                responeModel.Data = employeeAdvance;
                responeModel.StatusCode = System.Net.HttpStatusCode.Created;
                responeModel.Error = false;
                responeModel.Message = CommonMessage.CreateMessage;

            }
            return responeModel;
        }

        public async Task<ResponeModel> Delete(int id)
        {
            ResponeModel responeModel = new ResponeModel();
            if (id != 0)
            {
                var employeeAdvanceDetails = _dbContext.EmpAdvances.Where(e => e.Id == id).FirstOrDefault();

                if (employeeAdvanceDetails != null)
                {
                    employeeAdvanceDetails.IsActive = false;
                    await _dbContext.SaveChangesAsync();
                    responeModel.Data = null;
                    responeModel.StatusCode = System.Net.HttpStatusCode.OK;
                    responeModel.Error = false;
                    responeModel.Message = "Employee Advance deleted successfully.";

                }
            }
            return responeModel;
        }

        public async Task<ResponeModel> Get(int? id)
        {
            ResponeModel responeModel = new ResponeModel();
            var employeesAdvance = _dbContext.EmpAdvances.Where(e => id == 0 ? e.Id == e.Id : e.Id == id && e.IsActive == true).ToList();
            responeModel.Data = employeesAdvance;
            responeModel.DataLength = employeesAdvance.Count;
            responeModel.StatusCode = System.Net.HttpStatusCode.OK;
            responeModel.Error = false;
            responeModel.Message = employeesAdvance.Count + " Employee Advance found.";

            return responeModel;
        }

        public async Task<ResponeModel> Update(EmpAdvanceUpdateModel empAdvanceModel, int id)
        {
            ResponeModel responeModel = new ResponeModel();
            if (empAdvanceModel != null && id != 0)
            {
                var employeeAdvanceUpdateDetails = _dbContext.EmpAdvances.Where(e => e.Id == id).FirstOrDefault();

                if (employeeAdvanceUpdateDetails != null)
                {
                    employeeAdvanceUpdateDetails.ApprovalDate = empAdvanceModel.ApprovalDate;
                    employeeAdvanceUpdateDetails.ApprovedAmount = empAdvanceModel.ApprovedAmount;
                    employeeAdvanceUpdateDetails.ApprovedById = empAdvanceModel.ApprovedById;
                    employeeAdvanceUpdateDetails.UpdatedDate = DateTime.Now;

                    await _dbContext.SaveChangesAsync();
                    responeModel.Data = employeeAdvanceUpdateDetails;
                    responeModel.StatusCode = System.Net.HttpStatusCode.Created;
                    responeModel.Error = false;
                    responeModel.Message = CommonMessage.UpdateMessage;

                }

            }
            return responeModel;
        }
        /// <summary>
        /// Get advance list for the employee on the base of employee Id 
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>

        public async Task<ResponeModel> AdvanceRequest(EmpAdvanceModel empAdvanceModel)
        {
            ResponeModel responeModel = new ResponeModel();
            if (empAdvanceModel != null)
            {
                // firt add record in EmpClaim

                EmpClaim empClaim = new EmpClaim()
                {
                    IsSpecailDisease = false,
                    HospitalTotalBill = empAdvanceModel.EstimateAmount,
                    ClaimAmount = empAdvanceModel.EstimateAmount,
                    ClaimRequetsDate = DateTime.Now,
                    CreatedBy = empAdvanceModel.EmplId,
                    CreatedDate = DateTime.Now
                };

                _dbContext.Add(empClaim);
                await _dbContext.SaveChangesAsync();

                // then add record in EmpAdvance
                EmpAdvance employeeAdvanceData = new EmpAdvance()
                {
                    EmplId = empAdvanceModel.EmplId,
                    PatientId = empAdvanceModel.PatientId,
                    ClaimId = empClaim.Id,
                    RequestSubmittedById = empAdvanceModel.RequestSubmittedById,
                    RequestName = empAdvanceModel.RequestName,
                    AdvanceRequestDate = empAdvanceModel.AdvanceRequestDate, // Is input reuired or DateTime.Now() ?
                    AdvanceAmount = empAdvanceModel.AdvanceAmount,
                    Reason = empAdvanceModel.Reason,
                    PayTo = empAdvanceModel.PayTo,
                    Claim_TypeId = ((long)RecordMasterClaimTypes.AdvanceClaim),
                    //ApprovalDate = empAdvanceModel.ApprovalDate,
                    //ApprovedAmount = empAdvanceModel.ApprovedAmount,
                    // ApprovedById = empAdvanceModel.ApprovedById,
                    HospitalName = empAdvanceModel.HospitalName,
                    HospitalRegNo = empAdvanceModel.HospitalRegNo,
                    DateOfAdmission = empAdvanceModel.DateOfAdmission,
                    DoctorName = empAdvanceModel.DoctorName,
                    CreatedDate = DateTime.Now,
                    CreatedBy = empAdvanceModel.EmplId,
                    IsActive = true

                };

                _dbContext.Add(employeeAdvanceData);
                await _dbContext.SaveChangesAsync();

                // then add record in UploadTypeDetail
                UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                {
                    ClaimId = empClaim.Id,
                    ClaimTypeId = ((long)empAdvanceModel.ClaimTypeId),
                    UploadTypeId = ((long)empAdvanceModel.UploadTypeId),
                    Amount = empAdvanceModel.AdvanceAmount,
                    CreatedBy = empAdvanceModel.EmplId,
                    CreatedDate = DateTime.Now
                };
                _dbContext.Add(uploadTypeDetail);
                await _dbContext.SaveChangesAsync();


                List<UploadDocument> uploadedDocuments = await FileUpload(empAdvanceModel, uploadTypeDetail.Id);

                await _dbContext.AddRangeAsync(uploadedDocuments);
                await _dbContext.SaveChangesAsync();

                // Add status in EmpClaimStatus

                EmpClaimStatus empClaimStatus = new EmpClaimStatus()
                {
                    ClaimId = empClaim.Id,
                    ClaimTypeId = ((long)empAdvanceModel.ClaimTypeId),
                    StatusId = ((long)empAdvanceModel.StatusId),
                    CreatedBy = empAdvanceModel.EmplId,
                    CreatedDate = DateTime.Now
                };

                await _dbContext.AddAsync(empClaimStatus);
                await _dbContext.SaveChangesAsync();

                EmpClaimProcessDetailsModel claimProcessDetailsModel = new EmpClaimProcessDetailsModel()
                {
                    ClaimId = empClaim.Id,
                    ClaimTypeId = RecordMasterClaimTypes.Advance,
                    SenderId = empAdvanceModel.EmplId,
                    RecipientId = ((long)RecordMasterIds.HRID),
                    CreatedBy = empAdvanceModel.EmplId,
                };
                await SubmitEmpClaimProcessDetails(claimProcessDetailsModel);

                if (empAdvanceModel.PayTo == "Hospital" || empAdvanceModel.PayTo == "hospital")
                {
                    HospitalAccountDetail hospitalAccountDetail = new HospitalAccountDetail()
                    {
                        BankName = empAdvanceModel.BankName,
                        IfscCode = empAdvanceModel.IFSCCode,
                        AccountNumber = empAdvanceModel.BeneficiaryAccountNo,
                        BeneficiaryName = empAdvanceModel.BeneficiaryName,
                        EmpAdvanceId = employeeAdvanceData.Id,
                        CreatedBy = empAdvanceModel.EmplId,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.Add(hospitalAccountDetail);
                    await _dbContext.SaveChangesAsync();
                }

                var responseData = new { emplId = employeeAdvanceData.EmplId, patientId = employeeAdvanceData.PatientId, empclaimId = employeeAdvanceData.ClaimId, claimId = employeeAdvanceData.Id, requestSubmittedById = employeeAdvanceData.RequestSubmittedById };

                responeModel.Data = responseData;
                responeModel.StatusCode = System.Net.HttpStatusCode.Created;
                responeModel.Error = false;
                responeModel.Message = CommonMessage.CreateMessage;
            }
            return responeModel;
        }

        public async Task<ResponeModel> SubmitEmpClaimProcessDetails(EmpClaimProcessDetailsModel claimProcessDetailsModel)
        {
            ResponeModel responeModel = new ResponeModel();
            EmpClaimProcessDetails empClaimProcessDetails = new EmpClaimProcessDetails()
            {
                ClaimId = claimProcessDetailsModel.ClaimId,
                ClaimTypeId = ((long)claimProcessDetailsModel.ClaimTypeId),
                SenderId = claimProcessDetailsModel.SenderId,
                RecipientId = claimProcessDetailsModel.RecipientId,
                CreatedBy = claimProcessDetailsModel.CreatedBy,
                CreatedDate = DateTime.Now,
                IsActive = true,

            };

            await _dbContext.AddAsync(empClaimProcessDetails);
            await _dbContext.SaveChangesAsync();

            return responeModel;
        }

        /// <summary>
        /// For uploading files from Emp Advance Model
        /// </summary>
        /// <param name="empAdvanceModel"></param>
        /// <returns></returns>
        private async Task<List<UploadDocument>> FileUpload(EmpAdvanceModel empAdvanceModel, long AdvanceUploadTypeId)
        {
            List<UploadDocument> uploadedDocuments = new List<UploadDocument>();

            // Your existing file upload logic...
            List<string> filePathsAdmissionAdviceFile = new List<string>();
            // Handle AdmissionAdviceFile
            if (empAdvanceModel.AdmissionAdviceFile != null && empAdvanceModel.AdmissionAdviceFile.Count > 0)
            {
                foreach (var file in empAdvanceModel.AdmissionAdviceFile)
                {
                    if (file.Length > 0)
                    {
                        string basePath = "wwwroot/UploadDocuments/AdmissionAdvice";
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePathName = basePath;
                        if (!Directory.Exists(filePathName))
                        {
                            Directory.CreateDirectory(filePathName);
                        }
                        string filePath = Path.Combine(filePathName, fileName); // Save in 'admission_advice' folder
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        // Save the file path to the model
                        filePathsAdmissionAdviceFile.Add(filePath);
                    }
                }
            }
            List<string> diagnosisFile = new List<string>();
            if (empAdvanceModel.DiagnosisFile != null && empAdvanceModel.DiagnosisFile.Count > 0)
            {
                foreach (var file in empAdvanceModel.DiagnosisFile)
                {
                    if (file.Length > 0)
                    {
                        string basePath = "wwwroot/UploadDocuments/DiagnosisUpload";
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePathName = basePath;
                        if (!Directory.Exists(filePathName))
                        {
                            Directory.CreateDirectory(filePathName);

                        }
                        string filePath = Path.Combine(filePathName, fileName); // Save in 'admission_advice' folder
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        // Save the file path to the model
                        diagnosisFile.Add(filePath);
                    }
                }
            }
            List<string> estimateAmountFile = new List<string>();
            if (empAdvanceModel.EstimateAmountFile != null && empAdvanceModel.EstimateAmountFile.Count > 0)
            {
                foreach (var file in empAdvanceModel.EstimateAmountFile)
                {
                    if (file.Length > 0)
                    {
                        string basePath = "wwwroot/UploadDocuments/EstimateAmountFile";
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string filePathName = basePath;
                        if (!Directory.Exists(filePathName))
                        {
                            Directory.CreateDirectory(filePathName);

                        }
                        string filePath = Path.Combine(filePathName, fileName); // Save in 'admission_advice' folder
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        // Save the file path to the model
                        estimateAmountFile.Add(filePath);
                    }
                }
            }
            // Add the file paths and other details to the UploadDocument list
            foreach (var filePath in filePathsAdmissionAdviceFile)
            {
                var admissionAdviceDocument = new UploadDocument
                {
                    AdvanceUploadTypeId = AdvanceUploadTypeId, // Set the appropriate type ID
                    //Amount = empAdvanceModel.AdvanceAmount,
                    FileName = Path.GetFileName(filePath),
                    PathUrl = filePath,
                    Comment = "Admission Advice",
                    CreatedBy = empAdvanceModel.EmplId,
                    CreatedDate = DateTime.Now
                };
                uploadedDocuments.Add(admissionAdviceDocument);
            }

            foreach (var filePath in diagnosisFile)
            {
                var diagnosisDocument = new UploadDocument
                {
                    AdvanceUploadTypeId = AdvanceUploadTypeId, // Set the appropriate type ID
                                                               // Amount = empAdvanceModel.AdvanceAmount,
                    FileName = Path.GetFileName(filePath),
                    PathUrl = filePath,
                    Comment = "Diagnosis Upload",
                    CreatedBy = empAdvanceModel.EmplId,
                    CreatedDate = DateTime.Now

                };
                uploadedDocuments.Add(diagnosisDocument);
            }

            foreach (var filePath in estimateAmountFile)
            {
                var estimateAmountDocument = new UploadDocument
                {
                    AdvanceUploadTypeId = AdvanceUploadTypeId, // Set the appropriate type ID
                    Amount = empAdvanceModel.AdvanceAmount,
                    FileName = Path.GetFileName(filePath),
                    PathUrl = filePath,
                    Comment = "Estimate Amount File",
                    CreatedBy = empAdvanceModel.EmplId,
                    CreatedDate = DateTime.Now

                };
                uploadedDocuments.Add(estimateAmountDocument);
            }
            return uploadedDocuments;
        }

        //Added On 16 Oct

        private async Task<List<UploadDocument>> UploadBillFiles(List<IFormFile> files, string floderName)
        {
            bool status = false;
            var response = await _commandRepo.UploadDocumets(files, floderName);
            List<UploadDocument> uploadDocuments = new List<UploadDocument>();
            if (response != null && response.Count != 0)
            {
                foreach (var item in response)
                {
                    UploadDocument uploadDocument = new UploadDocument()
                    {
                        FileName = item.fileName,
                        PathUrl = item.filePath
                    };

                    uploadDocuments.Add(uploadDocument);
                }

            }
            return uploadDocuments;
        }

        #region Claim Create
        /// <summary>
        /// This method is used for add new employee
        /// </summary>
        /// <param name="patientAndOtherDetails"></param>
        /// <returns></returns>
        public async Task<ResponeModel> DirectCreateClaim(PatientAndOtherDetailsModel patientAndOtherDetails)
        {
            ResponeModel responeModel = new ResponeModel();
            if (patientAndOtherDetails != null)
            {
                // Add Employee direct claim

                EmpClaim empClaim = new EmpClaim()
                {
                    IsSpecailDisease = patientAndOtherDetails.IsSpecailDisease,
                    HospitalTotalBill = patientAndOtherDetails.FinalHospitalBill,
                    ClaimAmount = patientAndOtherDetails.ClaimAmount,
                    ClaimRequetsDate = DateTime.Now,
                    CreatedBy = patientAndOtherDetails.EmpId,
                    CreatedDate = DateTime.Now
                };
                _dbContext.Add(empClaim);
                await _dbContext.SaveChangesAsync();


                /// Then send data to the EmpClaimStatus
                EmpClaimStatus empClaimStatus = new EmpClaimStatus()
                {
                    ClaimId = empClaim.Id,
                    ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                    StatusId = 1,
                    CreatedBy = patientAndOtherDetails.EmpId,
                    CreatedDate = DateTime.Now
                };
                _dbContext.Add(empClaimStatus);
                await _dbContext.SaveChangesAsync();

                /// Then send data into EmpClaimBill

                EmpClaimBill empClaimBill = new EmpClaimBill()
                {
                    EmpClaimId = empClaim.Id,
                    EmpId = patientAndOtherDetails.EmpId,
                    StatusId = 1,
                    HospitalCompleteBill = patientAndOtherDetails.FinalHospitalBill,
                    MedicineBill = patientAndOtherDetails.MedicenBill.Amount,
                    ConsultationBill = patientAndOtherDetails.Consultation.Amount,
                    InvestigationBill = patientAndOtherDetails.Investigation.Amount,
                    RoomRentBill = patientAndOtherDetails.RoomRent.Amount,
                    OthersBill = patientAndOtherDetails.EmpId,
                    CreatedBy = patientAndOtherDetails.EmpId,
                    CreatedDate = DateTime.Now
                };

                _dbContext.Add(empClaimBill);
                await _dbContext.SaveChangesAsync();

                if (patientAndOtherDetails.AdmissionAdviceUpload != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.AdmissionAdviceUpload, "AdmissionAdvice");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.AdmissionAdviceUpload),
                        Amount = patientAndOtherDetails.MedicenBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                }
                if (patientAndOtherDetails.DischargeSummaryUpload != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.DischargeSummaryUpload, "DischargeSummary");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.AdmissionAdviceUpload),
                        Amount = patientAndOtherDetails.MedicenBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                }
                if (patientAndOtherDetails.InvestigationReportsUpload != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.InvestigationReportsUpload, "Investigation");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.AdmissionAdviceUpload),
                        Amount = patientAndOtherDetails.MedicenBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                }
                if (patientAndOtherDetails.DischargeSummaryUpload != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.DischargeSummaryUpload, "DischargeSummary");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.AdmissionAdviceUpload),
                        Amount = patientAndOtherDetails.MedicenBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                }





                if (patientAndOtherDetails.MedicenBill != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.MedicenBill.Files, "MedicenBill");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.Medicine),
                        Amount = patientAndOtherDetails.MedicenBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                }
                if (patientAndOtherDetails.Consultation != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.Consultation.Files, "Consultation");


                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.Consultation),
                        Amount = patientAndOtherDetails.Consultation.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                }
                if (patientAndOtherDetails.Investigation != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.Investigation.Files, "Investigation");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.Investigation),
                        Amount = patientAndOtherDetails.Investigation.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                }
                if (patientAndOtherDetails.OtherBill != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.OtherBill.Files, "OtherBill");


                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.Other),
                        Amount = patientAndOtherDetails.OtherBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                }


                // if bill is not in final bill then add to EmpClaimNotInMailBill table

                if (patientAndOtherDetails.MedicenNotFinalBill != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.MedicenNotFinalBill.Files, "MedicenNotFinalBill");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.MedicinenotinFinalBill),
                        Amount = patientAndOtherDetails.OtherBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                    EmpClaimNotInMailBill empClaimNotInMailBill = new EmpClaimNotInMailBill()
                    {
                        Amount = 0,
                        BillType = "",
                        EmpClaimBillId = empClaimBill.Id,
                        IsActive = true,
                        CreatedBy = empClaimBill.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(empClaimNotInMailBill);
                    await _dbContext.SaveChangesAsync();
                }
                if (patientAndOtherDetails.ConsultationNotFinalBill != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.ConsultationNotFinalBill.Files, "ConsultationNotFinalBill");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.ConsultationNotFinalBill),
                        Amount = patientAndOtherDetails.OtherBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                    EmpClaimNotInMailBill empClaimNotInMailBill = new EmpClaimNotInMailBill()
                    {
                        Amount = 0,
                        BillType = "",
                        EmpClaimBillId = empClaimBill.Id,
                        IsActive = true,
                        CreatedBy = empClaimBill.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(empClaimNotInMailBill);
                    await _dbContext.SaveChangesAsync();
                }
                if (patientAndOtherDetails.InvestigationNotFinalBill != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.InvestigationNotFinalBill.Files, "InvestigationNotFinalBill");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.InvestigationNotFinalBill),
                        Amount = patientAndOtherDetails.OtherBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();


                    EmpClaimNotInMailBill empClaimNotInMailBill = new EmpClaimNotInMailBill()
                    {
                        Amount = 0,
                        BillType = "",
                        EmpClaimBillId = empClaimBill.Id,
                        IsActive = true,
                        CreatedBy = empClaimBill.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(empClaimNotInMailBill);
                    await _dbContext.SaveChangesAsync();
                }
                if (patientAndOtherDetails.OtherBillNotFinalBill != null)
                {
                    var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.OtherBillNotFinalBill.Files, "OtherBillNotFinalBill");

                    UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                    {
                        ClaimId = empClaim.Id,
                        ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                        UploadTypeId = ((long)RecordMasterUplodDocType.InvestigationNotFinalBill),
                        Amount = patientAndOtherDetails.OtherBill.Amount,
                        CreatedBy = patientAndOtherDetails.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(uploadTypeDetail);
                    await _dbContext.SaveChangesAsync();

                    UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.Comment = ""; s.IsActive = true; return s; }).ToList();

                    await _dbContext.AddRangeAsync(UploadDocuments);
                    await _dbContext.SaveChangesAsync();

                    EmpClaimNotInMailBill empClaimNotInMailBill = new EmpClaimNotInMailBill()
                    {
                        Amount = 0,
                        BillType = "",
                        EmpClaimBillId = empClaimBill.Id,
                        IsActive = true,
                        CreatedBy = empClaimBill.EmpId,
                        CreatedDate = DateTime.Now
                    };

                    _dbContext.Add(empClaimNotInMailBill);
                    await _dbContext.SaveChangesAsync();
                }



                //Then send data to Advance
                EmpAdvance employeeAdvanceData = new EmpAdvance()
                {
                    EmplId = patientAndOtherDetails.EmpId,
                    PatientId = patientAndOtherDetails.PatientId,
                    ClaimId = empClaim.Id,
                    Claim_TypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                    RequestSubmittedById = patientAndOtherDetails.RequestSubmittedById,
                    RequestName = patientAndOtherDetails.RequestName,
                    AdvanceRequestDate = DateTime.Now, // Is input reuired or DateTime.Now() ?
                    AdvanceAmount = patientAndOtherDetails.AdvanceAmount,
                    Reason = patientAndOtherDetails.Reason,
                    PayTo = patientAndOtherDetails.PayTo,
                    ApprovalDate = patientAndOtherDetails.ApprovalDate,
                    ApprovedAmount = patientAndOtherDetails.ApprovedAmount,
                    // ApprovedById = empAdvanceModel.ApprovedById,
                    HospitalName = patientAndOtherDetails.HospitalName,
                    HospitalRegNo = patientAndOtherDetails.HospitalRegNo,
                    DateOfAdmission = patientAndOtherDetails.DateOfAdmission,
                    DoctorName = patientAndOtherDetails.DoctorName,
                    EstimatedAmount=patientAndOtherDetails.EstimatedAmount,
                    IsPreHospitalizationExpenses= patientAndOtherDetails.IsPreHospitalizationExpenses,
                    CreatedDate = DateTime.Now,
                    CreatedBy = patientAndOtherDetails.EmpId
                };

                _dbContext.Add(employeeAdvanceData);
                await _dbContext.SaveChangesAsync();

                EmpClaimProcessDetailsModel claimProcessDetailsModel = new EmpClaimProcessDetailsModel()
                {
                    ClaimId = empClaim.Id,
                    ClaimTypeId = RecordMasterClaimTypes.DirectClaim,
                    SenderId = patientAndOtherDetails.EmpId,
                    RecipientId = ((long)RecordMasterIds.HRID),
                    CreatedBy = patientAndOtherDetails.EmpId,
                };
                await SubmitEmpClaimProcessDetails(claimProcessDetailsModel);

                // Pre Hospitalization Expenses Save if IsPreHospitalizationExpenses is True
                if (patientAndOtherDetails.IsPreHospitalizationExpenses)
                {
                    if (patientAndOtherDetails.PreHospitalizationExpensesMedicine != null)
                    {
                        var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.PreHospitalizationExpensesMedicine.Files, "PreHospitalizationExpensesMedicine");

                        UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                        {
                            ClaimId = empClaim.Id,
                            ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                            UploadTypeId = ((long)RecordMasterUplodDocType.PreHospitalizationExpensesMedicine),
                            Amount = patientAndOtherDetails.PreHospitalizationExpensesMedicine.Amount,
                            CreatedBy = patientAndOtherDetails.EmpId,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                        };

                        _dbContext.Add(uploadTypeDetail);
                        await _dbContext.SaveChangesAsync();

                        UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                        await _dbContext.AddRangeAsync(UploadDocuments);
                        await _dbContext.SaveChangesAsync();

                    }
                    if (patientAndOtherDetails.PreHospitalizationExpensesConsultation != null)
                    {
                        var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.PreHospitalizationExpensesConsultation.Files, "PreHospitalizationExpensesConsultation");

                        UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                        {
                            ClaimId = empClaim.Id,
                            ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                            UploadTypeId = ((long)RecordMasterUplodDocType.PreHospitalizationExpensesConsultation),
                            Amount = patientAndOtherDetails.PreHospitalizationExpensesConsultation.Amount,
                            CreatedBy = patientAndOtherDetails.EmpId,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                        };

                        _dbContext.Add(uploadTypeDetail);
                        await _dbContext.SaveChangesAsync();

                        UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                        await _dbContext.AddRangeAsync(UploadDocuments);
                        await _dbContext.SaveChangesAsync();

                    }
                    if (patientAndOtherDetails.PreHospitalizationExpensesInvestigation != null)
                    {
                        var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.PreHospitalizationExpensesInvestigation.Files, "PreHospitalizationExpensesInvestigation");

                        UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                        {
                            ClaimId = empClaim.Id,
                            ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                            UploadTypeId = ((long)RecordMasterUplodDocType.PreHospitalizationExpensesInvestigation),
                            Amount = patientAndOtherDetails.PreHospitalizationExpensesInvestigation.Amount,
                            CreatedBy = patientAndOtherDetails.EmpId,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                        };

                        _dbContext.Add(uploadTypeDetail);
                        await _dbContext.SaveChangesAsync();

                        UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                        await _dbContext.AddRangeAsync(UploadDocuments);
                        await _dbContext.SaveChangesAsync();

                    }
                    if (patientAndOtherDetails.PreHospitalizationExpensesOther != null)
                    {
                        var UploadDocuments = await UploadBillFiles(patientAndOtherDetails.PreHospitalizationExpensesOther.Files, "PreHospitalizationExpensesOther");

                        UploadTypeDetail uploadTypeDetail = new UploadTypeDetail()
                        {
                            ClaimId = empClaim.Id,
                            ClaimTypeId = ((long)RecordMasterClaimTypes.DirectClaim),
                            UploadTypeId = ((long)RecordMasterUplodDocType.PreHospitalizationExpensesOther),
                            Amount = patientAndOtherDetails.PreHospitalizationExpensesOther.Amount,
                            CreatedBy = patientAndOtherDetails.EmpId,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                        };

                        _dbContext.Add(uploadTypeDetail);
                        await _dbContext.SaveChangesAsync();

                        UploadDocuments = UploadDocuments.Where(e => e.AdvanceUploadTypeId == null || e.AdvanceUploadTypeId == 0).Select(s => { s.AdvanceUploadTypeId = uploadTypeDetail.Id; s.CreatedBy = patientAndOtherDetails.EmpId; s.CreatedDate = DateTime.Now; s.IsActive = true; s.Comment = ""; return s; }).ToList();

                        await _dbContext.AddRangeAsync(UploadDocuments);
                        await _dbContext.SaveChangesAsync();
                    }

                }

                var responseData = new { emplId = employeeAdvanceData.EmplId, patientId = employeeAdvanceData.PatientId, empclaimId = employeeAdvanceData.ClaimId, claimId = employeeAdvanceData.Id, requestSubmittedById = employeeAdvanceData.RequestSubmittedById };


                responeModel.Data = responseData;
                responeModel.StatusCode = System.Net.HttpStatusCode.Created;
                responeModel.Error = false;
                responeModel.Message = CommonMessage.CreateMessage;
            }
            return responeModel;
        }


        #endregion End Claim Create
        public async Task<ResponeModel> GetAdvanceClaimApproved(long? empId)
        {
            ResponeModel responeModel = new ResponeModel();

            var query = from empAdv in _dbContext.EmpAdvances
                        join emp in _dbContext.Employees on empAdv.EmplId equals emp.Id
                        join empFaim in _dbContext.EmpFamilys on empAdv.PatientId equals empFaim.Id
                        join empRel in _dbContext.EmpRelations on empFaim.RelationId equals empRel.Id
                        join empclmStatus in _dbContext.EmpClaimStatus on empAdv.ClaimId equals empclmStatus.ClaimId
                        join claimStatus in _dbContext.ClaimStatusCategory on empclmStatus.StatusId equals claimStatus.Id
                        join empCreatedBy in _dbContext.Employees on empclmStatus.CreatedBy equals empCreatedBy.Id
                        join empUpdatedBy in _dbContext.Employees on empclmStatus.UpdatedBy equals empUpdatedBy.Id into empUpdatedByGroup
                        from empUpdatedBy in empUpdatedByGroup.DefaultIfEmpty()
                        select new
                        {
                            EmployeeObject = emp,
                            EmpclmStatusObject = empclmStatus,
                            EmpAdvanceObject = empAdv,
                            EmployeeName = emp.Name,
                            PatientName = empFaim.Name,
                            Relation = empRel.Name,
                            AdvanceAmount = empAdv.AdvanceAmount,
                            RequestDate = empAdv.AdvanceRequestDate,
                            ApprovedAmount = empAdv.ApprovedAmount,
                            ApprovedDate = empAdv.ApprovalDate,
                            Status = claimStatus.Name,
                            CreatedBy = empCreatedBy.Name,
                            CreatedDate = empclmStatus.CreatedDate,
                            UpdatedBy = empUpdatedBy != null ? empUpdatedBy.Name : null,
                            UpdatedDate = empclmStatus.UpdatedDate
                        };


            var claimAdvanceData = query.ToList().Where(f => f.EmpclmStatusObject.StatusId == ((long)RecordMasterClaimStatusCategory.Approved) && f.EmpclmStatusObject.ClaimTypeId == ((long)RecordMasterClaimTypes.Advance)).OrderByDescending(o => o.CreatedDate).ToList();

            if (empId != null)
            {
                claimAdvanceData = claimAdvanceData.Where(f => f.EmployeeObject.Id == empId).ToList();
            }
            List<object> empAdvanceData = new();
            if (claimAdvanceData != null && claimAdvanceData.Count() != 0)
            {

                foreach (var item in claimAdvanceData)
                {
                    var dataEmpAdvance = new
                    {
                        AdvanceId = item.EmpAdvanceObject.Id,
                        EmpId = item.EmployeeObject.Id,
                        EmployeeName = item.EmployeeName,
                        PatientName = item.PatientName,
                        Relation = item.Relation,
                        AdvanceAmount = item.AdvanceAmount,
                        RequestDate = item.RequestDate.ToString("MM/dd/yyyy"),
                        ApprovedAmount = item.ApprovedAmount,
                        ApprovedDate = item.ApprovedDate?.ToString("MM/dd/yyyy")
                    };
                    empAdvanceData.Add(dataEmpAdvance);
                }
            }

            responeModel.Data = empAdvanceData;
            responeModel.DataLength = empAdvanceData.Count;
            responeModel.StatusCode = System.Net.HttpStatusCode.OK;
            responeModel.Error = false;
            responeModel.Message = empAdvanceData.Count + " Advance claim approved details.";

            return responeModel;
        }
        public async Task<ResponeModel> GetAdvanceClaimRequest(long? empId)
        {
            ResponeModel responeModel = new ResponeModel();

            var query = from empAdv in _dbContext.EmpAdvances
                        join emp in _dbContext.Employees on empAdv.EmplId equals emp.Id
                        join empFaim in _dbContext.EmpFamilys on empAdv.PatientId equals empFaim.Id
                        join empRel in _dbContext.EmpRelations on empFaim.RelationId equals empRel.Id
                        join empclmStatus in _dbContext.EmpClaimStatus on empAdv.ClaimId equals empclmStatus.ClaimId
                        join claimStatus in _dbContext.ClaimStatusCategory on empclmStatus.StatusId equals claimStatus.Id
                        join empCreatedBy in _dbContext.Employees on empclmStatus.CreatedBy equals empCreatedBy.Id
                        join empUpdatedBy in _dbContext.Employees on empclmStatus.UpdatedBy equals empUpdatedBy.Id into empUpdatedByGroup
                        from empUpdatedBy in empUpdatedByGroup.DefaultIfEmpty()
                        select new
                        {
                            EmployeeObject = emp,
                            EmpclmStatusObject = empclmStatus,
                            EmpAdvanceObject = empAdv,
                            EmployeeName = emp.Name,
                            PatientName = empFaim.Name,
                            Relation = empRel.Name,
                            AdvanceAmount = empAdv.AdvanceAmount,
                            RequestDate = empAdv.AdvanceRequestDate,
                            ApprovedAmount = empAdv.ApprovedAmount,
                            ApprovedDate = empAdv.ApprovalDate,
                            Status = claimStatus.Name,
                            CreatedBy = empCreatedBy.Name,
                            CreatedDate = empclmStatus.CreatedDate,
                            UpdatedBy = empUpdatedBy != null ? empUpdatedBy.Name : null,
                            UpdatedDate = empclmStatus.UpdatedDate
                        };

            var claimAdvanceData = query.ToList().Where(f => f.EmpclmStatusObject.StatusId == ((long)RecordMasterClaimStatusCategory.ClaimInitiated) && f.EmpclmStatusObject.ClaimTypeId == ((long)RecordMasterClaimTypes.Advance)).OrderByDescending(o => o.CreatedDate).ToList();

            if (empId != null)
            {
                claimAdvanceData = claimAdvanceData.Where(f => f.EmployeeObject.Id == empId).ToList();
            }

            List<object> empAdvanceData = new();
            if (claimAdvanceData != null && claimAdvanceData.Count() != 0)
            {

                foreach (var item in claimAdvanceData)
                {
                    var dataEmpAdvance = new
                    {
                        AdvanceId = item.EmpAdvanceObject.Id,
                        EmpId = item.EmployeeObject.Id,
                        EmployeeName = item.EmployeeName,
                        PatientName = item.PatientName,
                        Relation = item.Relation,
                        AdvanceAmount = item.AdvanceAmount,
                        RequestDate = item.RequestDate.ToString("MM/dd/yyyy"),
                        ApprovedAmount = item.ApprovedAmount,
                        ApprovedDate = item.ApprovedDate?.ToString("MM/dd/yyyy")
                    };
                    empAdvanceData.Add(dataEmpAdvance);
                }
            }

            responeModel.Data = empAdvanceData;
            responeModel.DataLength = empAdvanceData.Count;
            responeModel.StatusCode = System.Net.HttpStatusCode.OK;
            responeModel.Error = false;
            responeModel.Message = empAdvanceData.Count + " Advance claim found.";

            return responeModel;
        }

        public async Task<ResponeModel> GetDirectClaimApproved(long? empId)
        {
            ResponeModel responeModel = new ResponeModel();

            var query = from empAdv in _dbContext.EmpAdvances
                        join emp in _dbContext.Employees on empAdv.EmplId equals emp.Id
                        join empFaim in _dbContext.EmpFamilys on empAdv.PatientId equals empFaim.Id
                        join empRel in _dbContext.EmpRelations on empFaim.RelationId equals empRel.Id
                        join empclmStatus in _dbContext.EmpClaimStatus on empAdv.ClaimId equals empclmStatus.ClaimId
                        join claimStatus in _dbContext.ClaimStatusCategory on empclmStatus.StatusId equals claimStatus.Id
                        join empCreatedBy in _dbContext.Employees on empclmStatus.CreatedBy equals empCreatedBy.Id
                        join empUpdatedBy in _dbContext.Employees on empclmStatus.UpdatedBy equals empUpdatedBy.Id into empUpdatedByGroup
                        from empUpdatedBy in empUpdatedByGroup.DefaultIfEmpty()
                        select new
                        {
                            EmployeeObject = emp,
                            EmpclmStatusObject = empclmStatus,
                            EmpAdvanceObject = empAdv,
                            EmployeeName = emp.Name,
                            PatientName = empFaim.Name,
                            Relation = empRel.Name,
                            AdvanceAmount = empAdv.AdvanceAmount,
                            RequestDate = empAdv.AdvanceRequestDate,
                            ApprovedAmount = empAdv.ApprovedAmount,
                            ApprovedDate = empAdv.ApprovalDate,
                            Status = claimStatus.Name,
                            CreatedBy = empCreatedBy.Name,
                            CreatedDate = empclmStatus.CreatedDate,
                            UpdatedBy = empUpdatedBy != null ? empUpdatedBy.Name : null,
                            UpdatedDate = empclmStatus.UpdatedDate
                        };


            var claimAdvanceData = query.ToList().Where(f => f.EmpclmStatusObject.StatusId == ((long)RecordMasterClaimStatusCategory.Approved) && f.EmpclmStatusObject.ClaimTypeId == ((long)RecordMasterClaimTypes.DirectClaim)).OrderByDescending(o => o.CreatedDate).ToList();

            if (empId != null)
            {
                claimAdvanceData = claimAdvanceData.Where(f => f.EmployeeObject.Id == empId).ToList();
            }
            List<object> empAdvanceData = new();
            if (claimAdvanceData != null && claimAdvanceData.Count() != 0)
            {

                foreach (var item in claimAdvanceData)
                {
                    var dataEmpAdvance = new
                    {
                        DirectClaimId = item.EmpAdvanceObject.Id,
                        EmpId = item.EmployeeObject.Id,
                        EmployeeName = item.EmployeeName,
                        PatientName = item.PatientName,
                        Relation = item.Relation,
                        AdvanceAmount = item.AdvanceAmount,
                        RequestDate = item.RequestDate.ToString("MM/dd/yyyy"),
                        ApprovedAmount = item.ApprovedAmount,
                        ApprovedDate = item.ApprovedDate?.ToString("MM/dd/yyyy")
                    };
                    empAdvanceData.Add(dataEmpAdvance);
                }
            }

            responeModel.Data = empAdvanceData;
            responeModel.DataLength = empAdvanceData.Count;
            responeModel.StatusCode = System.Net.HttpStatusCode.OK;
            responeModel.Error = false;
            responeModel.Message = empAdvanceData.Count + " Direct claim approved details.";

            return responeModel;
        }

        public async Task<ResponeModel> GetDirectClaimRequest(long? empId)
        {
            ResponeModel responeModel = new ResponeModel();

            var query = from empAdv in _dbContext.EmpAdvances
                        join emp in _dbContext.Employees on empAdv.EmplId equals emp.Id
                        join empFaim in _dbContext.EmpFamilys on empAdv.PatientId equals empFaim.Id
                        join empRel in _dbContext.EmpRelations on empFaim.RelationId equals empRel.Id
                        join empclmStatus in _dbContext.EmpClaimStatus on empAdv.ClaimId equals empclmStatus.ClaimId
                        join claimStatus in _dbContext.ClaimStatusCategory on empclmStatus.StatusId equals claimStatus.Id
                        join empCreatedBy in _dbContext.Employees on empclmStatus.CreatedBy equals empCreatedBy.Id
                        join empUpdatedBy in _dbContext.Employees on empclmStatus.UpdatedBy equals empUpdatedBy.Id into empUpdatedByGroup
                        from empUpdatedBy in empUpdatedByGroup.DefaultIfEmpty()
                        select new
                        {
                            EmployeeObject = emp,
                            EmpclmStatusObject = empclmStatus,
                            EmpAdvanceObject = empAdv,
                            EmployeeName = emp.Name,
                            PatientName = empFaim.Name,
                            Relation = empRel.Name,
                            AdvanceAmount = empAdv.AdvanceAmount,
                            RequestDate = empAdv.AdvanceRequestDate,
                            ApprovedAmount = empAdv.ApprovedAmount,
                            ApprovedDate = empAdv.ApprovalDate,
                            Status = claimStatus.Name,
                            CreatedBy = empCreatedBy.Name,
                            CreatedDate = empclmStatus.CreatedDate,
                            UpdatedBy = empUpdatedBy != null ? empUpdatedBy.Name : null,
                            UpdatedDate = empclmStatus.UpdatedDate
                        };


            var claimAdvanceData = query.ToList().Where(f => f.EmpclmStatusObject.StatusId == ((long)RecordMasterClaimStatusCategory.ClaimInitiated) && f.EmpclmStatusObject.ClaimTypeId == ((long)RecordMasterClaimTypes.DirectClaim)).OrderByDescending(o => o.CreatedDate).ToList();

            if (empId != null)
            {
                claimAdvanceData = claimAdvanceData.Where(f => f.EmployeeObject.Id == empId).ToList();
            }
            List<object> empAdvanceData = new();
            if (claimAdvanceData != null && claimAdvanceData.Count() != 0)
            {

                foreach (var item in claimAdvanceData)
                {
                    var dataEmpAdvance = new
                    {
                        DirectClaimId = item.EmpAdvanceObject.Id,
                        EmpId = item.EmployeeObject.Id,
                        EmployeeName = item.EmployeeName,
                        PatientName = item.PatientName,
                        Relation = item.Relation,
                        AdvanceAmount = item.AdvanceAmount,
                        RequestDate = item.RequestDate.ToString("MM/dd/yyyy"),
                        ApprovedAmount = item.ApprovedAmount,
                        ApprovedDate = item.ApprovedDate?.ToString("MM/dd/yyyy")
                    };
                    empAdvanceData.Add(dataEmpAdvance);
                }
            }

            responeModel.Data = empAdvanceData;
            responeModel.DataLength = empAdvanceData.Count;
            responeModel.StatusCode = System.Net.HttpStatusCode.OK;
            responeModel.Error = false;
            responeModel.Message = empAdvanceData.Count + " Direct claim approved details.";

            return responeModel;
        }


        #region Get advance details for view HR

        public async Task<ResponeModel> GetAdvanceDetails(long advanceId, string url)
        {
            ResponeModel responeModel = new ResponeModel();

            var result = (from EA in _dbContext.EmpAdvances
                          /*join EC in _dbContext.EmployeeClaims on EA.c*/
                          where EA.Id == advanceId && EA.Claim_TypeId ==1
                          select new
                          {
                              PatientName = EA.EmpFamily.Name,
                              DocumentLists = (from utd in _dbContext.UploadTypeDetails
                                               where utd.ClaimId == EA.ClaimId
                                               join ud in _dbContext.UploadDocuments on utd.Id equals ud.AdvanceUploadTypeId
                                               join ut in _dbContext.UplodDocType on utd.UploadTypeId equals ut.Id
                                               select new
                                               {
                                                   UploadTypeName = ut.Name,
                                                   Amount = ud.Amount,
                                                   FileName = ud.FileName,
                                                   PathUrl = url + ud.PathUrl,
                                                   Comment = ud.Comment
                                               }).ToList(),

                              Relation = EA.EmpFamily.EmpRelation.Name,
                              DOB = EA.EmpFamily.DateOfBirth,
                              Gender = EA.EmpFamily.Gender,
                              AdvanceRequestDate = EA.AdvanceRequestDate,
                              ApprovedRequestedAmount = EA.ApprovedAmount,
                              Status = (from ecc in _dbContext.ClaimStatusCategory
                                        join ecs in _dbContext.EmpClaimStatus on ecc.Id equals ecs.StatusId
                                        where ecs.ClaimId == EA.ClaimId
                                        select ecc.Name).FirstOrDefault(),
                              ApprovalDate = EA.ApprovalDate,
                              HospitalName = EA.HospitalName,
                              HospitalRegNo = EA.HospitalRegNo,
                              DoctorName = EA.DoctorName,
                              Diagnosis = EA.Reason,
                              LikelyDateOfAdmission = EA.DateOfAdmission,
                              AdmissionAdvice = EA.PayTo,
                              Other = EA.Reason,
                              ITRIncomeProof = (from efi in _dbContext.EmpFamilyITRs
                                                where efi.FamilyId == EA.PatientId
                                                select new
                                                {
                                                    FileName = efi.FileName,
                                                    FilePath = url + efi.Path
                                                }).FirstOrDefault(),

                              EstimateAmount = EA.EstimatedAmount,
                              AdvancedRequested = EA.AdvanceAmount,
                              ApprovedAmount = EA.ApprovedAmount
                          }).FirstOrDefault();

            responeModel.Data = result;
            responeModel.DataLength = '1';
            responeModel.StatusCode = System.Net.HttpStatusCode.OK;
            responeModel.Error = false;
            responeModel.Message = "Advance details get successfuly";
            return responeModel;
        }

        #endregion


        #region Call direct claim by claim Id

        public async Task<ResponeModel> GetDirectClaimDetails(long directClaimId, string url)
        {
            ResponeModel responeModel = new ResponeModel();


            var result = (from EA in _dbContext.EmpAdvances
                          where EA.Id == directClaimId 
                          select new
                          {
                              PatientName = EA.EmpFamily.Name,
                              DocumentLists = (from utd in _dbContext.UploadTypeDetails
                                               where utd.ClaimId == EA.ClaimId
                                               join ud in _dbContext.UploadDocuments on utd.Id equals ud.AdvanceUploadTypeId
                                               join ut in _dbContext.UplodDocType on utd.UploadTypeId equals ut.Id
                                               select new
                                               {
                                                   UploadTypeName = ut.Name,
                                                   Amount = ud.Amount,
                                                   FileName = ud.FileName,
                                                   PathUrl = url + ud.PathUrl,
                                                   Comment = ud.Comment
                                               }).ToList(),

                              Relation = EA.EmpFamily.EmpRelation.Name,
                              DOB = EA.EmpFamily.DateOfBirth,
                              Gender = EA.EmpFamily.Gender,
                              AdvanceRequestDate = EA.AdvanceRequestDate,
                              ApprovedRequestedAmount = EA.ApprovedAmount,
                              Status = (from ecc in _dbContext.ClaimStatusCategory
                                        join ecs in _dbContext.EmpClaimStatus on ecc.Id equals ecs.StatusId
                                        where ecs.ClaimId == EA.ClaimId
                                        select ecc.Name).FirstOrDefault(),
                              ApprovalDate = EA.ApprovalDate,
                              HospitalName = EA.HospitalName,
                              HospitalRegNo = EA.HospitalRegNo,
                              DoctorName = EA.DoctorName,
                              Diagnosis = EA.Reason,
                              LikelyDateOfAdmission = EA.DateOfAdmission,
                              AdmissionAdvice = EA.PayTo,
                              Other = EA.Reason,
                              ITRIncomeProof = (from efi in _dbContext.EmpFamilyITRs
                                                where efi.FamilyId == EA.PatientId
                                                select new
                                                {
                                                    FileName = efi.FileName,
                                                    FilePath = url + efi.Path
                                                }).FirstOrDefault(),

                              EstimateAmount = EA.EstimatedAmount,
                              AdvancedRequested = EA.AdvanceAmount,
                              ApprovedAmount = EA.ApprovedAmount
                          }).FirstOrDefault();

            responeModel.Data = result;
            responeModel.DataLength = '1';
            responeModel.StatusCode = System.Net.HttpStatusCode.OK;
            responeModel.Error = false;
            responeModel.Message = "Advance details get successfuly";
            return responeModel;
        }


        #endregion

    }
}
