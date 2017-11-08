using SilverzoneERP.Entities.Models;
using System.Linq;

namespace SilverzoneERP.Entities.ViewModel.School
{
    public class Answer_sheetRecieve_ViewModel
    {
        public long Id { get; set; }
        public long EventMgtId { get; set; }

        public int OMR_Sheets { get; set; }
        public bool Attedence_sheet { get; set; }

        public int QP_Class1 { get; set; }
        public int QP_Class2 { get; set; }
        public int Other { get; set; }

        public Payment_ModeType? PaymentType { get; set; }
        public int Amount { get; set; }
        public levelType levelId { get; set; }

        public int budleNo { get; set; }

        public bool isEnter_ansSheet_scanInfo { get; set; }
        public Answer_sheetScanned_ViewModel ansSheet_scanInfo { get; set; }

        public static void parse(
            Answer_sheetRecieve_ViewModel vm,
            Answer_sheetRecieve model)
        {
            model.Amount = vm.Amount;
            model.LevelId = vm.levelId;
            model.Attedence_sheet = vm.Attedence_sheet;
            model.Bundle_No = vm.budleNo;
            model.EventMgtId = vm.EventMgtId;
            model.OMR_Sheets = vm.OMR_Sheets;
            model.Other = vm.Other;
            model.PaymentType = vm.PaymentType;
            model.QP_Class1 = vm.QP_Class1;
            model.QP_Class2 = vm.QP_Class2;
            model.Status = true;
        }
    }

    public class Answer_sheetScanned_ViewModel
    {
        public int OMR_Scanned { get; set; }
        public int ScannedNo_From { get; set; }
        public int ScannedNo_To { get; set; }
        public int Rejected_Sheet { get; set; }
        public string Remarks { get; set; }

        public static void parse(
          Answer_sheetScanned_ViewModel vm,
          Scanned_answerSheet model,
          long ansSheetId)
        {
            model.Id = ansSheetId;
            model.OMR_Scanned = vm.OMR_Scanned;
            model.Rejected_Sheet = vm.Rejected_Sheet;
            model.ScannedNo_From = vm.ScannedNo_From;
            model.ScannedNo_To = vm.ScannedNo_To;
            model.Remarks = vm.Remarks;
        }

    }

    public class schoolSearch_ViewModel
    {
        public long Code { get; set; }
        public CodeType codeType { get; set; }
        public long eventId { get; set; }

        public static dynamic parse(EventManagement model)
        {
            return new
            {
                eventMgtId = model.Id,
                model.RegNo,
                model.School.SchCode,
                model.School.SchName,
                model.CoOrdinator.FirstOrDefault().CoOrdName,
                model.CoOrdinator.FirstOrDefault().CoOrdMobile,
                totalStudent = model.EnrollmentOrder.Sum(x => x.EnrollmentOrderDetail.Sum(y => y.No_Of_Student))
            };
        }

        public static dynamic parse(IQueryable<Answer_sheetRecieve> model)
        {
            return model.ToList().Select(x => new
            {
                x.Id,
                x.Amount,
                x.Attedence_sheet,
                LevelName = "Level - " + x.LevelId.GetHashCode().ToString(),
                x.Bundle_No,
                x.EventMgtInfo.School.SchCode,
                x.EventMgtInfo.RegNo,
                x.EventMgtInfo.Event.EventName,
                x.OMR_Sheets,
                x.Other,
                x.QP_Class1,
                x.QP_Class2,
                ScannedNo_From = x.Scanned_answerSheetInfo != null ? x.Scanned_answerSheetInfo.ScannedNo_From : 0,
                ScannedNo_To = x.Scanned_answerSheetInfo != null ? x.Scanned_answerSheetInfo.ScannedNo_To : 0,
                Rejected_Sheet = x.Scanned_answerSheetInfo != null ? x.Scanned_answerSheetInfo.Rejected_Sheet : 0
            });
        }
    }

}