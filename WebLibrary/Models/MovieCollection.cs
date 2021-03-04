using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLibrary.Data;

namespace WebLibrary.Models {
    public class MovieCollection : IInfoFile {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Назва колекції обов'язкова!")]
        [StringLength(200, ErrorMessage = "Недопустима довжина назви!")]
        [RegularExpression(StringRevision.INPUT_REG_EXP, ErrorMessage = "Недопустима назва!")]
        public string Name { get; set; }
        [BsonIgnore]
        public List<Movie> Movies { get; set; }

        public List<string> MoviesId { get; set; }

        public MovieCollection() {
            Movies = new List<Movie>();
            MoviesId = new List<string>();
        }

        public void ProcessData() {
            Name = StringRevision.RemoveUnnecessarySpaces(Name);
        }

        public byte[] GetInfo() {
            StringBuilder info = new StringBuilder();
            info.AppendLine(Name.ToUpper());
            info.AppendLine();
            info.AppendLine();
            info.AppendLine("Фільми:");

            foreach (Movie m in Movies) {
                info.AppendLine();
                info.AppendLine(Encoding.Default.GetString(m.GetInfo()));
            }

            return Encoding.Default.GetBytes(info.ToString());
        }
    }
}
