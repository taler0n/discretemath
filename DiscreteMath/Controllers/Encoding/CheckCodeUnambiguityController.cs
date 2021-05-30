using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DiscreteMath.Models;
using DiscreteMath.Models.EncodingAlgorithms;
using System.Linq;
using DiscreteMath.Models.EncodingData;
using System.Text.Json;

namespace DiscreteMath.Controllers.Encoding
{
    public class CheckCodeUnambiguityController : Controller
    {
        ExerciseContext db = new ExerciseContext();

        // GET: CheckCodeUnambiguity
        public ActionResult Index()
        {
            var exerciseList = db.Exercises.Where(e => e.Type == (int) ExerciseTypes.CheckCodeUnambiguity).ToList();
            var rng = new Random(DateTime.Now.Millisecond);
            var exercise = exerciseList[rng.Next(exerciseList.Count)];
            var data = JsonSerializer.Deserialize<CheckCodeUnambiguityData>(exercise.ParametersJson);
            return View(new CheckCodeUnambiguityModel(data));
        }

        [HttpPost]
        public ActionResult Answer(CheckCodeUnambiguityModel model)
        {
            List<int> codewordLengths = new List<int>();
            foreach (var codeword in model.InputData.Code.ElementaryCodes.Values)
            {
                codewordLengths.Add(codeword.Length);
            }
            int correctAnswer = 0;
            string comment = "";
            if (CodeUnambiguityChecker.CheckMcmillanInequality(codewordLengths, model.InputData.CodeAlphabet.Length))
            {
                if (CodeUnambiguityChecker.CheckPrefixProperty(model.InputData.Code))
                {
                    comment = "Код обладает свойством взаимной однозначности, " +
                        "так как он обладает свойством префикса.";
                }
                else if (CodeUnambiguityChecker.CheckMarkovTheorem(model.InputData.Code))
                {
                    comment = "Код обладает свойством взаимной однозначности, " +
                        "так как он соответствует критерию Маркова.";
                }
                else
                {
                    correctAnswer = 1;
                    comment = "Код не обладает свойством взаимной однозначности, " +
                        "так как он не соответствует критерию Маркова.";
                }
            }
            else
            {
                correctAnswer = 1;
                comment = "Код не обладает свойством взаимной однозначности, " +
                    "так как для него не выполняется неравенство Макмиллана.";
            }
            string checkMessage = "Верно. ";
            if (model.SelectedAnswer != correctAnswer)
            {
                checkMessage = "Неверно. ";
            }
            model.Comment = checkMessage + comment;
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
                            string alphabet = reader.ReadLine();
                            if (alphabet != null)
                            {
                                for (int i = 0; i < alphabet.Length - 1; i++)
                                {
                                    for (int j = i + 1; j < alphabet.Length; j++)
                                    {
                                        if (alphabet[j] == alphabet[i])
                                        {
                                            ViewBag.ErrorMessage = "Все символы алфавита должны быть различны!";
                                            return View("Index");
                                        }
                                    }
                                }
                                Dictionary<char, string> codewords = new Dictionary<char, string>();
                                char[] separator = { ' ' };
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
                                    for (int i = 0; i < codeword.Length; i++)
                                    {
                                        if (alphabet.IndexOf(codeword[i]) == -1)
                                        {
                                            ViewBag.ErrorMessage = "Кодовые слова должны использовать только символы алфавита!";
                                            return View("Index");
                                        }
                                    }
                                    codewords.Add(symbol, codeword);
                                }
                                if (codewords.Count == 0)
                                {
                                    ViewBag.ErrorMessage = "Пожалуйста, введите в файл данные для построения кода!";
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "";
                                    Code code = new Code(codewords);
                                    List<int> codewordLengths = new List<int>();
                                    foreach (var codeword in codewords.Values)
                                    {
                                        codewordLengths.Add(codeword.Length);
                                    }
                                    if (CodeUnambiguityChecker.CheckMcmillanInequality(codewordLengths, alphabet.Length))
                                    {
                                        if (CodeUnambiguityChecker.CheckPrefixProperty(code))
                                        {
                                            ViewBag.Answer = "Код обладает свойством взаимной однозначности, " +
                                                "так как он обладает свойством префикса.";
                                        }
                                        else if (CodeUnambiguityChecker.CheckMarkovTheorem(code))
                                        {
                                            ViewBag.Answer = "Код обладает свойством взаимной однозначности, " +
                                                "так как он соответствует критерию Маркова.";
                                        }
                                        else
                                        {
                                            ViewBag.Answer = "Код не обладает свойством взаимной однозначности, " +
                                                "так как он не соответствует критерию Маркова.";
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.Answer = "Код не обладает свойством взаимной однозначности, " +
                                            "так как для него не выполняется неравенство Макмиллана.";
                                    }
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Пожалуйста, введите в файл данные для построения кода!";
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