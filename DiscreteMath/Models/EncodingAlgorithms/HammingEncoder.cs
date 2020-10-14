using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DiscreteMath.Models.EncodingAlgorithms
{
    public static class HammingEncoder
    {
        public static string Encode(string message)
        {
            StringBuilder encodedMessage = new StringBuilder();
            int power = 1;
            int position = 0;
            while (position + power - 1 < message.Length)
            {
                encodedMessage.Append('0');
                encodedMessage.Append(message.Substring(position, power - 1));
                position += power - 1;
                power *= 2;
            }
            encodedMessage.Append('0');
            encodedMessage.Append(message.Substring(position));

            power = 1;

            while (power < encodedMessage.Length)
            {
                int sum = 0;
                bool addSymbol = true;
                position = power - 1;
                bool cycle = true;
                while (cycle)
                {
                    int intervalCount = 0;
                    while (intervalCount < power)
                    {
                        if (addSymbol)
                        {
                            sum += int.Parse(encodedMessage[position].ToString());
                        }
                        position++;
                        intervalCount++;
                        if (position == encodedMessage.Length)
                        {
                            cycle = false;
                            break;
                        }
                    }
                    addSymbol = !addSymbol;
                }
                encodedMessage[power - 1] = (sum % 2).ToString()[0];
                power *= 2;
            }

            return encodedMessage.ToString();
        }

        public static string Decode(string message)
        {
            int wrongBitsSum = 0;
            int power = 1;
            while (power < message.Length)
            {
                int sum = 0;
                bool addSymbol = true;
                int position = power - 1;
                bool cycle = true;
                while (cycle)
                {
                    int intervalCount = 0;
                    while (intervalCount < power)
                    {
                        if (addSymbol)
                        {
                            sum += int.Parse(message[position].ToString());
                        }
                        position++;
                        intervalCount++;
                        if (position == message.Length)
                        {
                            cycle = false;
                            break;
                        }
                    }
                    addSymbol = !addSymbol;
                }
                if (sum % 2 != 0)
                {
                    wrongBitsSum += power;
                }
                power *= 2;
            }
            if (wrongBitsSum > 0)
            {
                int correctSymbol = (int.Parse(message[wrongBitsSum - 1].ToString()) + 1) % 2;
                message = message.Substring(0, wrongBitsSum - 1) + correctSymbol.ToString() + message.Substring(wrongBitsSum);
            }

            power = 1;
            StringBuilder messageBuilder = new StringBuilder();
            for (int i = 0; i < message.Length; i++)
            {
                if (i == power - 1)
                {
                    power *= 2;
                }
                else
                {
                    messageBuilder.Append(message[i]);
                }
            }

            return messageBuilder.ToString();
        }
    }
}