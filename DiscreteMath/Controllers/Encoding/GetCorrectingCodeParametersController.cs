using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DiscreteMath.Models;
using DiscreteMath.Models.EncodingData;
using System.Linq;
using System.Text.Json;
using System.Text;

namespace DiscreteMath.Controllers.Encoding
{
    public class GetCorrectingCodeParametersController : Controller
    {
        ExerciseContext db = new ExerciseContext();

        // GET: CheckUnambiousCodeExistance
        public ActionResult Index()
        {
            var exerciseList = db.Exercises.Where(e => e.Type == (int)ExerciseTypes.GetCorrectingCodeParameters).ToList();
            var rng = new Random(DateTime.Now.Millisecond);
            var exercise = exerciseList[rng.Next(exerciseList.Count)];
            var data = JsonSerializer.Deserialize<GetCorrectingCodeParametersData>(exercise.ParametersJson);
            return View(new GetCorrectingCodeParametersModel(data));
        }

        [HttpPost]
        public ActionResult Answer(GetCorrectingCodeParametersModel model)
        {
            var blockCode = new BlockCode(model.InputData.Code.ElementaryCodes, model.InputData.Code.ElementaryCodes.Values.ElementAt(0).Length);
            bool foundMistake = false;
            string checkMessage = "Верно. ";
            var commentBuilder = new StringBuilder();
            int codeDistance = blockCode.GetCodeDistance();
            if (model.CodeDistance != codeDistance)
            {
                foundMistake = true;
                commentBuilder.Append(String.Format("Кодовое расстояние - {0}. ", codeDistance));
            }
            int detectableErrors = blockCode.GetDetectableErrors(codeDistance);
            if (model.DetectableErrors != detectableErrors)
            {
                foundMistake = true;
                commentBuilder.Append(String.Format("Количество обнаруживаемых ошибок - {0}. ", detectableErrors));
            }
            int fixableErrors = blockCode.GetFixableErrors(codeDistance);
            if (model.FixableErrors != fixableErrors)
            {
                foundMistake = true;
                commentBuilder.Append(String.Format("Количество исправляемых ошибок - {0}. ", fixableErrors));
            }
            if (foundMistake)
            {
                checkMessage = "Неверно. ";
            }
            model.Comment = checkMessage + commentBuilder.ToString();
            return View(model);
        }

        //старый метод, будет удален
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
                                    codewords.Add(symbol, codeword);
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