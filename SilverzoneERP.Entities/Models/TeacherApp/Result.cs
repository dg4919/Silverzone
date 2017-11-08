using SilverzoneERP.Entities.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilverzoneERP.Entities.Models
{
    public class Result : Entity<long>
    {
        [MaxLength(5)]
        [Column(TypeName = "char")]
        public string SchCode { get; set; }

        [MaxLength(2)]
        [Column(TypeName = "char")]
        public string RollNo { get; set; }

        [MaxLength(3)]
        [Column(TypeName = "char")]
        public string Class { get; set; }

        [MaxLength(1)]
        [Column(TypeName = "char")]
        public string Sections { get; set; }

        public decimal TotMarks { get; set; }

        public int ClassRank { get; set; }
        public int AllIndiaRank { get; set; }
        [MaxLength(12)]
        [Column(TypeName = "varchar")]
        public string NIORollNo { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string StudName { get; set; }

        public decimal RawScore { get; set; }
        public Nullable<byte> SecondLevelEligible { get; set; }
        public Nullable<byte> Medal { get; set; }
        [MaxLength(6)]
        [Column(TypeName = "varchar")]
        public string EventCode { get; set; }
        [MaxLength(7)]
        [Column(TypeName = "varchar")]
        public string EventYear { get; set; }

        public int Level { set; get; }
    }

    public class Result_L1 : Entity<long>
    {
        [MaxLength(5)]
        [Column(TypeName = "char")]
        public string SchCode { get; set; }

        [MaxLength(2)]
        [Column(TypeName = "char")]
        public string RollNo { get; set; }

        [MaxLength(3)]
        [Column(TypeName = "char")]
        public string Class { get; set; }

        [MaxLength(1)]
        [Column(TypeName = "char")]
        public string Sections { get; set; }

        public decimal TotMarks { get; set; }

        public int ClassRank { get; set; }
        public int AllIndiaRank { get; set; }
        [MaxLength(12)]
        [Column(TypeName = "varchar")]
        public string NIORollNo { get; set; }

        [MaxLength(100)]
        [Column(TypeName = "varchar")]
        public string StudName { get; set; }

        public decimal RawScore { get; set; }
        public Nullable<byte> SecondLevelEligible { get; set; }
        public Nullable<byte> Medal { get; set; }

        public long ResultEventId { get; set; }
        [ForeignKey("ResultEventId")]
        public virtual ResultEvent_Detail ResultEvent { get; set; }

        public virtual Result_L2Final Result_L2 { get; set; }
    }

    public class Result_L2Final : Entity<long>
    {
        [ForeignKey("Id")]
        public virtual Result_L1 Result_L1 { get; set; }

        public decimal TotMarks { get; set; }

        public int ClassRank { get; set; }
        public int AllIndiaRank { get; set; }
        public decimal RawScore { get; set; }

        public string StateRank { get; set; }
    }

    public class ResultEvent : AuditableEntity<long>
    {
        public string EventYear { get; set; }
        public string shortCode { get; set; }
        public bool isResult_declared { get; set; }
        public virtual ICollection<ResultEvent_Detail> ResultEvent_Details { get; set; }
    }

    public class ResultEvent_Detail : Entity<long>
    {
        public long ResultEventId { get; set; }
        [ForeignKey("ResultEventId")]
        public virtual ResultEvent ResultEventInfo { get; set; }

        public long eventId { get; set; }
        [ForeignKey("eventId")]
        public virtual Event EventInfo { get; set; }
    }
}