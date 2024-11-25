# git-auto-commit
creates a commit on each change of the file

## supported scenarios
1) you have one "main" branch from which you create feature branches

2) you are a solo developer (I will add support for multiple people later)

## goals of the project:
This is targetting solo developers, in order to support open-development paradigm
So commit on every change

## remarks aka known issues
probably will not work with rebasing

[ ] add --verbose as a parameter

[ ] also push every commit - nah, I mean no need to do this pushing commits is not that hard and this allows for better privacy support

[ ] there are a lot of branches, I need to remove automatically them

[ ] I need to fetch "main" branch automatically

[ ] get default branch name 

[ ] run own instance of git-server with withible commit details
# Support on Patreon

https://www.patreon.com/o_kryvonos


# Join Discord server 

https://discord.gg/W5HFSKVXgc

## Building

```
dotnet run
```

## License

Copyright © Kryvonos Oleksandr 2024 <oleksandr.kryvonos.ods.ua@gmail.com>. Provided under the [MIT License](http://opensource.org/licenses/MIT).
