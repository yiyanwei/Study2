using MongoDB.Driver;
using BooksApi.Models;
using System.Collections.Generic;

namespace BooksApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        public BookService(IBookstoreDatabaseSettings settings)
        {
            //创建一个mongo客户端访问对象
            var client = new MongoClient(settings.ConnectinString);
            //定义数据库对象
            var db = client.GetDatabase(settings.DatabaseName);
            //获取名为Books的数据集合，相当于普通数据库里的表
            _books = db.GetCollection<Book>(settings.BooksCollectionName);
        }

        public List<Book> GetBooks()
        {
            //_books.Find()
            //FilterDefinition
            return _books.Find<Book>(book => true).ToList();
        }

        public Book GetBook(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}