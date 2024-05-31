using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;


namespace HDigital.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAi : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetData(string input)
        {
            string apiKey = "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";
            string response = "";
            OpenAIAPI openai = new OpenAIAPI(apiKey);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = input;
            completion.Model = "gpt-3.5-turbo-instruct";
            completion.MaxTokens = 4000;
            var output = await openai.Completions.CreateCompletionAsync(completion);
            if (output != null)
            {
                foreach (var item in output.Completions)
                {
                    response = item.Text;
                }
                return Ok(response);
            }
            else {
                return BadRequest("Not Found");
            }

        }
    }
}
