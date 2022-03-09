using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace crudcore.Model
{
    public class Publication
    {
        [Key]
        public int PublicationID { get; set; }
        public string PublicationName { get; set; }
        public virtual IList<Book> Books { get; set; }


    }

    public class PViewModel
    {
        public IEnumerable<Publication> PublicationIDVM { get; set; }
        public Publication PublicationNameVM { get; set; }
    }
    public class Book
    {
        [Key]
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime PublishDate { get; set; }
        public bool availability { get; set; }
        public int PublicationID_FK { get; set; }
        [ForeignKey("PublicationID_FK")]
        public Publication publication { get; set; }

    }
}