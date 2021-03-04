using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.IO;

namespace WebLibrary.Models {
    public class Movie : IInfoFile {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required(ErrorMessage = "Назва стрічки обов'язкова!")]
        [StringLength(200, ErrorMessage = "Недопустима довжина назви!")]
        [RegularExpression(StringRevision.INPUT_REG_EXP, ErrorMessage = "Недопустима назва!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Зазначте режисера!")]
        [RegularExpression(StringRevision.INPUT_REG_EXP, ErrorMessage = "Недопустиме ім'я!")]
        public List<string> Directors { get; set; }

        [Required(ErrorMessage = "Рік прем'єри обов'язковий!")]
        [Range(1900, 2025, ErrorMessage = "Рік прем'єри має некоректне значення!")]
        public int? Year { get; set; }

        [Required(ErrorMessage = "Короткий опис обов'язковий!")]
        [StringLength(1500, MinimumLength = 10, ErrorMessage = "Кількість символів повинна бути від 10 до 1500.")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Оцінка стрічки обов'язкова!")]
        [Range(1, 10, ErrorMessage = "Оцінка повинна бути від 1-го до 10 балів!")]
        public int? Rate { get; set; }

        [RegularExpression(StringRevision.INPUT_REG_EXP, ErrorMessage = "Недопустиме ім'я!")]
        public List<string> Actors { get; set; }

        [RegularExpression(StringRevision.INPUT_REG_EXP, ErrorMessage = "Недопустима назва!")]
        public List<string> Studios { get; set; }

        [BsonIgnore]
        [Required(ErrorMessage = "Оберіть жанр!")]
        public List<string> Genres { get; set; }

        public List<string> GenresId { get; set; }

        public string ImageName { get; set; }
        public string VideoName { get; set; }

        public Movie() {
            Actors = new List<string>();
            Studios = new List<string>();
            GenresId = new List<string>();
        }

        public override bool Equals(object obj) {
            if (obj != null && obj is Movie m) {
                return m.GetHashCode() == GetHashCode() && !Equals(m.Id, Id);
            }
            return false;
        }

        public override int GetHashCode() {
            StringBuilder hashString = new StringBuilder(Title);
            hashString.Append(Year);
            foreach (string d in Directors) {
                hashString.Append(d);
            }
            return hashString.ToString().GetHashCode();
        }

        public void ProcessData() {
            if (Title != null)
                Title = StringRevision.RemoveUnnecessarySpaces(Title);

            ProcessList(Directors);
            ProcessList(Genres);
            ProcessList(Actors);
            ProcessList(Studios);
        }

        private void ProcessList(List<string> l) {
            for (int i = 0; i < l.Count; i++) {
                if (l[i] != null) {
                    l[i] = StringRevision.RemoveUnnecessarySpaces(l[i]);
                }
                else {
                    l.RemoveAt(i);
                    i--;
                }
            }
            Enumerable.Distinct(l);
        }

        public byte[] GetInfo() {
            StringBuilder info = new StringBuilder();
            info.AppendLine(Title.ToUpper());
            info.AppendLine();
            info.AppendLine($"\nОцінка: {Rate}");
            info.AppendLine($"Рік: {Year}");
            if (Genres?.Count > 0)
                info.AppendLine($"Жанри: {string.Join(", ", Genres)}");
            if (Directors?.Count > 0)
                info.AppendLine($"Режисери: {string.Join(", ", Directors)}");
            if (Actors?.Count > 0)
                info.AppendLine($"Актори: {string.Join(", ", Actors)}");
            if (Studios?.Count > 0)
                info.AppendLine($"Студія: {string.Join(", ", Studios)}");
            info.AppendLine($"Опис: {Summary}");

            return Encoding.Default.GetBytes(info.ToString());
        }
    }
}
