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
    public class CheckCodeOptimalityController : Controller
    {
        ExerciseContext db = new ExerciseContext();

        // GET: CheckUnambiousCodeExistance
        public ActionResult Index()
        {
            var exerciseList = db.Exercises.Where(e => e.Type == (int)ExerciseTypes.CheckCodeOptimality).ToList();
            var rng = new Random(DateTime.Now.Millisecond);
            var exercise = exerciseList[rng.Next(exerciseList.Count)];
            var data = JsonSerializer.Deserialize<CheckCodeOptimalityData>(exercise.ParametersJson);
            return View(new CheckCodeOptimalityModel(data));
        }

        [HttpPost]
        public ActionResult Answer(CheckCodeOptimalityModel model)
        {
            var optimalCode = OptimalCodeBuilder.GetOptimalCode(model.InputData.CodeAlphabet, model.InputData.Code.CodeFrequencies);
            double suggestedOptimality = model.InputData.Code.GetCodeOptimality();
            double optimality = optimalCode.GetCodeOptimality();
            int correctAnswer = 0;
            string comment = String.Format("Введенный код является кодом с минимальной избыточностью. " +
                "Средняя длина элементарного кода - {0:0.0000}.", optimality);
            if (suggestedOptimality - optimality > 0.000001)
            {
                correctAnswer = 1;
                comment = String.Format("Введенный код не является кодом с минимальной избыточностью. " +
                    "Средняя длина элементарного кода - {0:0.0000}. Минимальная средняя длина - {1:0.0000}. Ею обладает следующий код:", 
                    suggestedOptimality, optimality);
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
                                Dictionary<char, double> frequencies = new Dictionary<char, double>();
                                char[] separator = { ' ' };
                                double frequencySum = 0;
                                while (!reader.EndOfStream)
                                {
                                    string[] symbolArgs = reader.ReadLine().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    if (symbolArgs.Length != 3)
                                    {
                                        ViewBag.ErrorMessage = "Пожалуйста, проверьте правильность введенных данных!";
                                        return View("Index");
                                    }
                                    char symbol = Convert.ToChar(symbolArgs[0]);
                                    double frequency = Convert.ToDouble(symbolArgs[1]);
                                    frequencySum += frequency;
                                    string codeword = symbolArgs[2];
                                    for (int i = 0; i < codeword.Length; i++)
                                    {
                                        if (alphabet.IndexOf(codeword[i]) == -1)
                                        {
                                            ViewBag.ErrorMessage = "Кодовые слова должны использовать только символы алфавита!";
                                            return View("Index");
                                        }
                                    }
                                    codewords.Add(symbol, codeword);
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
                                    FrequencyCode suggestedCode = new FrequencyCode(codewords, frequencies);
                                    FrequencyCode optimalCode = OptimalCodeBuilder.GetOptimalCode(alphabet, frequencies);
                                    double suggestedCodeOptimality = suggestedCode.GetCodeOptimality();
                                    double optimality = optimalCode.GetCodeOptimality();

                                    if (suggestedCodeOptimality - optimality > 0.000001)
                                    {
                                        ViewBag.Answer = String.Format("Введенный код не является кодом с минимальной избыточностью. " +
                                            "Средняя длина элементарного кода - {0}. Минимальная средняя длина - {1}. " +
                                            "Ею обладает следующий код:", suggestedCodeOptimality, optimality);
                                        return View("Index", optimalCode);
                                    }
                                    else
                                    {
                                        ViewBag.Answer = String.Format("Введенный код является кодом с минимальной избыточностью. " +
                                            "Средняя длина элементарного кода - {0}.", optimality);
                                        return View("Index");
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