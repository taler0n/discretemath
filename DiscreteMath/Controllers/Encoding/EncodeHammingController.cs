using DiscreteMath.Models;
using DiscreteMath.Models.EncodingAlgorithms;
using DiscreteMath.Models.EncodingData;
using System;
using System.Linq;
using System.Text.Json;
using System.Web.Mvc;

namespace DiscreteMath.Controllers.Encoding
{
    public class EncodeHammingController : Controller
    {
        ExerciseContext db = new ExerciseContext();

        // GET: CheckUnambiousCodeExistance
        public ActionResult Index()
        {
            var exerciseList = db.Exercises.Where(e => e.Type == (int)ExerciseTypes.EncodeHamming).ToList();
            var rng = new Random(DateTime.Now.Millisecond);
            var exercise = exerciseList[rng.Next(exerciseList.Count)];
            var data = JsonSerializer.Deserialize<EncodeHammingData>(exercise.ParametersJson);
            return View(new EncodeHammingModel(data));
        }

        [HttpPost]
        public ActionResult Answer(EncodeHammingModel model)
        {
            string encoded = HammingEncoder.Encode(model.InputData.Message);
            string checkMessage = "Верно. ";
            string comment = "";
            if (model.EncodedMessage != encoded)
            {
                checkMessage = "Неверно. ";
                comment = String.Format("Правильный код - {0}. ", encoded);
            }
            model.Comment = checkMessage + comment;
            return View(model);
        }

        //старый метод, будет удален
        [HttpPost]
        public ActionResult Encode(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                for (int i = 0; i < message.Length; i++)
                {
                    if (message[i] != '0' && message[i] != '1')
                    {
                        ViewBag.ErrorMessage = "Сообщение может состоять только из 0 и 1!";
                        return View("Index");
                    }
                }
                ViewBag.ErrorMessage = "";
                ViewBag.Original = message;
                ViewBag.Encoded = HammingEncoder.Encode(message);
            }
            else
            {
                ViewBag.ErrorMessage = "Пожалуйста, введите сообщение!";
            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Decode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                for (int i = 0; i < code.Length; i++)
                {
                    if (code[i] != '0' && code[i] != '1')
                    {
                        ViewBag.ErrorMessage = "Сообщение может состоять только из 0 и 1!";
                        return View("Index");
                    }
                }
                ViewBag.ErrorMessage = "";
                ViewBag.Encoded = code;
                ViewBag.Decoded = HammingEncoder.Decode(code);
            }
            else
            {
                ViewBag.ErrorMessage = "Пожалуйста, введите сообщение!";
            }
            return View("Index");
        }
    }
}