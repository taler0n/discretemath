using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DiscreteMath.Models;

namespace DiscreteMath.Controllers.Encoding
{
    public class GetCorrectingCodeParametersController : Controller
    {
        // GET: GetCorrectingCodeParameters
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
                            Dictionary<char, string> codewords = new Dictionary<char, string>();
                            char[] separator = { ' ' };
                            int blockSize = -1;
                            while (!reader.EndOfStream)
                            {
                                string[] symbolArgs = reader.ReadLine().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                if (symbolArgs.Length != 2)
                                {
                                    ViewBag.ErrorMessage = "Пожалуйста, проверьте правильность введенных данных!";
                                    return View("Index");
                                }
                                char symbol = Convert.ToChar(symbolArgs[0]);
                                string codeword = symbolArgs[1];
                                if (blockSize == -1)
                                {
                                    blockSize = codeword.Length;
                                }
                                else if (codeword.Length != blockSize)
                                {
                                    ViewBag.ErrorMessage = "Пожалуйста, проверьте правильность введенных данных!";
                                    return View("Index");
                                }
                                else
                                {
                                    codewords.Add(symbol, codeword);
                                }
                            }
                            if (codewords.Count == 0)
                            {
                                ViewBag.ErrorMessage = "Пожалуйста, введите в файл данные для построения кода!";
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "";
                                BlockCode code = new BlockCode(codewords, blockSize);
                                int codeDistance = code.GetCodeDistance();
                                int detectableErrors = code.GetDetectableErrors(codeDistance);
                                int fixableErrors = code.GetFixableErrors(codeDistance);
                                ViewBag.Answer = String.Format("Кодовое расстояние - {0}. " +
                                    "Количество обнаруживаемых ошибок - {1}. Количество исправляемых ошибок - {2}.",
                                    codeDistance, detectableErrors, fixableErrors);
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