using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BooksApi.Services;
using BooksApi.Models;

namespace BooksApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        
        private readonly BookService _bookService;
        public BooksController(BookService bookService)
        {
            this._bookService = bookService;
        }

        /// <summary>
        /// 获取所有的书籍信息
        /// </summary>
        /// <remarks>
        /// 例子:
        /// Get api/books/
        /// </remarks>
        /// <returns>书籍信息的List集合</returns>
        [HttpGet]
        public ActionResult<List<Book>> Get() =>
            _bookService.GetBooks();

        /// <summary>
        /// 根据书籍的Id获取信息
        /// </summary>
         /// <remarks>
        /// 例子:
        /// Get api/books/1
        /// </remarks>
        /// <param name="id">书籍Id</param>
        /// <returns>书籍对象实例</returns>
        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id) =>
            _bookService.GetBook(id);

        /// <summary>
        /// 创建一个书籍对象
        /// </summary>
        /// <param name="book">书籍对象数据</param>
        /// <returns>跳转到获取书籍接口</returns>
        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            _bookService.Create(book);
            return CreatedAtRoute("GetBook", new { id = book.Id }, book);
        }

        /// <summary>
        /// 更新书籍数据
        /// </summary>
        /// <param name="id">书籍id</param>
        /// <param name="bookIn">书籍对象</param>
        /// <returns>返回204</returns>
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.GetBook(id);
            if (book == null)
            {
                //返回404
                return NotFound();
            }

            _bookService.Update(id, bookIn);
            //返回204
            return NoContent();
        }

        /// <summary>
        /// 删除书籍根据id
        /// </summary>
        /// <remarks>
        /// eg: delete  api/delete/5
        /// </remarks>
        /// <param name="id">书籍id</param>
        /// <returns>返回204</returns>
        /// <response code="500">服务器错误</response>  
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.GetBook(id);
            if (book == null)
            {
                //返回404
                return NotFound();
            }
            //删除该id的book记录
            _bookService.Remove(id);
            return NoContent();
        }

    }
}