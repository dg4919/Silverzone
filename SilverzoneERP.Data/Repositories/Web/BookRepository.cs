using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using SilverzoneERP.Context;
using System;

namespace SilverzoneERP.Data
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        //***********************  Book Filter  *************************

        public Book GetById(long id)
        {
            return FindBy(x => x.Id == id).SingleOrDefault();
        }

        public IQueryable<long> GetBooksId(long subjectId, long classId, long categoryId)
        {
            var booklist = GetAll();

            if (subjectId != 0)
                booklist = booklist.Where(x => x.ItemTitle_Master.SubjectId == subjectId);

            if (classId != 0)
                booklist = booklist.Where(x => x.ItemTitle_Master.ClassId == classId);

            if (categoryId != 0)
                booklist = booklist.Where(x => x.ItemTitle_Master.CategoryId == categoryId);

            return booklist.Select(x => x.Id);
        }

        public Book GetByTitleId(long titleId)
        {
            return _dbset.SingleOrDefault(x => x.Title_Mid == titleId 
                                            && x.Status == true);
        }

        public Book GetByTitle(string title)
        {
            return FindBy(x => x.Title == title).FirstOrDefault();
        }
        public Book GetByISBN(string isbn)
        {
            return FindBy(x => x.ISBN == isbn).FirstOrDefault();
        }

        public IEnumerable<Book> GetByAuthor(string authorName)
        {
            return _dbset.Where(x => x.Publisher == authorName).AsEnumerable();
        }

        public Book GetByEdition(string edition)
        {
            return FindBy(x => x.Edition == edition).FirstOrDefault();
        }
        public IEnumerable<Book> GetByWeight(decimal weight)
        {
            return _dbset.Where(x => x.Weight == weight).AsEnumerable();
        }
        public IEnumerable<Book> GetByPrice(decimal price)
        {
            return _dbset.Where(x => x.Price == price).AsEnumerable();
        }
        public IEnumerable<Book> GetByPriceRange(decimal startprice, decimal endprice)
        {
            return _dbset.Where(x => x.Price >= startprice && x.Price <= endprice).AsEnumerable();
        }
        public IEnumerable<Book> GetByCategory(long categoryId)
        {
            return _dbset.Where(x => x.ItemTitle_Master.CategoryId == categoryId).AsEnumerable();
        }

        public Book get_books_byId(long bookId)
        {
            // find use to pass only PK value
            var book = _dbset.Find(bookId);

            return book;
        }

        // ***************  Used for filteration Records  **********************

        //public IQueryable<Book> GetAllBooks()
        //{
        //    return _dbset.Include(x => x.Category);
        //}

        // bcoz where return IQueryable type > then after we can convert it into IEnumerable
        // books is the filtered records 
        public IEnumerable<Book> FilterByClassAndSubject(long classId, long subjectid, IQueryable<Book> books)
        {
            return books.Where(x => x.ItemTitle_Master.ClassId == classId && x.Id == subjectid);
        }

        public IEnumerable<Book> FilterByClassSubjectAndCategory(long classId, long subjectid, IEnumerable<long> categoriesId, IQueryable<Book> books)
        {
            return books.Where(x => x.ItemTitle_Master.ClassId == classId && x.Id == subjectid && categoriesId.Contains(x.ItemTitle_Master.CategoryId));
        }

        public IQueryable<Book> FilterByClassId(long? classId, IQueryable<Book> books)
        {
            return books.Where(x => x.ItemTitle_Master.ClassId == classId.Value);
        }

        public IQueryable<Book> FilterBySubjectId(long? subjectId, IQueryable<Book> books)
        {
            return books.Where(x => x.ItemTitle_Master.SubjectId == subjectId.Value);
        }

        public IQueryable<Book> FilterBycategoriesId(IEnumerable<long> categoriesId, IQueryable<Book> books)
        {
            return books.Where(x => categoriesId.Contains(x.ItemTitle_Master.CategoryId));
        }

        //public IQueryable<Book> parseBookImage(IQueryable<Book> books)
        //{
        //    return books.Select(x =>
        //    new Book
        //    {
        //        Id = x.Id,
        //        BookImage = string.Format("{0}{1}",
        //                      image_urlResolver.bookImage_main,
        //                      x.BookImage),
        //        Title = x.Title,
        //        Category = x.Category,
        //        Class = x.Class,
        //        Subject = x.Subject,
        //        Price = x.Price,
        //        Publisher = x.Publisher,
        //        in_Stock = x.in_Stock
        //    });
        //}

        public List<string> upload_book_Image_toTemp(string tempPath)
        {
            return ClassUtility.upload_orignal_Images_toTemp(tempPath);
        }

        public bool check_book(long classId, long subjectId, long bookCategoryId)
        {
            return _dbset.Any(x => x.ItemTitle_Master.ClassId == classId
                                && x.ItemTitle_Master.SubjectId == subjectId
                                && x.ItemTitle_Master.CategoryId == bookCategoryId
                              );
        }

        public IQueryable<Book> GetByStatus(bool status)
        {
            return FindBy(x => x.Status == status
                            && x.ItemTitle_Master.Subject.Status == true         // only fetch active records
                            && x.ItemTitle_Master.BookCategory.Status == true);
        }

        public IQueryable<Book> getbookCategory(long subjectId, long classId)
        {
            return FindBy(x => x.ItemTitle_Master.SubjectId == subjectId
                            && x.ItemTitle_Master.ClassId == classId
                            && x.Status == true);
        }

    }

    public class BookCategoryRepository : BaseRepository<BookCategory>, IBookCategoryRepository
    {
        public BookCategoryRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookCategory GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        // ICollection have Count property > but IEnumerable has not
        public ICollection<BookCategory> GetByName(string Name)
        {
            //return FindBy(x => x.Name == Name);            // match exact wrd
            //return FindBy(x => x.Name.Contains(Name));    // use as like by in SQL

            // trim use to remove extra space + match with lower case
            return FindBy(x => x.Name.ToLower().Trim() == Name.ToLower().Trim()).ToList();    // use as like by in SQL
        }

        public IEnumerable<BookCategory> GetByNameAndStatus(string name, bool status)
        {
            // Extension methods of IQueryable<T> like > count, Add method will be show only in repostory > not in API
            return FindBy(x => x.Name.Contains(name) && x.Status == status).AsEnumerable();
        }

        public IEnumerable<BookCategory> GetByStatus(bool status)
        {
            return _dbset.Where(x => x.Status == status).AsEnumerable();
        }

        public bool Iscategory_Exist(string Name, int Id)
        {
            return _dbset.Any(x => x.Name.ToLower().Trim() == Name.ToLower().Trim() && x.Id != Id);
        }

    }

    public class BookContentRepository : BaseRepository<BookContent>, IBookContentRepository
    {
        public BookContentRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookContent GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        public BookContent GetByName(string name)
        {
            return FindBy(x => x.Name == name).FirstOrDefault();
        }

        public IEnumerable<BookContent> GetByStatus(bool status)
        {
            return _dbset.Where(x => x.Status == status).AsEnumerable();
        }

        public void deleteWhere(IEnumerable<BookContent> contents)
        {
            // RemoveRange is attach with specific table and context clas
            _dbContext.Contents.RemoveRange(contents);
        }
    }

    public class BookDetailRepository : BaseRepository<BookDetail>, IBookDetailRepository
    {
        public BookDetailRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookDetail GetById(int id)
        {
            return FindBy(x => x.Id == id).FirstOrDefault();
        }

        public IEnumerable<BookDetail> GetByBookId(int id)
        {
            return _dbset.Where(x => x.Id == id).AsEnumerable();
        }


    }

    public class BookDispatchRepository : BaseRepository<BookDispatch>, IBookDispatchRepository
    {
        public BookDispatchRepository(SilverzoneERPContext dbcontext) : base(dbcontext) { }

        public BookDispatch GetByConsignmentNumber(string cnumber)
        {
            return FindBy(x => x.ConsignmentNumber == cnumber).FirstOrDefault();
        }

        public IEnumerable<BookDispatch> GetByCourierName(string cname)
        {
            return _dbset.Where(x => x.CourierName == cname).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetbyDeliveryStatus(string dstatus)
        {
            return _dbset.Where(x => x.DeliveryStatus == dstatus).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetByDispatchDate(DateTime ddate)
        {
            return _dbset.Where(x => x.DispatchDate == ddate).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetByEventCode(string ecode)
        {
            return _dbset.Where(x => x.EventCode == ecode).AsEnumerable();
        }

        public BookDispatch GetById(int id)
        {
            return _dbset.Where(x => x.Id == id).FirstOrDefault();
        }

        public BookDispatch GetByPacketId(string pktid)
        {
            return FindBy(x => x.PacketID == pktid).FirstOrDefault();
        }

        public IEnumerable<BookDispatch> GetBySchoolCode(string scode)
        {
            return _dbset.Where(x => x.SchCode == scode).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetBySchoolCodeAndEventCode(string scode, string ecode)
        {
            return _dbset.Where(x => x.SchCode == scode && x.EventCode == ecode).AsEnumerable();
        }

        public IEnumerable<BookDispatch> GetByWeight(decimal wt)
        {
            return _dbset.Where(x => x.Weight == wt).AsEnumerable();
        }
    }

}
