using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HealthClaim.Model.Dtos.Claims
{
    public class PatientAndOtherDetailsModel
    {
        //Model as per EmpClaim
        public bool? IsSpecailDisease { get; set; }

        // public DateTime? ClaimRequetsDate { get; set; }
        public double? ClaimAmount { get; set; }
        public DateTime? ClaimApprovedDate { get; set; }
        public double? ClaimApprovedAmount { get; set; }

        public List<IFormFile> AdmissionAdviceUpload { get; set; }
        public List<IFormFile> DischargeSummaryUpload { get; set; }
        public List<IFormFile> InvestigationReportsUpload { get; set; }
        public List<IFormFile> DiagnosisUpload { get; set; }

        //Model as par end EmpClaim

        //Model as per  EmpClaimBill
        //public long EmpClaimId { get; set; }
        public long EmpId { get; set; }
        // public long StatusId { get; set; }
        public double FinalHospitalBill { get; set; }
        public List<IFormFile> FinalHospitalBillUpload { get; set; }

        public MedicenBill MedicenBill { get; set; }
        public MedicenNotFinalBill? MedicenNotFinalBill { get; set; }
        public Consultation Consultation { get; set; }
        public ConsultationNotFinalBill? ConsultationNotFinalBill { get; set; }
        public Investigation Investigation { get; set; }
        public InvestigationNotFinalBill? InvestigationNotFinalBill { get; set; }
        public RoomRent RoomRent { get; set; }
        public RoomRentNotFinalBill? RoomRentNotFinalBill { get; set; }

        public OtherBill OtherBill { get; set; }
        public OtherBillNotFinalBill? OtherBillNotFinalBill { get; set; }

        //Model as par end EmpClaimBill


        //Fields for EmpAdvance
        public long PatientId { get; set; }
        public long RequestSubmittedById { get; set; }
        public long ClaimId { get; set; }
        //public long ClaimTypeId { get; set; }
        public string RequestName { get; set; }
        public double AdvanceAmount { get; set; }
        public string Reason { get; set; }
        public string PayTo { get; set; }
        public DateTime ApprovalDate { get; set; }
        public double ApprovedAmount { get; set; }
        public long? ApprovedById { get; set; }
        [Required]
        public string HospitalName { get; set; }
        [Required]
        public string HospitalRegNo { get; set; }
        [Required]
        public DateTime DateOfAdmission { get; set; }
        [Required]
        public string DoctorName { get; set; }
        [Required]
        public double EstimatedAmount { get; set; }
        //End for EmpAdvance

        /*public long MemberId { get; set; }
        public string HospitalName { get; set; }
        public string HospitalRegdNo { get; set; }
        public string DoctorName { get; set; }
        public string ActualDateOfAdmission { get; set; }
        public List<IFormFile> AdmissionAdviceUpload { get; set; }
        public DateTime DateOfDischarge { get; set; }
        public double FinalHospitalBillAmount { get; set; }
        public List<IFormFile> FinalHospitalBillUpload { get; set; }
        public List<IFormFile> InvestigationReports { get; set; }

        public BillDetailsNewModel BillDetailsNewModels { get; set; }*/
        public bool IsPreHospitalizationExpenses { get; set; }
        public PreHospitalizationExpensesMedicine? PreHospitalizationExpensesMedicine { get; set; }
        public PreHospitalizationExpensesConsultation? PreHospitalizationExpensesConsultation { get; set; }
        public PreHospitalizationExpensesInvestigation? PreHospitalizationExpensesInvestigation { get; set; }
        public PreHospitalizationExpensesOther? PreHospitalizationExpensesOther { get; set; }
    }

    public class MedicenBill
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class MedicenNotFinalBill
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class Consultation
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class ConsultationNotFinalBill
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class Investigation
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class InvestigationNotFinalBill
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class RoomRent
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class RoomRentNotFinalBill
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class OtherBill
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class OtherBillNotFinalBill
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class PreHospitalizationExpensesMedicine
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class PreHospitalizationExpensesConsultation
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class PreHospitalizationExpensesInvestigation
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
    public class PreHospitalizationExpensesOther
    {
        public int Amount { get; set; }
        public List<IFormFile> Files { get; set; }

    }
}
