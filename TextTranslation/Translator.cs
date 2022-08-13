using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;


namespace TextTranslation
{
    public class Translator
    {
        private static readonly HttpClient client = new HttpClient();

        public Translator()
        {
            client.BaseAddress = new System.Uri("https://text-translation-fairseq-1.ai-sandbox.4th-ir.io/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Translates text from source language to specified language
        /// </summary>
        /// <param name="sentence">Text to be translated</param>
        /// <param name="sourceLanguage">Language of text to be translated</param>
        /// <param name="conversionLanguage">Target language for translation</param>
        /// <exception cref="Exception">Thrown when translation fails</exception>
        /// <returns>Returns the translated text </returns>
        public async Task<string> TranslateText(string sentence, string sourceLanguage, string conversionLanguage)
        {
            RequestContent request = new RequestContent(sentence);

            string requestUri = $"api/v1/sentence?source_lang={sourceLanguage}&conversion_lang={conversionLanguage}";

            var requestJson = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUri, requestJson);

            try
            {
                response.EnsureSuccessStatusCode();

                string r = await response.Content.ReadAsStringAsync();
                char[] param = { '[', ']' };
                ResponseContent responseContent = JsonSerializer.Deserialize<ResponseContent>(r.Trim(param));

                return responseContent.conversion_text;
            }
            catch(Exception exception)
            {
                Exception ex = new Exception("Translation Failed\n"+response,exception);
                throw ex;
            }
        

        }

    }
}
