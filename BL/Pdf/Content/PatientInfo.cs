using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vet.BL.Pdf.Style;
using Vet.Models;

namespace Vet.BL.Pdf.Content
{
    internal class PatientInfo
    {
        public static readonly Color Shading = new Color(243, 243, 243);

        public void Add(Section section, VetUser user, MedicalRecord record)
        {
            var p = section.AddParagraph();
            p.AddFormattedText("Kórlap", new Font("Arial", "24 pt"));
            p.Format.Alignment = ParagraphAlignment.Center;
            p.AddLineBreak();
            p.AddLineBreak();
            var table = AddPatientInfoTable(section);

            AddLeftInfo(table.Rows[0].Cells[0], user);
            AddRightInfo(table.Rows[0].Cells[1], record.Animal);
        }

        private Table AddPatientInfoTable(Section section)
        {
            var table = section.AddTable();
            table.Shading.Color = Shading;

            table.Rows.LeftIndent = 0;

            table.LeftPadding = Size.TableCellPadding;
            table.TopPadding = Size.TableCellPadding;
            table.RightPadding = Size.TableCellPadding;
            table.BottomPadding = Size.TableCellPadding;

            // Use two columns of equal width
            var columnWidth = Size.GetWidth(section) / 2.0;
            table.AddColumn(columnWidth);
            table.AddColumn(columnWidth);

            // Only one row is needed
            table.AddRow();

            return table;
        }

        private void AddLeftInfo(Cell cell, VetUser user)
        {
            // Add patient name and sex symbol
            var p1 = cell.AddParagraph();
            p1.AddFormattedText("Tulajdonos adatai:", TextFormat.Underline);
            p1.AddLineBreak();
            p1.AddLineBreak();
            p1.AddText(user.RealName);
            p1.AddLineBreak();
            p1.AddText("Email:");
            p1.AddLineBreak();
            p1.AddText(user.Email);
            p1.AddLineBreak();
            p1.AddText("Telefonszám:");
            p1.AddLineBreak();
            p1.AddText(user.PhoneNumber);

            // Add patient ID
            var p2 = cell.AddParagraph();
            p2.AddText("Tkód: ");
            p2.AddText(user.Id);
        }


        private void AddRightInfo(Cell cell, Animal animal)
        {
            var p = cell.AddParagraph();

            // Add birthdate
            p.AddFormattedText("Állat adatai: ", TextFormat.Underline);
            p.AddLineBreak();
            p.AddLineBreak();
            p.AddText($"Állat kód: {animal.Id}");
            p.AddLineBreak();
            p.AddText($"Név: {animal.Name}");
            p.AddLineBreak();
            p.AddText($"Faj: {animal.Species.Name}");
            p.AddLineBreak();
            var spec = animal.SubSpecies == null ? "" : animal.SubSpecies;
            p.AddText($"Fajta: {spec}");
            p.AddLineBreak();
            p.AddText($"Születési dátum: {Format(animal.DateOfBirth)}");
            p.AddLineBreak();
            p.AddText($"Ivar: {animal.Sex}");
            p.AddLineBreak();
            var weight = animal.Weight == 0 ? "nincs mért adat" : $"{animal.Weight} kg";
            p.AddText($"Legutóbbi súly: {weight}");
        }

        private string Format(DateTime birthdate)
        {
            return $"{birthdate.ToString("yyyy. MM. dd.")} (kor: {Age(birthdate)})";
        }


        private int Age(DateTime birthdate)
        {
            var today = DateTime.Today;
            int age = today.Year - birthdate.Year;
            return birthdate.AddYears(age) > today ? age - 1 : age;
        }
    }
}
