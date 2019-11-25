using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RazorPagesMovie.Models
{
    
    public class Movie
    {
        public int ID{get;set;}
        [StringLength(30,MinimumLength=3,ErrorMessage="标题长度需大于3个字符小于30个字符")]
        public string Title{get;set;}
        [DataType(DataType.Date)]
        [Range(typeof(DateTime),"1/1/1966","1/1/2020",ErrorMessage="请输入正确的时间范围")]
        [Display(Name="Release Date")]
        public DateTime ReleaseDate{get;set;}
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$"),Required,StringLength(30)]
        public string Genre{get;set;}
        [Range(1,100),DataType(DataType.Currency)]
        [Column(TypeName="decimal(18,2)")]
        public decimal Price{get;set;}
        [RegularExpression(@"^[A-Z][a-zA-Z0-9""'\s-]*$"),StringLength(5)]
        public string Rating{get;set;}
    }
}