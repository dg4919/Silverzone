using System.Collections.Generic;
using System.Linq;
using SilverzoneERP.Entities.Models;
using System;

namespace SilverzoneERP.Data
{
    public interface IBookRepository : IRepository<Book>
    {
        Book GetById(long id);
        Book GetByTitle(string title);
        Book GetByISBN(string isbn);
        IEnumerable<Book> GetByAuthor(string authorName);
        //IEnumerable<Book> GetByClassAndSubject(string stdclass,long subjectid);
        Book GetByEdition(string edition);
        IEnumerable<Book> GetByWeight(decimal weight);
        IEnumerable<Book> GetByPrice(decimal price);
        Book GetByTitleId(long titleId);
        IEnumerable<Book> GetByPriceRange(decimal startprice, decimal endprice);
        IEnumerable<Book> GetByCategory(long categoryId);
        Book get_books_byId(long bookId);

        IQueryable<Book> FilterByClassId(long? classId, IQueryable<Book> books);
        IQueryable<Book> FilterBySubjectId(long? subjectId, IQueryable<Book> books);
        IQueryable<Book> FilterBycategoriesId(IEnumerable<long> categoriesId, IQueryable<Book> books);

        List<string> upload_book_Image_toTemp(string tempPath);
        bool check_book(long classId, long subjectId, long bookCategoryId);
        IQueryable<Book> GetByStatus(bool status);
        IQueryable<Book> getbookCategory(long subjectId, long classId);
        IQueryable<long> GetBooksId(long subjectId, long classId, long categoryId);

    }

    public interface IBookDispatchRepository : IRepository<BookDispatch>
    {
        BookDispatch GetById(int id);
        BookDispatch GetByPacketId(string pktid);
        IEnumerable<BookDispatch> GetByDispatchDate(DateTime ddate);
        IEnumerable<BookDispatch> GetBySchoolCode(string scode);
        IEnumerable<BookDispatch> GetByEventCode(string ecode);
        IEnumerable<BookDispatch> GetBySchoolCodeAndEventCode(string scode, string ecode);
        BookDispatch GetByConsignmentNumber(string cnumber);
        IEnumerable<BookDispatch> GetByWeight(decimal wt);
        IEnumerable<BookDispatch> GetbyDeliveryStatus(string dstatus);
        IEnumerable<BookDispatch> GetByCourierName(string cname);

    }

    public interface IBookDetailRepository : IRepository<BookDetail>
    {
        BookDetail GetById(int id);
        IEnumerable<BookDetail> GetByBookId(int id);
    }

    public interface IBookContentRepository : IRepository<BookContent>
    {
        BookContent GetById(int id);
        BookContent GetByName(string name);
        IEnumerable<BookContent> GetByStatus(bool status);
    }

    public interface IBookCategoryRepository : IRepository<BookCategory>
    {
        BookCategory GetById(int id);
        IEnumerable<BookCategory> GetByNameAndStatus(string name, bool status);
        IEnumerable<BookCategory> GetByStatus(bool status);

        ICollection<BookCategory> GetByName(string Name);
        bool Iscategory_Exist(string Name, int Id);
    }

}
