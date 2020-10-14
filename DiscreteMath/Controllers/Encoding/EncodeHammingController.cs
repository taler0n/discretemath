using DiscreteMath.Models.EncodingAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiscreteMath.Controllers.Encoding
{
    public class EncodeHammingController : Controller
    {
        // GET: EncodeHamming
        public ActionResult Index()
        {
            return View();
        }

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