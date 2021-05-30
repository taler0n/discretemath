using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DiscreteMath.Models;
using DiscreteMath.Models.EncodingAlgorithms;
using System.Linq;
using System.Text.Json;
using DiscreteMath.Models.EncodingData;

namespace DiscreteMath.Controllers.Encoding
{
    public class BuildOptimalCodeController : Controller
    {
        ExerciseContext db = new ExerciseContext();

        // GET: CheckUnambiousCodeExistance
        public ActionResult Index()
        {
            var exerciseList = db.Exercises.Where(e => e.Type == (int)ExerciseTypes.BuildOptimalCode).ToList();
            var rng = new Random(DateTime.Now.Millisecond);
            var exercise = exerciseList[rng.Next(exerciseList.Count)];
            var data = JsonSerializer.Deserialize<BuildOptimalCodeData>(exercise.ParametersJson);
            return View(new BuildOptimalCodeModel(data));
        }

        [HttpPost]
        public ActionResult Answer(BuildOptimalCodeModel model)
        {
            var suggestedCode = new FrequencyCode(model.ElementaryCodes, model.InputData.FrequencyList);
            var optimalCode = OptimalCodeBuilder.GetOptimalCode(model.InputData.CodeAlphabet, model.InputData.FrequencyList);
            double suggestedOptimality = suggestedCode.GetCodeOptimality();
            double optimality = optimalCode.GetCodeOptimality();
            string checkMessage = String.Format("Верно. Введенный код является кодом с минимальной избыточностью. " +
                "Средняя длина элементарного кода - {0:0.0000}.", optimality);
            if (suggestedOptimality - optimality > 0.000001)
            {
                checkMessage = String.Format("Неверно. Введенный код не является кодом с минимальной избыточностью. " +
                    "Средняя длина элементарного кода - {0:0.0000}. Минимальная средняя длина - {1:0.0000}.",
                    suggestedOptimality, optimality);
            }
            model.Comment = checkMessage;
            return View(model);
        }

        //старый метод, будет удален
        [HttpPost]
        public string Build(List<string> symbols, List<string> frequencies)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var symbol in symbols)
            {
                sb.Append(symbol);
            }
            foreach (var f in frequencies)
            {
                sb.Append(f);
            }
            return sb.ToString();
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
                                Dictionary<char, double> frequencies = new Dictionary<char, double>();
                                char[] separator = { ' ' };
                                double frequencySum = 0;
                                while (!reader.EndOfStream)
                                {
                                    string[] symbolArgs = reader.ReadLine().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    if (symbolArgs.Length != 2)
                                    {
                                        ViewBag.ErrorMessage = "Пожалуйста, проверьте правильность введенных данных!";
                                        return View("Index");
                                    }
                                    char symbol = Convert.ToChar(symbolArgs[0]);
                                    double frequency = Convert.ToDouble(symbolArgs[1]);
                                    frequencySum += frequency;
                                    frequencies.Add(symbol, frequency);
                                }
                                if (frequencies.Count == 0)
                                {
                                    ViewBag.ErrorMessage = "Пожалуйста, введите в файл данные для построения кода!";
                                }
                                else if (Math.Abs(frequencySum - 1) > 0.000001)
                                {
                                    ViewBag.ErrorMessage = "Сумма вероятностей должна быть равна 1!";
                                }
                                else
                                {
                                    ViewBag.ErrorMessage = "";
                                    FrequencyCode code = OptimalCodeBuilder.GetOptimalCode(alphabet, frequencies);
                                    return View("Index", code);
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