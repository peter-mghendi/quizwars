using QuizWars.Shared.Models.Response;

namespace QuizWars.Client.Extensions;

public static class ModelExtensions
{
    public static string GetImageUrl(this TopicResponse topic)
    {
        return $"https://fakeimg.pl/345x300/282828/eae0d0/?retina=1&text={topic.Title}";
    }
}