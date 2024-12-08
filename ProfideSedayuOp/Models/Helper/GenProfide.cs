using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace ProfideSedayuOp.Models.Helper
{
    public class GenProfide
    {
        public async Task ReplacePlaceholdersAsync(InvoiceInput dt, string bookmarkName, [FromBody] List<List<string>> tableData)
        {
            try
            {
                // Pastikan file output tidak terkunci sebelumnya
                if (System.IO.File.Exists(dt.outputPath))
                {
                    System.IO.File.Delete(dt.outputPath); // Hapus file lama jika sudah ada
                }

                // Salin template ke lokasi output
                System.IO.File.Copy(dt.templatePath, dt.outputPath);

                // Membuka dokumen Word
                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(dt.outputPath, true))
                {
                    string cabangPertama = tableData[0][3];
                    DateTime currentDate = DateTime.Now;
                    string formattedDate = currentDate.ToString("MM-yyyy");
                    var body = wordDocument.MainDocumentPart.Document.Body;

                    ReplaceTextPlaceholder(body, "invoice", dt.invoice);
                    ReplaceTextPlaceholder(body, "tglinv", dt.tglinv);
                    ReplaceTextPlaceholder(body, "JatuhTempo", dt.tgltmp);
                    ReplaceTextPlaceholder(body, "pnbp", dt.pnbp);
                    ReplaceTextPlaceholder(body, "jasatot", dt.jasatot);
                    ReplaceTextPlaceholder(body, "periodetot", dt.periodetot);
                    ReplaceTextPlaceholder(body, "TotalNilaiPNBP", dt.jpsatu);
                    ReplaceTextPlaceholder(body, "TotalNilaiJasa", dt.jidua);
                    ReplaceTextPlaceholder(body, "TotalNilaiDPP", dt.jdpp);
                    ReplaceTextPlaceholder(body, "TotalNilaiPPN", dt.jppn);
                    ReplaceTextPlaceholder(body, "TotalNilaiPPH", dt.jpph);
                    ReplaceTextPlaceholder(body, "jtot", dt.jtot);
                    ReplaceTextPlaceholder(body, "tanggalctts", dt.tanggalctts);
                    ReplaceTextPlaceholder(body, "terbilangtext", dt.terbilang);
                    ReplaceTextPlaceholder(body, "CabangCek", cabangPertama);
                    ReplaceTextPlaceholder(body, "NoFakturMark", dt.NoFaktur);
                    ReplaceTextPlaceholder(body, "MarkBulanTerbit", formattedDate);
                    var mainDocumentPart = wordDocument.MainDocumentPart;
                    var footerReferences = mainDocumentPart.Document.Descendants<FooterReference>().ToList();

                    foreach (var footerReference in footerReferences)
                    {
                        var footerPart = (FooterPart)mainDocumentPart.GetPartById(footerReference.Id);

                        // Create a deep copy of the footer
                        var newFooter = (Footer)footerPart.Footer.CloneNode(true);

                        // Replace placeholders
                        ReplaceTextInFooter(newFooter, "NoFakturMark", dt.NoFaktur);
                        ReplaceTextInFooter(newFooter, "MarkBulanTerbit", formattedDate);

                        // Create a new footer part
                        var newFooterPart = mainDocumentPart.AddNewPart<FooterPart>();
                        newFooterPart.Footer = newFooter;

                        // Update footer reference
                        footerReference.Id = mainDocumentPart.GetIdOfPart(newFooterPart);
                    }

                    var paragraphWithPlaceholder = body.Descendants<Paragraph>()
                        .FirstOrDefault(p => p.InnerText.Contains(bookmarkName));

                    if (paragraphWithPlaceholder != null)
                    {
                        // Ganti teks dengan tabel
                        int index = body.Elements<Paragraph>().ToList().IndexOf(paragraphWithPlaceholder);

                        // Hapus paragraf yang berisi placeholder
                        paragraphWithPlaceholder.Remove();

                        // Menambahkan tabel baru setelah paragraf
                        var table = await CreateTable(tableData);
                        body.InsertAt(table, index);
                    }
                    wordDocument.MainDocumentPart.Document.Save();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Terjadi kesalahan saat mengganti placeholder di dokumen Word.", ex);
            }
        }
        private void ReplaceTextPlaceholder(OpenXmlElement element, string placeholder, string newValue)
        {
            foreach (var text in element.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
            {
                if (text.Text.Contains(placeholder))
                {
                    text.Text = text.Text.Replace(placeholder, newValue);
                }
            }
        }
        private void ReplaceTextInFooter(OpenXmlElement element, string placeholder, string newValue)
        {
            var textElements = element.Descendants<Text>().ToList();
            foreach (var text in textElements)
            {
                if (text.Text != null && text.Text.Contains(placeholder))
                {
                    text.Text = text.Text.Replace(placeholder, newValue);
                }
            }
        }
        private async Task<Table> CreateTable(List<List<string>> tableData)
        {
            Table table = new Table();
            TableProperties tableProperties = new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 8 },
                    new BottomBorder { Val = BorderValues.Single, Size = 8 },
                    new LeftBorder { Val = BorderValues.Single, Size = 8 },
                    new RightBorder { Val = BorderValues.Single, Size = 8 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                    )
                );
            table.AppendChild(tableProperties);
            // Menambahkan header tabel
            // Menambahkan header tabel dengan styling
            TableRow headerRow = new TableRow();

            string[] headerTexts = { "No", "Tgl Pengajuan", "Nama Perusahaan", "Cabang", "PNBP", "Jasa" };
            int[] columnWidths = { 900, 2400, 3600, 3000, 2400, 2400 };
            for (int i = 0; i < headerTexts.Length; i++)
            {
                TableCell headerCell = new TableCell(
                    new Paragraph(
                        new ParagraphProperties(
                            new Justification() { Val = JustificationValues.Center }
                        ),
                        new Run(
                            new RunProperties(
                                new Bold(),
                                new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" },
                                new FontSize() { Val = "24" } // Ukuran font 13pt (26 = 13pt)
                            ),
                            new Text(headerTexts[i])
                        )
                    )
                );

                // Menambahkan warna latar belakang biru langit dan lebar kolom
                headerCell.AppendChild(new TableCellProperties(
                    new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = columnWidths[i].ToString() },
                    new Shading() { Val = ShadingPatternValues.Clear, Color = "auto", Fill = "87CEEB" } // Biru langit
                ));

                headerRow.Append(headerCell);
            }

            table.Append(headerRow);


            foreach (var row in tableData)
            {
                TableRow dataRow = new TableRow();
                for (int i = 0; i < row.Count; i++)
                {
                    TableCell dataCell = new TableCell(
                        new Paragraph(
                            new Run(
                                new RunProperties(
                                    new RunFonts() { Ascii = "Calibri", HighAnsi = "Calibri" },
                                    new FontSize() { Val = "20" } // Ukuran font 12pt (24 = 12pt)
                                ),
                                new Text(row[i])
                            )
                        )
                    );

                    // Properti untuk mengatur lebar sel berdasarkan kolom
                    dataCell.AppendChild(new TableCellProperties(
                        new TableCellWidth() { Type = TableWidthUnitValues.Dxa, Width = columnWidths[i].ToString() }
                    ));

                    dataRow.Append(dataCell);
                }
                table.Append(dataRow);
            }

            return table;
        }

        public async Task PopulateTableAtBookmarkAsync(string templatePath, string outputPath, string bookmarkName, List<List<string>> tableData)
        {
            try
            {
                if (System.IO.File.Exists(outputPath))
                {
                    System.IO.File.Delete(outputPath); // Hapus file lama jika sudah ada
                }

                // Salin template ke lokasi output
                System.IO.File.Copy(templatePath, outputPath);

                using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(outputPath, true))
                {
                    var body = wordDocument.MainDocumentPart.Document.Body;

                    // Cari teks yang akan diganti dengan tabel
                    var paragraphWithPlaceholder = body.Descendants<Paragraph>()
                        .FirstOrDefault(p => p.InnerText.Contains(bookmarkName));

                    if (paragraphWithPlaceholder != null)
                    {
                        // Ganti teks dengan tabel
                        int index = body.Elements<Paragraph>().ToList().IndexOf(paragraphWithPlaceholder);

                        // Hapus paragraf yang berisi placeholder
                        paragraphWithPlaceholder.Remove();

                        // Menambahkan tabel baru setelah paragraf
                        var table = await CreateTable(tableData);
                        body.InsertAt(table, index);
                    }

                    wordDocument.MainDocumentPart.Document.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Terjadi kesalahan saat mengisi tabel di lokasi bookmark.", ex);
            }
        }

    }
}