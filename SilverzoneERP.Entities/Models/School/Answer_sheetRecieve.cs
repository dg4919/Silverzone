using SilverzoneERP.Entities.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Answer_sheetRecieve : AuditableEntity<long>
    {
        public long EventMgtId { get; set; }
        [ForeignKey("EventMgtId")]
        public virtual EventManagement EventMgtInfo { get; set; }

        public int OMR_Sheets { get; set; }
        public bool Attedence_sheet { get; set; }

        public int QP_Class1 { get; set; }
        public int QP_Class2 { get; set; }
        public int Other { get; set; }

        public Payment_ModeType? PaymentType { get; set; }
        public int Amount { get; set; }

        public long Bundle_No  { get; set; }

        public levelType LevelId { get; set; }

        public virtual Scanned_answerSheet Scanned_answerSheetInfo { get; set; }
    }

    public class Scanned_answerSheet : Entity<long>
    {
        [ForeignKey("Id")]
        public virtual Answer_sheetRecieve Answer_sheetRecieveInfo { get; set; }

        public int OMR_Scanned { get; set; }
        public int ScannedNo_From { get; set; }
        public int ScannedNo_To { get; set; }
        public int Rejected_Sheet { get; set; }
        public string Remarks { get; set; }
    }
}
