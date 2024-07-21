namespace TVShowChecker.Infrastructure.Services;

internal sealed class ShowResponse
{
    public Show Show { get; set; }
}

internal sealed class Show
{
    public string Name { get; set; }
    public ShowLinks _Links { get; set; }
}

internal sealed class ShowLinks
{
    public EpisodeHref NextEpisode { get; set; }
    public EpisodeHref PreviousEpisode { get; set; }
}

internal sealed class EpisodeHref
{
    public string Href { get; set; }
}

internal sealed class EpisodeResponse
{
    public string Airdate { get; set; }
    public string Season { get; set; }
    public string Number { get; set; }
}
