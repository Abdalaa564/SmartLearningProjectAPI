
namespace SmartLearning.Application.DTOs.ChatGPT
{
    public class ChatGPTResponse
    {
        public List<ChatGPTChoice>? choices { get; set; }
    }
}

// الـ API بيرجع JSON بالشكل ده
//{
//    "ChatGPTChoice": [
//      {
//        "ChatGPTMessage": {
//            "role": "assistant",
//        "content": "Hello! How can I help you?"
//        }
//    }
//  ]
//}