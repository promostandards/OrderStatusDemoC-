using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromostandardsOrderStatus.net.hitpromo.ppds.orderstatus;
using System.Xml.Serialization;
using System.IO;

namespace PromostandardsOrderStatus
{
    class Demo
    {
        static void Main(string[] args)
        {
            try { 
                Dictionary<string, string> properties = loadPropertiesFile(getPropertiesFilePath());
                OrderStatusService service = new OrderStatusService();
                GetOrderStatusTypesRequest requestType = loadOrderStatusTypesRequest(properties);
                GetOrderStatusTypesResponse responseType = service.getOrderStatusTypes(requestType);
                GetOrderStatusDetailsRequest requestDetails = loadOrderStatusDetailsRequest(properties);
                GetOrderStatusDetailsResponse responseDetails = service.getOrderStatusDetails(requestDetails);
                outputOrderStatusType(responseType, properties["outputDirTypes"]);
                outputOrderStatusDetails(responseDetails, properties["outputDirDetails"]);
                System.Console.WriteLine("Press any key to exit....");
                System.Console.ReadLine();
            }
            catch(Exception exception)
            {
                System.Console.WriteLine("Exception is " + exception.Message);
                System.Console.WriteLine("Press any key to exit....");
                System.Console.ReadLine();
            }
        }

        public static GetOrderStatusDetailsRequest loadOrderStatusDetailsRequest(Dictionary<string, string> properties)
        {
            GetOrderStatusDetailsRequest request = new GetOrderStatusDetailsRequest();
            request.wsVersion = properties["wsVersion"];
            request.id = properties["id"];
            request.password = properties["credentials"];
            request.queryType = Int32.Parse(properties["queryType"]);
            request.referenceNumber = properties["referenceNumber"];
            return request;

        }

        public static GetOrderStatusTypesRequest loadOrderStatusTypesRequest(Dictionary<string, string> properties)
        {
            GetOrderStatusTypesRequest request = new GetOrderStatusTypesRequest();
            request.wsVersion = properties["wsVersion"];
            request.id = properties["id"];
            request.password = properties["credentials"];
            return request;

        }

        public static bool outputOrderStatusType(GetOrderStatusTypesResponse response, string filePath)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(GetOrderStatusTypesResponse));
                serializer.Serialize(stringwriter, response);
                string xmlOutput = stringwriter.ToString();
                File.WriteAllText(filePath, xmlOutput);
                System.Console.WriteLine("Dumped order status type response to " + filePath);
                return true;
            }
            catch(Exception exception)
            {
                System.Console.WriteLine("Exception is " + exception.Message);
                return false;
            }
        }

        public static bool outputOrderStatusDetails(GetOrderStatusDetailsResponse response, string filePath)
        {
            try
            {
                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(GetOrderStatusDetailsResponse));
                serializer.Serialize(stringwriter, response);
                string xmlOutput = stringwriter.ToString();
                File.WriteAllText(filePath, xmlOutput);
                System.Console.WriteLine("Dumped order status details response to " + filePath);
                return true;
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("Exception is " + exception.Message);
                return false;
            }
        }




        public static String getPropertiesFilePath()
        {
            try
            {
                System.Console.WriteLine("Please enter a properties file path to load the query arguments : ");
                String pathInput = System.Console.ReadLine();
                return pathInput;
            }
            catch(Exception exception)
            {
                System.Console.WriteLine("Exception is " + exception.Message);
                return null;
            }
        }

        public static Dictionary<String, String> loadPropertiesFile(String filePath)
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                foreach (string line in System.IO.File.ReadAllLines(filePath))
                {
                    if ((!string.IsNullOrEmpty(line)) &&
                        (!line.StartsWith(";")) &&
                        (!line.StartsWith("#")) &&
                        (!line.StartsWith("'")) &&
                        (line.Contains('=')))
                    {
                        int index = line.IndexOf('=');
                        string key = line.Substring(0, index).Trim();
                        string value = line.Substring(index + 1).Trim();

                        if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                            (value.StartsWith("'") && value.EndsWith("'")))
                        {
                            value = value.Substring(1, value.Length - 2);
                        }
                        dictionary.Add(key, value);
                    }
                }

                return dictionary;
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("Exception is " + exception.Message);
                return null;
            }
        }
    }
}
