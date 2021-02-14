# GitHubChangelogGenerator

This project generates a nice-looking html changelog for your github projects. 

## Available environment variables to use, docker, etc.:

Name | Required | Example | Description
--- | --- | --- | ---
GITUSERNAME | yes | "srcmkr" | As part of your repository, the username where the git is located is required
GITREPOSITORY | yes | "GitHubChangelogGenerator" | The name of the repository you want to create a changelog for
BRANCH | yes | "main" | The selected branch (should be a branch with multiple commits per release like main)
CHANGELOGNAME" | yes | "Github Changelog Generator" | This is the page title and caption in front of the changes
CHANGELOGLABEL | yes | "changelog" | Only issues tagged with this label will appear in changelog
CHANGELOGPUBLISHLABELS | yes | "enhancement,documentation,bug" | Set labels to show in changelog (in specific order)
TEMPLATE | no | "day" or "default" | Template to use for changelog
PAT | no | "abcdefghijklmnopqrstuvwxyz1234567890abcde" | If repository needs authorization, a PAT (personal access token) is required
