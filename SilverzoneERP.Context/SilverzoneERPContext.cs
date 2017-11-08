using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Entities.Models.Common;
using System.Threading;
using System.Data.Entity.Validation;

namespace SilverzoneERP.Context
{
    public class SilverzoneERPContext : DbContext
    {
        public SilverzoneERPContext() :
            base("SilverzoneERPContext")
        {
            Database.SetInitializer<SilverzoneERPContext>(null);
        }

        #region  ******************  Properties  ******************

        #region ************* Common **********************

        public DbSet<Class> Classes { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<ErrorLogs> ErrorLogs { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserLogs> UserLogs { get; set; }

        #endregion

        #region *************************** School ********************
        public DbSet<SchoolCategory> SchoolCategories { get; set; }
        public DbSet<SchoolGroup> SchoolGroups { get; set; }
        public DbSet<SchoolLog> SchoolLogs { get; set; }
        public DbSet<Zone> Zones { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<City> Cities { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<ERPuser> ERPusers { get; set; }

        public DbSet<FormManagement> FormManagements { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolDetail> SchoolDetails { get; set; }
        public DbSet<CoOrdinator> CoOrdinators { get; set; }
        public DbSet<SchoolRemarks> SchoolRemarks { get; set; }
        public DbSet<SchoolShippingAddress> SchoolShippingAddresses { get; set; }
        public DbSet<BlackListedSchool> BlackListedSchools { get; set; }
        public DbSet<EventManagement> EventManagements { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ExaminationDate> ExaminationDates { get; set; }
        public DbSet<EventYear> EventYears { get; set; }        
        public DbSet<CoOrdinatingTeacher> CoOrdinatingTeachers { get; set; }
        public DbSet<InFavourOf> InFavourOfs { get; set; }
        public DbSet<FeePayment> FeePayments { get; set; }
        public DbSet<DrownOnBank> DrownOnBanks { get; set; }
        public DbSet<DepositOnBank> DepositOnBanks { get; set; }
        public DbSet<EnrollmentOrder> EnrollmentOrder { get; set; }
        public DbSet<EnrollmentOrderDetail> EnrollmentOrderDetail { get; set; }
        public DbSet<StudentEntry> StudentEntries { get; set; }
        public DbSet<EventYearClass> EventYearClasses { get; set; }
        public DbSet<StudentAttendance> StudentAttendances { get; set; }
        public DbSet<Remarks> Remarks { set; get; }
        public DbSet<Lot> Lots { set; get; }
        public DbSet<LotDetail> LotDetails { set; get; }
        public DbSet<QPDispatchERP> QPDispatchERP { set; get; }
        public DbSet<SMS_EmailLog> SMS_EmailLog { set; get; }
        public DbSet<DND_EmailId> DND_Email { set; get; }
        public DbSet<DND_MobileNo> DND_MObile { set; get; }
        public DbSet<ChequeTransaction> ChequeTransaction { set; get; }
        public DbSet<Award> Award { set; get; }
        public DbSet<ExcellenceAward> ExcellenceAward { set; get; }

        public DbSet<Answer_sheetRecieve> Answer_sheetRecieve { set; get; }
        public DbSet<Scanned_answerSheet> Scanned_answerSheet { set; get; }
        
        #endregion

        #region *************************** Web ********************

        public DbSet<Book> Books { get; set; }
        public DbSet<BookReview> BookReviews { get; set; }
        public DbSet<BookContent> Contents { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<GenericOTP> GenericOTP { get; set; }
        public DbSet<UserShippingAddress> UserShippingAddress { get; set; }
        public DbSet<classSubject> classSubject { get; set; }
        public DbSet<ForgetPassword> ForgetPassword { get; set; }

        public DbSet<Dispatch_Master> DispatchSourceInfo { get; set; }
        public DbSet<Dispatch> Dispatch { get; set; }

        public DbSet<Coupon> Coupons { get; set; }

        public DbSet<Banner> Banners { get; set; }
        public DbSet<Olympiads> Olympiads { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<QuickLink> QuickLinks { get; set; }
        public DbSet<TeacherDetail> TeacherDetails { get; set; }
        public DbSet<TeacherEvent> TeacherEvents { get; set; }

        public DbSet<BookBundle> BookBundles { get; set; }
        public DbSet<BookBundleDetails> BundlesDetails { get; set; }

        public DbSet<MasterAcademicYear> MasterAcademicYears { get; set; }
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }

        public DbSet<BookDispatch> BookDispatches { get; set; }
        public DbSet<Query> Queries { get; set; }

        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<QuizQuestionOptions> QuizQuestionOptions { get; set; }
        public DbSet<UserQuizPoints> UserQuizPoints { get; set; }
        public DbSet<QPDispatch> QPDispatch { get; set; }

        public DbSet<Enquiry> Enquiry { get; set; }
        public DbSet<FeedBack> FeedBack { get; set; }
        public DbSet<Freelance> Freelance { get; set; }
        public DbSet<MetaTag> MetaTags { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<UpdateSection> UpdateSection { get; set; }

        public DbSet<MedalImage> MedalImages { get; set; }
        public DbSet<OlympiadList> OlympiadLists { get; set; }

        public DbSet<CourierMode> CourierMode { get; set; }
        public DbSet<ItemTitle_Master> ItemTitle_Masters { get; set; }
        public DbSet<Package_Master> Package_Masters { get; set; }
        public DbSet<Packet_BundleInfo> Packet_BundleInfo { get; set; }

        public DbSet<Dispatch_otherItem_Master> Dispatch_otherItem_Masters { get; set; }
        public DbSet<Dispatch_otherItem_address> Dispatch_otherItem_address { get; set; }

        public DbSet<DispatchLogs> DispatchLogs { get; set; }

        public DbSet<OrderStatusReason> OrderStatusReasons { get; set; }
        public DbSet<CounterCustomer> CounterCustomer { get; set; }

        public DbSet<InstantDownloads> InstantDownloads { get; set; }
        public DbSet<InstantDownload_Subjects> InstantDownload_Subjects { get; set; }
        public DbSet<InstantDownload_SubjectsMapping> InstantDownload_SubjectsMappings { get; set; }
        
        public DbSet<PaymentInfo> PaymentInfo { get; set; }
        public DbSet<PreviousYrQP> PreviousYrQP { get; set; }

        public DbSet<SchoolDownloads> SchoolDownloads { get; set; }
        public DbSet<Syllabus_SampleQP> Syllabus_SampleQPs { get; set; }

        public DbSet<GalleryImageCategory> GalleryImageCategory { get; set; }
        public DbSet<GalleryImage> GalleryImage { get; set; }

        public DbSet<Result_L1> Result_L1 { get; set; }

        public DbSet<Result_L2Final> Result_L2Final { get; set; }

        public DbSet<ResultEvent> ResultEvent { get; set; }

        public DbSet<ResultEvent_Detail> ResultEvent_Detail { get; set; }

        public DbSet<RegisterSchool> RegisterSchool { get; set; }

        public DbSet<Subscribe_Updates> Subscribe_Updates { get; set; }

        public DbSet<NewsUpdates> NewsUpdates { get; set; }

        public DbSet<Result_levelClass> Result_levelClass { get; set; }
        public DbSet<SiteConfiguration> SiteConfiguration { get; set; }
        public DbSet<CarrierMaster> CarrierMaster { get; set; }
        public DbSet<CarrierResume> CarrierResume { get; set; }
        public DbSet<ResultRequest> ResultRequest { get; set; }
        public DbSet<Schedule_Olympiads> Schedule_Olympiads { get; set; }

        #endregion

        #region*********************Teacher*******************
        public DbSet<TeacherQuestion> TeacherQuestions { get; set; }
        public DbSet<TeacherQuestionOption> TeacherQuestionOptions { get; set; }
        public DbSet<BookOrder> BookOrders { get; set; }
        public DbSet<DateOpted> DateOpteds { get; set; }
        #endregion

        #region  *****************  Inventory  **********************

        public DbSet<Stock_Master> Stock_Masters { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<StockLogs> StockLogs { get; set; }

        public DbSet<InventorySource> InventorySources { get; set; }
        public DbSet<InventorySourceDetail> InventorySourceDetails { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrder_Master> PurchaseOrder_Masters { get; set; }
        public DbSet<PurchaseOrderLogs> PurchaseOrderLogs { get; set; }

        public DbSet<DealerBookDiscount> DealerBookDiscounts { get; set; }
        public DbSet<DealerSecondaryAddress> DealerSceondaryAddressess { get; set; }


        public DbSet<RcSchools> RcSchools { get; set; }
        public DbSet<RcSchool_Visits> RcSchool_Visits { get; set; }
        public DbSet<RcSchool_VisitsInfo> RcSchool_VisitsInfo { get; set; }
        
        #endregion

        #endregion


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<StudentEntry>()
            //  .HasOptional(x => x.QuestionPaperLot)
            //  .WithRequired(y => y.StudentEntry);

            //modelBuilder.Entity<Book>()
            //    .HasOptional(x => x.BookDetails)
            //    .WithRequired(y => y.Book);

            modelBuilder.Entity<Dispatch_Master>()
                .HasOptional(x => x.DispatchInfo)
                .WithRequired(y => y.Dispatch_Master);

            //modelBuilder.Entity<PurchaseOrder_Master>()
            //    .HasRequired(p => p.SourceDetail)
            //    .WithMany(p => p.Source)
            //    .HasForeignKey(p => new { p.From, p.To })
            //    .WillCascadeOnDelete(false);
            modelBuilder.Entity<Result_L1>()
                 .HasOptional(x => x.Result_L2)
                 .WithRequired(x => x.Result_L1);

            modelBuilder.Entity<Answer_sheetRecieve>()
                 .HasOptional(x => x.Scanned_answerSheetInfo)
                 .WithRequired(x => x.Answer_sheetRecieveInfo);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(x => x.Entity is IAuditableEntity && (x.State == System.Data.Entity.EntityState.Added || x.State == System.Data.Entity.EntityState.Modified));
            foreach (var entry in modifiedEntries)
            {
                IAuditableEntity entity = entry.Entity as IAuditableEntity;
                if (entity != null)
                {
                    string identity =Thread.CurrentPrincipal.Identity.Name;
                    long identityName = string.IsNullOrEmpty(identity) ? 0 : long.Parse(identity);
                    //long identityName = 1;

                    TimeZone zone = TimeZone.CurrentTimeZone;
                    DateTime now = zone.ToUniversalTime(DateTime.Now);

                    //DateTime now = DateTime.UtcNow;

                    if (entry.State == System.Data.Entity.EntityState.Added)
                    {
                        entity.CreatedBy = identityName;
                        entity.CreationDate = now;
                    }
                    else
                    {
                        base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                        base.Entry(entity).Property(x => x.CreationDate).IsModified = false;
                    }
                    entity.UpdatedBy = identityName;
                    entity.UpdationDate = now;
                }
            }
            try{
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var newException = new FormatException(e);  // format the exception messgae
                throw newException;
            }
        }

    }
}
