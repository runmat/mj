using System;
using System.Linq;
using GeneralTools.Models;
using LibGit2Sharp;

namespace VsSolutionPersister
{
    public static class GitService
    {
        static public readonly string DefaultCommitMessage = "[default commit message, please change!]";

        static readonly Signature Signature = new Signature("Matthias Jenzen", "matthias.jenzen@kroschke.de", new DateTimeOffset(DateTime.Now));


        static public void CheckoutBranch(string workingDirectoryPath, string branchNameToCheckout)
        {
            using (var repo = new Repository(workingDirectoryPath))
            {
                if (string.Equals(repo.Head.RemoteName, branchNameToCheckout, StringComparison.CurrentCultureIgnoreCase))
                    return;

                var branchToCheckout = repo.Branches.FirstOrDefault(b => b.FriendlyName == branchNameToCheckout);
                if (branchToCheckout == null)
                    return;

                Commands.Checkout(repo, branchToCheckout);
            }
        }

        public static string GetLastCommitMessage(string workingDirectoryPath)
        {
            using (var repo = new Repository(workingDirectoryPath))
            {
                var lastCommit = repo.Head.Commits.FirstOrDefault();
                if (lastCommit != null)
                    return lastCommit.Message;
            }

            return "";
        }

        static public void AmendLastCommit(string workingDirectoryPath, string commitMessage)
        {
            using (var repo = new Repository(workingDirectoryPath))
            {
                repo.Commit(commitMessage, Signature, Signature, new CommitOptions { AmendPreviousCommit = true });
            }
        }

        public static bool IsDirty(string workingDirectoryPath)
        {
            using (var repo = new Repository(workingDirectoryPath))
            {
                var status = repo.RetrieveStatus();
                return status.IsDirty;
            }
        }

        static public bool StageAndCommitWorkingDirectory(string workingDirectoryPath, string newCommitMessage)
        {
            using (var repo = new Repository(workingDirectoryPath))
            {
                var status = repo.RetrieveStatus();
                var isDirty = status.IsDirty;

                if (!isDirty)
                    return true;

                var commitMessage = (newCommitMessage.IsNotNullOrEmpty() ? newCommitMessage : DefaultCommitMessage);

                foreach (var entry in status.Modified)
                    repo.Stage(entry.FilePath);
                foreach (var entry in status.Added)
                    repo.Stage(entry.FilePath);
                foreach (var entry in status.Removed)
                    repo.Stage(entry.FilePath);
                foreach (var entry in status.RenamedInWorkDir)
                    repo.Stage(entry.FilePath);

                foreach (var entry in status.Untracked)
                {
                    if (entry.FilePath.ToLower().EndsWith(".orig"))
                        repo.Remove(entry.FilePath);
                    else
                        repo.Stage(entry.FilePath);
                }
                foreach (var entry in status.Missing)
                    repo.Remove(entry.FilePath);

                repo.Commit(commitMessage, Signature, Signature, new CommitOptions());
            }

            return true;
        }
    }
}
