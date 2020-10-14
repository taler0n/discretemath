using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DiscreteMath.Models.EncodingAlgorithms;

namespace DiscreteMath.Controllers.Encoding
{
    public class CheckUnambiguousCodeExistanceController : Controller
    {
        // GET: CheckUnambiousCodeExistance
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                string format = Path.GetExtension(upload.FileName);
                if (format == ".txt")
                {
                    try
                    {
                        using (var reader = new StreamReader(upload.InputStream))
                        {
                            string alphabetLengthLine = reader.ReadLine();
                            string codewordLengthsLine = reader.ReadLine();
                            if (alphabetLengthLine != null && codewordLengthsLine != null)
                            {
                                char[] separator = { ' ' };
                                int alphabetLength = int.Parse(alphabetLengthLine);
                                if (alphabetLength <= 0)
                                {
                                    ViewBag.ErrorMessage = "Длина алфавита должна быть больше 0!";
                                    return View("Index");
                                }
                                List<int> codewordLengths = new List<int>();
                                string[] lines = codewordLengthsLine.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var lengthLine in lines)
                                {
                                    int length = int.Parse(lengthLine);
                                    if (length > 0)
                                    {
                                        codewordLengths.Add(length);
                                    }
                                    else
                                    {
                                        ViewBag.ErrorMessage = "Длины слов должны быть больше 0!";
                                        return View("Index");
                                    }
                                }
                                if (codewordLengths.Count == 0)
                                {
                                    ViewBag.ErrorMessage = "Пожалуйста, проверьте правильность введенных данных!";
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "";
                                    if (CodeUnambiguityChecker.CheckMcmillanInequality(codewordLengths, alphabetLength))
                                    {
                                        ViewBag.Answer = String.Format("Данный набор чисел может быть набором длин кодовых слов " +
                                            "однозначно декодируемого кода в алфавите длиной {0}, так как он удовлетворяет " +
                                            "неравенству Макмиллана.", alphabetLength);
                                    }
                                    else
                                    {
                                        ViewBag.Answer = String.Format("Данный набор чисел не может быть набором длин кодовых слов " +
                                            "однозначно декодируемого кода в алфавите длиной {0}, так как он не удовлетворяет " +
                                            "неравенству Макмиллана.", alphabetLength);
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Пожалуйста, проверьте правильность введенных данных!";
                            }
                        }
                    }
                    catch (FormatException)
                    {
                        ViewBag.ErrorMessage = "Пожалуйста, проверьте правильность введенных данных!";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Пожалуйста, загрузите файл в формате .txt!";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Пожалуйста, загрузите файл!";
            }
            return View("Index");
        }
    }
}