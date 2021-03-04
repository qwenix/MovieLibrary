using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLibrary.Models {

    public class Genre {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Назва обов'язкова!")]
        [StringLength(100, ErrorMessage = "Назва повинна містити до 100 символів!")]
        [RegularExpression(StringRevision.INPUT_REG_EXP, ErrorMessage = "Недопустима назва!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Короткий опис обов'язковий!")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Опис повинен містити від 10 до 1000 символів!")]
        [RegularExpression(StringRevision.INPUT_REG_EXP, ErrorMessage = "Будь-ласка, введіть корректний опис!")]
        public string Summary { get; set; }

        public override bool Equals(object obj) {
            if (obj != null && obj is Genre g) {
                return g.GetHashCode() == GetHashCode() && !Equals(g.Id, Id);
            }
            return false;
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        public override string ToString() {
            return Name;
        }
    }
}
