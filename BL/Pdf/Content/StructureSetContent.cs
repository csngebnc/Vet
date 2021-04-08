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
    internal class StructureSetContent
    {
        public void Add(Section section, MedicalRecord record)
        {
            AddHeading(section, record);
            AddAnamnesys(section, record);
            AddSymptoma(section, record);
            AddStructures(section, record);
            AddDetails(section, record);
        }

        private void AddHeading(Section section, MedicalRecord record)
        {
            var p1 = section.AddParagraph();
            p1.AddLineBreak();
            p1.AddFormattedText("Kórlap:", TextFormat.Bold | TextFormat.Underline);
            p1.AddLineBreak();
            p1.AddLineBreak();

            var p2 = section.AddParagraph();
            p2.AddFormattedText($"Dátum: {DateTime.Now.ToString("yyyy. MM. dd. - HH:mm")} \n" +
                                 $"Klinika, orvos: BunnyV, {record.Doctor.RealName  }", TextFormat.Bold);

            p2.AddLineBreak();
            p2.AddLineBreak();
        }

        private void AddAnamnesys(Section section, MedicalRecord record)
        {
            var anamnesis = section.AddParagraph();
            anamnesis.AddFormattedText("[ Anamnesis ]", TextFormat.Bold);
            anamnesis.AddLineBreak();
            anamnesis.AddText($"{record.Anamnesis}");
            anamnesis.AddLineBreak();
            anamnesis.AddLineBreak();

        }

        private void AddSymptoma(Section section, MedicalRecord record)
        {
            var symptoma = section.AddParagraph();
            symptoma.AddFormattedText("[ Symptoma ]", TextFormat.Bold);
            symptoma.AddLineBreak();
            symptoma.AddText($"{record.Symptoma}");
            symptoma.AddLineBreak();
            symptoma.AddLineBreak();
        }

        private void AddDetails(Section section, MedicalRecord record)
        {
            var details = section.AddParagraph();
            details.AddLineBreak();
            details.AddLineBreak();
            details.AddFormattedText("[ Megjegyzés, kezelési javaslat ]", TextFormat.Bold);
            details.AddLineBreak();
            details.AddText($"{record.Details}");
            details.AddLineBreak();
            details.AddLineBreak();
        }

        private void AddStructures(Section section, MedicalRecord record)
        {
            var therapia = section.AddParagraph();
            therapia.AddFormattedText("[ Therapia ]", TextFormat.Bold);
            therapia.Format.KeepWithNext = true;
            AddStructureTable(section, record.TherapiaRecords);
        }


        private void AddStructureTable(Section section, ICollection<TherapiaRecord> records)
        {
            var table = section.AddTable();

            FormatTable(table);
            AddColumnsAndHeaders(table);
            AddStructureRows(table, records);

            //AddLastRowBorder(table);
        }

        private static void FormatTable(Table table)
        {
            table.LeftPadding = 0;
            table.TopPadding = Size.TableCellPadding;
            table.RightPadding = 0;
            table.BottomPadding = Size.TableCellPadding;
            table.Format.LeftIndent = Size.TableCellPadding;
            table.Format.RightIndent = Size.TableCellPadding;
        }

        private void AddColumnsAndHeaders(Table table)
        {
            var width = Size.GetWidth(table.Section);
            table.AddColumn(width * 0.1);
            table.AddColumn(width * 0.3);
            table.AddColumn(width * 0.2);
        }

        private void AddStructureRows(Table table, ICollection<TherapiaRecord> records)
        {
            foreach (var record in records)
            {
                var row = table.AddRow();
                row.VerticalAlignment = VerticalAlignment.Center;

                row.Cells[0].AddParagraph("#"+record.Therapia.Id.ToString());
                row.Cells[1].AddParagraph(record.Therapia.Name);
                row.Cells[2].AddParagraph($"{record.Amount} {record.Therapia.UnitName}");
            }
        }

        private void AddLastRowBorder(Table table)
        {
            var lastRow = table.Rows[table.Rows.Count - 1];
            lastRow.Borders.Bottom.Width = 2;
        }
    }
}
