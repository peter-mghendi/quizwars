using System.Collections.Immutable;

namespace QuizWars.Shared.Models.Response;

public record GameResponse(long Id, Guid Identifier, TopicResponse Topic, UserInfo PlayerOne, UserInfo PlayerTwo)
{
    public ImmutableArray<UserInfo> Players => [PlayerOne, PlayerTwo];
}