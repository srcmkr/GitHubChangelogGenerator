# GitHubChangelogGenerator

This project generates a nice-looking html changelog for your github projects:  
  
![Date Template](https://raw.githubusercontent.com/srcmkr/GitHubChangelogGenerator/master/docs/day-template.PNG)  
How this was generated: https://github.com/srcmkr/GitHubChangelogGenerator/issues?q=is%3Aissue+is%3Aclosed

## Required environment variables to use, docker, etc.:

Name | Example | Description
--- | --- | ---
GITUSERNAME |"srcmkr" | As part of your repository, the username where the git is located is required
GITREPOSITORY |"GitHubChangelogGenerator" | The name of the repository you want to create a changelog for
BRANCH |"main" | The selected branch (should be a branch with multiple commits per release like main)
CHANGELOGNAME" |"Github Changelog Generator" | This is the page title and caption in front of the changes
CHANGELOGLABEL |"changelog" | Only issues tagged with this label will appear in changelog
CHANGELOGPUBLISHLABELS |"enhancement,documentation,bug" | Set labels to show in changelog (in specific order)
TEMPLATE | "day" or "default" | Template to use for changelog
PAT |"abcdefghijklmnopqrstuvwxyz1234567890abcde" | If repository needs authorization, a PAT (personal access token) is required

## Full example for 
```docker run --rm -v C:\test:/app/data -e CHANGELOGNAME="Github Changelog Generator" -e CHANGELOGLABEL=changelog -e BRANCH=master -e CHANGELOGPUBLISHLABELS="enhancement,documentation,bug" -e PAT="YOURPERSONALACCESSTOKEN" -e GITUSERNAME=srcmkr -e GITREPOSITORY=GitHubChangelogGenerator -e TEMPLATE=day srcmkr/githubchangeloggeneratordocker```

**Where to create a personal access token?**  
There you are: https://github.com/settings/tokens
