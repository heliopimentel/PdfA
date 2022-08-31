using iText.Kernel.Pdf;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PdfA
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string prog = "+---------------------+\n|   Verifique PDF/A   |\n+---------------------+\n";
            Console.WriteLine(prog);
            ConsoleColor foregroundColor = Console.ForegroundColor;
            string txt = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            txt += (txt.EndsWith("\\") ? "" : "\\") + "NaoPdfA.txt";

            if (args.Length != 1 || !Directory.Exists(args[0]))
            {
                Console.Write(args.Length == 1 ? "Diretório não encontrado [folder not found]:\n" : "Use: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(args.Length == 1 ? args[0] : "PdfA \"d:\\PDFs\"");
                Console.ForegroundColor = foregroundColor;
            }
            else
                try
                {
                    Console.Write("...");

                    StreamWriter sw = new StreamWriter(txt);
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

                    string[] file = Directory.GetFiles(args[0], "*.pdf", SearchOption.AllDirectories);
                    for (int n = 0; n < file.Length; n++)
                    {
                        int faltam = file.Length - n;
                        if (faltam % 1000 == 0) Console.ForegroundColor = ConsoleColor.Red;
                        if (n == 0 || faltam % 100 == 0) Console.Write(faltam.ToString(" ###,##0 "));
                        if (faltam % 1000 == 0) Console.ForegroundColor = foregroundColor;
                        if (faltam % 10 == 0) Console.Write(".");
                        if (!PdfA(file[n])) sw.WriteLine(file[n]);
                    }

                    sw.Write(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                    sw.Close();

                    Console.Write("\n\n=> ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(txt);
                    Console.ForegroundColor = foregroundColor;
                    Process.Start(txt);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n\n" + ex.Message);
                    Console.ForegroundColor = foregroundColor;
                }

            Console.Write("\n\nPressione ENTER.");
            Console.ReadLine();
        }

        static bool PdfA(string pdf)
        {
            try { return new PdfDocument(new PdfReader(pdf)).GetReader().GetPdfAConformanceLevel() != null; }
            catch { return false; }
        }
    }
}