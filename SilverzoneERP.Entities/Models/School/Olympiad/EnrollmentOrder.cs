﻿using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace SilverzoneERP.Entities.Models
{
    public class EnrollmentOrder : AuditableEntity<long>
    {        
        public long EventManagementId { set; get; }

        [ForeignKey("EventManagementId")]
        public virtual EventManagement EventManagement { set; get; }

        public int OrderNo { set; get; }

        public DateTime OrderDate { set; get; } = DateTime.Now;
    
        #region ExamDate
        public Nullable<long> ExaminationDateId { set; get; }

        [ForeignKey("ExaminationDateId")]
        public virtual ExaminationDate ExaminationDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ChangeExamDate { set; get; }
        #endregion

        [Required]
        public string SrcFrom { set; get; }

        [Required]
        public bool IsConfirm { set; get; }


        public virtual IList<EnrollmentOrderDetail> EnrollmentOrderDetail { set; get; }        
    }
}
