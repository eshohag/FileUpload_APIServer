using System.Net.Http.Headers;
using System.Text;

namespace FileUpload_APIServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SendFormDataWithFile();
            Console.ReadKey();
        }
        public static void SendFormDataWithFile()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using var httpClient = new HttpClient(clientHandler);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes("mRemit001:abcd1234.")));

            // Add form fields
            using var formData = new MultipartFormDataContent();
            formData.Add(new StringContent("001"), "appCode");
            formData.Add(new StringContent("M001"), "moduleCode");
            formData.Add(new StringContent("a\\b\\c\\"), "relativePath");
            formData.Add(new StringContent("testFileNewName.pdf"), "fileName");

            // Add file
            var fileBytes = System.IO.File.ReadAllBytesAsync("D:\\test.pdf").Result;
            var fileContent = new ByteArrayContent(fileBytes);
            formData.Add(fileContent, "file", "testFileNewName.pdf");

            //var response = httpClient.PostAsync("https://172.26.8.20:443/api/FileOperation/PostFile", formData).Result;
            var response = httpClient.PostAsync("https://localhost:7091/api/FileOperation/PostFile", formData).Result;

            // Check the response status
            if (response.IsSuccessStatusCode)
            {
                // File uploaded successfully
                Console.WriteLine("File uploaded successfully.");
            }
            else
            {
                // File upload failed
                Console.WriteLine($"File upload failed. Status code: {response.StatusCode}");
            }
        }
    }
}