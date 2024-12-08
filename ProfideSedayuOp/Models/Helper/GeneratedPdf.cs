using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;

namespace ProfideSedayuOp.Models.Helper
{
    public class GeneratedPdf
    {
        public string GenerateDocxFromTemplate(string templatePath, string outputPath, Dictionary<string, string> textData, List<List<string>> tableData)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Template file tidak ditemukan.", templatePath);

            var dt = "sayasiap";
            // Buka dokumen template
            return dt;
        }
    }
    public class PdfConverter
    {
        public void ConvertDocxToPdf(string wordPath, string pdfPath)
        {
            // Pastikan path untuk output file valid
            string outputDir = System.IO.Path.GetDirectoryName(pdfPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Siapkan proses konversi
            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c soffice --headless --convert-to pdf --outdir \"{outputDir}\" \"{wordPath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                try
                {
                    // Jalankan proses
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // Debugging (optional)
                    Console.WriteLine($"Output: {output}");
                    Console.WriteLine($"Error: {error}");

                    // Verifikasi apakah file berhasil dibuat
                    if (!File.Exists(pdfPath))
                    {
                        throw new FileNotFoundException($"Konversi gagal: file PDF tidak ditemukan di {pdfPath}");
                    }
                }
                catch (Exception ex)
                {
                    // Tangani kesalahan
                    throw new Exception("Terjadi kesalahan saat konversi file Word ke PDF.", ex);
                }
            }
        }

    }
    public static class MswordUpdate
    {
        public static void ReplacePlaceHolder(string inputPath, string outputPath, string nama, string tanggal, string jenis)
        {
            // Salin file template ke lokasi baru
            System.IO.File.Copy(inputPath, outputPath, true);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(outputPath, true))
            {
                var document = wordDoc.MainDocumentPart.Document;
                var paragraphs = document.Descendants<Paragraph>();

                foreach (var paragraph in paragraphs)
                {
                    foreach (var text in paragraph.Descendants<Text>())
                    {
                        // Ganti placeholder dengan nilai
                        if (text.Text.Contains("{Nama}"))
                        {
                            text.Text = text.Text.Replace("{Nama}", nama);
                        }
                        if (text.Text.Contains("{tanggal}"))
                        {
                            text.Text = text.Text.Replace("{tanggal}", tanggal);
                        }
                        if (text.Text.Contains("{jenis}"))
                        {
                            text.Text = text.Text.Replace("{jenis}", jenis);
                        }
                    }
                }

                document.Save();
            }
        }

    }
}