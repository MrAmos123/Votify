using Data.Models.Client;
using SharedLibraryCore;
using SharedLibraryCore.Configuration;
using SharedLibraryCore.Interfaces;

namespace Votify.Commands;

public class NoCommand : Command
{
    public NoCommand(CommandConfiguration config, ITranslationLookup translationLookup) : base(config,
        translationLookup)
    {
        Name = "no";
        Description = "vote no on the current vote";
        Alias = "n";
        Permission = EFClient.Permission.User;
        RequiresTarget = false;
    }

    public override async Task ExecuteAsync(GameEvent gameEvent)
    {
        if (Plugin.Votify.InProgressVote(gameEvent.Owner))
        {
            var result = Plugin.Votify.CastVote(gameEvent.Owner, gameEvent.Origin, Vote.No);
            switch (result)
            {
                case VoteResult.Success:
                    gameEvent.Origin.Tell(Plugin.Configuration.Translations.VoteSuccess
                        .FormatExt(Plugin.Configuration.Translations.VoteNo));
                    break;
                case VoteResult.NoVoteInProgress:
                    gameEvent.Origin.Tell(Plugin.Configuration.Translations.NoVoteInProgress);
                    break;
                case VoteResult.AlreadyVoted:
                    gameEvent.Origin.Tell(Plugin.Configuration.Translations.AlreadyVoted);
                    break;
            }
        }
        else
        {
            gameEvent.Origin.Tell(Plugin.Configuration.Translations.NoVoteInProgress);
        }
    }
}
