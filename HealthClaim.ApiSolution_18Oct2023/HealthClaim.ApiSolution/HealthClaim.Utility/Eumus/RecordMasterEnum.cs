
using System.ComponentModel.DataAnnotations;

namespace HealthClaim.Utility.Eumus
{
    public enum RecordMasterClaimTypes
    {
        Advance = 1,
        AdvanceClaim = 2,
        DirectClaim = 3
    }
    public enum RecordMasterIds
    {
        HRID = 17,
        FinianceId = 15,
    }

    public enum RecordMasterUplodDocType
    {
        Medicine = 1,
        MedicinenotinFinalBill = 2,
        Consultation = 3,
        RoomRent = 4,
        Investigation = 5,
        Other = 6,
        ConsultationNotFinalBill = 7,
        InvestigationNotFinalBill = 8,
        OtherBillNotFinalBill = 9,
        AdmissionAdviceUpload = 10,
        DischargeSummary=11,
        InvestigationReports = 12,
        FinalHospitalBill = 13,
        Diagnosis = 14,
        PreHospitalizationExpensesMedicine = 15,
        PreHospitalizationExpensesConsultation = 16,
        PreHospitalizationExpensesInvestigation = 17,
        PreHospitalizationExpensesOther = 18,
    }
    public enum RecordMasterEmpRelations
    {
        Father = 1,
        Mother = 2,
        Son = 3,
        Daughter = 4,
        Husband = 5,
        Spouse = 6,
    }
    public enum RecordMasterClaimStatusCategory
    {
        [Display(Name = "ClaimInitiated")]
        ClaimInitiated = 1,
        Approved = 2,
        HRProcessing = 3,
        FinanceProcessing = 4,
        Rejected = 5,

    }
}
